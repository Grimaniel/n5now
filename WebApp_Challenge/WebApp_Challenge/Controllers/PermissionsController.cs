using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using WebApp_Challenge.Models;
using WebApp_Challenge.Data;
using WebApp_Challenge.DTOs;
using Nest; // Para Elasticsearch
using Confluent.Kafka;
using Newtonsoft.Json;
namespace WebApp_Challenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IElasticClient _elasticClient;
        private readonly IProducer<Null, string> _kafkaProducer;

        public PermissionsController(ApplicationDbContext context, IElasticClient elasticClient, IProducer<Null, string> kafkaProducer)
        {
            _context = context;
            _elasticClient = elasticClient;
            _kafkaProducer = kafkaProducer;
        }

        private async Task SendMessageToKafkaAsync(string operation, string message)
        {
            var kafkaMessage = new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(new
                {
                    Id = Guid.NewGuid(),
                    Operation = operation,
                    Message = message
                })
            };

            await _kafkaProducer.ProduceAsync("permissions-topic", kafkaMessage);
        }


        [HttpPost("request")]
        public async Task<IActionResult> RequestPermission([FromBody] PermissionDto dto)
        {
            var permission = new Permission
            {
                EmployeeId = dto.EmployeeId,
                PermissionTypeId = dto.PermissionTypeId,
                Date = DateTime.UtcNow
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            // Indexar en Elasticsearch
            await _elasticClient.IndexDocumentAsync(permission);

            // Enviar mensaje a Kafka
            await SendMessageToKafkaAsync("request", $"Requested permission: {permission.PermissionId}");

            // Implementar el envío del mensaje aquí

            Log.Information("Requested permission: {PermissionId}", permission.PermissionId);

            return Ok(permission);
        }

        [HttpPut("modify")]
        public async Task<IActionResult> ModifyPermission([FromBody] PermissionDto dto)
        {
            var permission = await _context.Permissions.FindAsync(dto.PermissionId);

            if (permission == null)
            {
                return NotFound();
            }

            permission.EmployeeId = dto.EmployeeId;
            permission.PermissionTypeId = dto.PermissionTypeId;
            permission.Date = dto.Date;

            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();

            // Indexar en Elasticsearch
            await _elasticClient.IndexDocumentAsync(permission);

            // Enviar mensaje a Kafka
            await SendMessageToKafkaAsync("modify", $"Modified permission: {permission.PermissionId}");

            // Implementar el envío del mensaje aquí

            Log.Information("Modified permission: {PermissionId}", permission.PermissionId);

            return Ok(permission);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetPermissions(int id)
        {
            var permissions = await _context.Permissions
                .Where(p => p.EmployeeId == id)
                .ToListAsync();

            if (permissions == null || !permissions.Any())
            {
                return NotFound();
            }

            // Enviar mensaje a Kafka
            await SendMessageToKafkaAsync("get", $"Retrieved permissions for employee: {id}");

            // Implementar el envío del mensaje aquí

            Log.Information("Retrieved permissions for employee: {EmployeeId}", id);

            return Ok(permissions);
        }

    }
}
