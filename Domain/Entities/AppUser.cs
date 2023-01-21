using Domain.Entities.Auth;
using Domain.Entities.Benaa;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class AppUser : User
    {
        public int? BaladiaId { get; set; }
        public int? AmanaId { get; set; }

        public Office Office { get; set; }
        public string OfficeId { get; set; }
        public Office OwnerOffice { get; set; }
        public HashSet<Drawing> Drawings { get; set; } = new HashSet<Drawing>();
    }
}