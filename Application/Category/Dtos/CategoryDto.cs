using Common.Infrastructures;
using System.Collections.Generic;
using Mapster;
using Common;

namespace Application.Category.Dtos
{
    public class CategoryDto  : BaseEntityAudit , IRegister
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentCategory { get; set; }
        public string ParentCategoryId { get; set; }


        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Benaa.Category, CategoryDto>()
                .Map(dest => dest.ParentCategory, src => src.ParentCategory != null ? src.ParentCategory.Name : "");
        }
    }
}
