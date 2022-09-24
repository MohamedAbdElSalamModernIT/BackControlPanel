namespace Common.Interfaces
{
  public interface IPaging
  {
    int Page { get; set; }
    int PageSize { get; set; }
    string SortBy { get; set; }
    string SortOrder { get; set; }
    string Filter { get; set; }
  }
}