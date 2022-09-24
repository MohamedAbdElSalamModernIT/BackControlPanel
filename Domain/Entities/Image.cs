using Common;
using Common.Attributes;
using Common.Interfaces;

namespace Domain.Entities {
  [NotGenerated]
  public class Image:BaseEntity,IImage {
    public string Id { get; set; }
    public byte[] Data { get; set; }
    public string Imageurl => $"r/{Id}";//?s=full
    public string ThumbnailUrl => $"r/{Id}";//?s=thumb
    public string RefId { get; set; }
  }
}