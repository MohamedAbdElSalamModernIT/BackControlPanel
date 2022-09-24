using X.PagedList;

namespace Common.Infrastructures {

  public class PageList {
    public PagedListMetaData Metadata { get; set; }
  }
  public  class PageList<T>:PageList {

      public IPagedList<T> Items { get; set; }
    public PageList(IPagedList<T> listPage) {
      Items = listPage;
      Metadata = listPage.GetMetaData();
    }
    
  }
}