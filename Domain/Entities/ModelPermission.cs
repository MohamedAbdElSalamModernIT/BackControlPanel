using System.Collections.Generic;

namespace Domain.Entities {

    public class ModulePermission {
        public string Name { get; set; }
        public List<GroupedPermission> Permissions { get; set; }
    }  
}
