namespace WebApp_Challenge.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public int EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime Date { get; set; }
        public Employee Employee { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
