using Common.Attributes;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum UserType
    {
        [Description("أخرى")]
        [Value(new int[] { 1, 2, 3 })]
        Other,

        [Description("مسئول الأمانة")]
        [Value(new int[] { 2, 3, 4 })]
        AmanaManager,

        [Description("مسئول البلدية")]
        [Value(new int[] { 3 })]
        BaldiaManager,

        [Description("موظف البلدية")]
        [Value(new int[0])]
        BaladiaEmployee,

        [Description("مدير مكتب")]
        [Value(new int[] { 5 })]
        OfficeManager,

        [Description("مهندس المكتب")]
        [Value(new int[0])]
        Engineer,
    }
}