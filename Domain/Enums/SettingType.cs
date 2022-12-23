namespace Domain.Enums
{
    public enum SettingType
    {
        String,
        Number,
        Boolean
    }

    public enum FileType
    {
        Autocad,
        Revit,
    }

    public enum DrawingType
    {
        Structure,
        Architectural,
    }
    public enum DrawingStatus
    {
        Pending,
        Submitted,
    }

    public enum ConditionStatus
    {
        Success,
        Fail,
        Other,
    }
}