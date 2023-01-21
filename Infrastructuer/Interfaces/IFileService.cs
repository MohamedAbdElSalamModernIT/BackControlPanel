using Common;
using Domain.Entities;
using System.Threading.Tasks;

namespace Infrastructure;


public interface IFileService
{
    Task<AppFile> SaveFileAsync(IFile dtoFile);

    Task<Result> RemoveFile(string url);

}