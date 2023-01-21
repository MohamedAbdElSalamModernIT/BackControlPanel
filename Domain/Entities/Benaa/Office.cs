using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Benaa
{
    public class Office : BaseEntityAudit, IDeleteEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public int AmanaId { get; set; }
        public Amana Amana { get; set; }

        public string OwnerId { get; set; }
        public AppUser Owner { get; set; }

        public HashSet<AppUser> Engineers { get; set; } = new HashSet<AppUser>();
        public HashSet<Drawing> Drawings { get; set; } = new HashSet<Drawing>();
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
