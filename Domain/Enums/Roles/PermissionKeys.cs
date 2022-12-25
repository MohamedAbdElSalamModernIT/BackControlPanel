using Common.Attributes;

namespace Domain.Enums.Roles {
    public enum PermissionKeys : short {
        ///======================UserManagementModule Permissions=============================================

        //User Module Permissions
        [DescribePermission(SystemModule.UserManagementModule, "view_user", "view_user", "Users")]
        ViewUser = 1,        

        [DescribePermission(SystemModule.UserManagementModule, "add_user", "add_user", "Users")]
        AddUser,
        [DescribePermission(SystemModule.UserManagementModule, "edit_user", "edit_user", "Users")]
        EditUser,
        [DescribePermission(SystemModule.UserManagementModule, "remove_user", "remove_user", "Users")]
        RemoveUser,


        //Role Module Permissions
        [DescribePermission(SystemModule.UserManagementModule, "view_role", "view_role", "Roles")]
        ViewRole,        

        [DescribePermission(SystemModule.UserManagementModule, "add_role", "add_role", "Roles")]
        AddRole,
        [DescribePermission(SystemModule.UserManagementModule, "edit_role", "edit_role", "Roles")]
        EditRole,
        [DescribePermission(SystemModule.UserManagementModule, "remove_role", "remove_role", "Roles")]
        RemoveRole,


        //Category Module Permissions
        [DescribePermission(SystemModule.Category, "read_category", "read_category", "Categories")]
        ReadCategory,
        [DescribePermission(SystemModule.Category, "add_category", "add_category", "Categories")]
        AddCategory,
        [DescribePermission(SystemModule.Category, "edit_category", "edit_category", "Categories")]
        EditCategory,
        [DescribePermission(SystemModule.Category, "remove_category", "remove_category", "Categories")]
        RemoveCategory,


        //condition Module Permissions
        [DescribePermission(SystemModule.Condition, "view_condition_map", "view_condition_map", "conditions")]
        ViewConditionMap,
        [DescribePermission(SystemModule.Condition, "add_condition_map", "add_condition_map", "conditions")]
        AddConditionMap,
        [DescribePermission(SystemModule.Condition, "edit_condition_map", "edit_condition_map", "conditions")]
        EditConditionMap,
        [DescribePermission(SystemModule.Condition, "remove_condition_map", "remove_condition_map", "conditions")]
        RemoveConditionMap,

        [DescribePermission(SystemModule.Condition, "read_condition", "read_condition", "conditions")]
        ReadCondition,
        [DescribePermission(SystemModule.Condition, "edit_conditions", "edit_conditions", "conditions")]
        EditCondition,

        //baladya Module Permissions
        [DescribePermission(SystemModule.Baladya, "read_baladya", "read_baladya", "Baladyat")]
        ReadBaladya,
        [DescribePermission(SystemModule.Baladya, "add_baladya", "add_baladya", "Baladyat")]
        AddBaladya,
        [DescribePermission(SystemModule.Baladya, "edit_baladya", "edit_baladya", "Baladyat")]
        EditBaladya,
        [DescribePermission(SystemModule.Baladya, "remove_baladya", "remove_baladya", "Baladyat")]
        RemoveBaladya,


        //amana Module Permissions
        [DescribePermission(SystemModule.Amana, "read_amana", "read_amana", "Amanat")]
        ReadAmana,
        [DescribePermission(SystemModule.Amana, "add_amana", "add_amana", "Amanat")]
        AddAmana,
        [DescribePermission(SystemModule.Amana, "edit_amana", "edit_amana", "Amanat")]
        EditAmana,
        [DescribePermission(SystemModule.Amana, "remove_amana", "remove_amana", "Amanat")]
        RemoveAmana,

        //Information Module Permissions
        [DescribePermission(SystemModule.Information, "edit_information", "edit_information", "Information")]
        EditInformation,

        [DescribePermission(SystemModule.Information, "read_information", "read_information", "Information")]
        ReadInformation,
        
        //Drawings Module Permissions
        [DescribePermission(SystemModule.Drawings, "read_drawings", "read_drawings", "Drawings")]
        ReadDrawings,



    }
}