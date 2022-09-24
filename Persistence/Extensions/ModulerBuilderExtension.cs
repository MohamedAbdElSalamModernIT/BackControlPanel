using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace Persistence.Extensions {
  public static class ModelBuilderExtensions {
    public static ModelBuilder UseValueConverterForType<T>(this ModelBuilder modelBuilder, ValueConverter converter) {
      return modelBuilder.UseValueConverterForType(typeof(T), converter);
    }


    public static ModelBuilder UseValueConverterForType(this ModelBuilder modelBuilder, Type type, ValueConverter converter) {
      // iterate all entities and find all fields of specific type, then set value converter for them
      foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
        var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);
        foreach (var property in properties) {
          modelBuilder.Entity(entityType.Name).Property(property.Name)
              .HasConversion(converter);
        }
      }

      return modelBuilder;
    }
  }
}
