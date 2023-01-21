using Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppFile : BaseEntityAudit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Url { get; set; }
        private string CacheUrl => $"r/{Id}";
    }
}
