namespace Infrastructure
{
    public interface IFile
    {
        string Extension { get; set; }
        string Url { get; set; }
        string Base64 { get; set; }
    }


    public class FileDto : IFile
    {
        public string Extension { get; set; }
        public string Url { get; set; }
        public FileStatus FileStatus { get; set; }
        public string Base64 { get; set; }
    }
    public enum FileStatus
    {
        None,
        New,
        Modify,
        Remove
    }
}
