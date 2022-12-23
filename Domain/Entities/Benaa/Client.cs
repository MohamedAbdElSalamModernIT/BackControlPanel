namespace Domain.Entities.Benaa
{
    public class Client
    {
        public string IdentityId { get; set; }
        public string OfficeName { get; set; } = "";
    }
    
    public class Employee
    {
        public string IdentityId { get; set; }
        public int BaladiaId { get; set; }
    }
}