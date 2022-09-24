using Application.Conditions.Dtos;
using System.Collections.Generic;

namespace Application.Category.Dtos
{
    public class CategoriesResultDto
    {
        public bool IsConditions { get; set; } = false;
        public List<CategoryDto> Categories { get; set; }
        public List<ConditionsMapDto> Conditions { get; set; }
    }
}
