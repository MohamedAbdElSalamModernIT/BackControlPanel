using System.ComponentModel;

namespace Domain.Enums
{
    public enum UserType
    {
        [Description("مسئول الأمانة")]
        AmanaManager,

        [Description("مسئول البلدية")]
        BaldiaManager,

        [Description("موظف البلدية")]
        BaladiaEmployee,

        [Description("عميل")]
        Client,

        [Description("أخرى")]
        Other,
    }
}