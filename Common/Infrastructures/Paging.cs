using Common.Interfaces;

namespace Common.Infrastructures {
  public class Paging :IPaging{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
    public string Filter { get; set; }


  }
}