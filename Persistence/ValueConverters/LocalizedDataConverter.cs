using System;
using System.Linq.Expressions;
using Common.Infrastructures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Persistence.ValueConverters {
  public class LocalizedDataConverter : ValueConverter<LocalizedData, string> {
    public LocalizedDataConverter() : base(ToStoreExpr, FromStoreExpr) {

    }

    static Expression<Func<LocalizedData, string>> ToStoreExpr = x => JsonConvert.SerializeObject(x, EntityConventions.Settings);

    static Expression<Func<string, LocalizedData>> FromStoreExpr = x => ConvertToLocalizedData(x);


    private static LocalizedData ConvertToLocalizedData(string data) {
      try {
        // try to deserialize to LocalizedData
        return JsonConvert.DeserializeObject<LocalizedData>(data, EntityConventions.Settings);
      } catch (Exception) {
        return new LocalizedData();
      }
    }
    public static class CustomDbFunctions {
      [DbFunction("JSON_VALUE", "")]
      public static string JsonValue(string source, string path) => throw new NotSupportedException();
    }

  }
}