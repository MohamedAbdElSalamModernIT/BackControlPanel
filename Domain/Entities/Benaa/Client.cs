using Domain.Enums;

namespace Domain.Entities.Benaa
{
    public class Client
    {
        public string IdentityId { get; set; }
        public string OfficeName { get; set; } = "";
        public int BaladiaId { get; set; }
        public int AmanaId { get; set; }
        public UserType? Type { get; set; }
    }

}