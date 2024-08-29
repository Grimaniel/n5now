namespace WebApp_Challenge.Models
{
    public class PermissionType
    {
        public int PermissionTypeId { get; set; }
        public string Description { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
