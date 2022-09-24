using AutoMapper;

namespace Common.Extensions {
  public static class BaseEntityExtension {
    private static IMapper _mapper;

    public static void Configure(IMapper mapper) {
      _mapper = mapper;
    }
    public static T MapTo<T>(this IBaseEntity entity) {
      return _mapper.Map<T>(entity);
    }
  }
}