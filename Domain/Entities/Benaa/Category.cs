using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Benaa
{
    public class Category : BaseEntityAudit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public HashSet<Category> Categories{ get; set; }

    }


}
