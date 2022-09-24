using Domain.Attributes;

namespace Domain.Enums {
  public enum SettingKeys {
  [DescribeSetting("session_expiry",SettingType.Number,"8")]
    SessionExpiry,
  [DescribeSetting("outlet_max_distance",SettingType.Number,"100")]
    OutletPromoterMaxDistance,
  }
}