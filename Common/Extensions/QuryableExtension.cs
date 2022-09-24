using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Extensions {
  public static class QuryableExtension {
    public static IQueryable<TSource> Protected<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
      var resulte = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
      if (resulte) {
        source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
      }
      source = source.Where(predicate);
      return source;
    }

    public static IQueryable<TSource> Protected<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) {
      var resulte = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
      if (resulte) {
        source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
      }
      source = source.Where(predicate);
      return source;
    }
    public static IQueryable<TSource> Protected<TSource>(this IQueryable<TSource> source) {
      var resulte = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
      if (resulte) {
        source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
      }

      return source;
    }


    public static IEnumerable<TSource> WhereProtected<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) {
      var resulte = typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
      if (resulte) {
        source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
      }
      source = source.Where(predicate);
      return source;
    }

    public static IEnumerable<TSource> WhereProtected<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
     var resulte=typeof(IDeleteEntity).IsAssignableFrom(typeof(TSource));
      if (resulte) {
        source = source.Where(s => !((IDeleteEntity)s).IsDeleted);
      }
      source = source.Where(predicate);
      return source;
    }
  }
}