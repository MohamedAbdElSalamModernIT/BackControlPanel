using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Infrastructures;
using Common.Interfaces;
using X.PagedList;

namespace Common.Extensions {
  public static class PageListExtensions {
    public static async Task<PageList<T>> ToPagedListAsync<T>(this IQueryable<T> query, IPaging command, CancellationToken cancellationToken = default) {
      var (page, pageSize) = (1, 10);

      page = command.Page > 0 ? command.Page : page;
      pageSize = command.PageSize > 0 ? command.PageSize : pageSize;
      var result = await query.ToPagedListAsync(page, pageSize, cancellationToken);

      return new PageList<T>(result);
    }

    public static PageList<T> ToPagedList<T>(this List<T> query, IPaging command, CancellationToken cancellationToken = default) {
      var (page, pageSize) = (1, 10);

      page = command.Page > 0 ? command.Page : page;
      pageSize = command.PageSize > 0 ? command.PageSize : pageSize;
      var result = query.ToPagedList(page, pageSize);

      return new PageList<T>(result);
    }
  }
}