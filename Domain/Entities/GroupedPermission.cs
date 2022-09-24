using System.Collections.Generic;

namespace Domain.Entities {
    public class GroupedPermission {
        public string Name { get; set; }
        public List<string> Permissions { get; set; }
    }
}
