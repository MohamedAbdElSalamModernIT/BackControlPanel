using Domain.Entities.Auth;
using Domain.Enums;

namespace Domain.Entities.Benaa
{
    public class Client
    {
        public string UserId { get; set; }
        public int? BaladiaId { get; set; }
        public int? AmanaId { get; set; }

        public Office Office { get; set; } 
        public string OfficeId { get; set; } 
        public AppUser User { get; set; } 
    }

}