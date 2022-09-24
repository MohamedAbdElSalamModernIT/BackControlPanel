namespace Common.Interfaces {
  public interface IImage {
     byte[] Data { get; set; }
     string Imageurl { get; }
     string ThumbnailUrl { get; }
     string RefId { get; set; }
  }
}