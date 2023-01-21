using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Options;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public class FileService : IFileService
{
    private readonly ImageOption _imgOption;
    private readonly IHostingEnvironment _env;
    private readonly ILogger<FileService> _logger;
    private readonly IAppDbContext _context;


    public FileService(IOptions<ImageOption> imgOption, IHostingEnvironment env, IAppDbContext context, ILogger<FileService> logger)
    {
        _imgOption = imgOption.Value;
        _env = env;
        _logger = logger;
        _context = context;
    }

    public async Task<Result> RemoveFile(string url)
    {
        await RemoveFileAsync(url);
        return Result.Successed("File Removed Successfully");
    }
    public async Task<List<AppFile>> SaveFilesAsync(IEnumerable<IFile> dtoFiles)
    {
        var appFiles = new List<AppFile>();
        foreach (var dtoFile in dtoFiles)
        {
            var file = await SaveFileAsync(dtoFile);
            appFiles.Add(file);
        }
        return appFiles;
    }

    public async Task<AppFile> SaveFileAsync(IFile dtoFile)
    {
        try
        {
            return await SaveNewFileAsync(dtoFile);
            // if (appFile == null)
            //   return Result.Successed();
            // var result = appFile.Adapt<FileDto>();
            // return Result.Successed(result);
            // re
        }
        catch (ApiException e)
        {
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
        //   
        // }catch(ApiException e) {
        //     return Result.Failure(ApiExeptionType.FaildSaveImage,e.Message);
        //   } catch(Exception e) {
        //     return Result.Failure(ApiExeptionType.FaildSaveImage,e.Message);
        //   }
    }

    private async Task<AppFile?> SaveNewFileAsync(IFile dtoFile)
    {

        if (!string.IsNullOrEmpty(dtoFile.Url))
            await RemoveFileAsync(dtoFile.Url);

        if (string.IsNullOrEmpty(dtoFile.Base64))
            return null;
        // Check byte array is filled 
        // dtoFile.Id = null;
        var bytes = GetDataBytes(dtoFile.Base64);

        if (ValidateImageFormat(bytes, out string validationError) == false)
        {
            throw new ApiException(ApiExceptionType.InvalidImage, validationError);
        }

        var appFile = new AppFile()
        {
            Name = "",
            Data = bytes,

        };

        await _context.AppFiles.AddAsync(appFile);
        appFile.Url =  $"r/{appFile.Id}";
        if (!_imgOption.SaveInDatabse)
        {
            string path = Path.Combine(_env.WebRootPath, "Image");
            CreateFolders(path);
            var imageUrl = Path.Combine(path, appFile.Id);
            await File.WriteAllBytesAsync(imageUrl, appFile.Data);
            appFile.Data = null!;
        }

        return appFile;
    }

    public async Task RemoveFileAsync(string fileUrl)
    {
        var appFile = await _context.AppFiles.Where(e => e.Url == fileUrl).FirstOrDefaultAsync();

        if (appFile != null)
        {
            _context.AppFiles.Remove(appFile);
            DeleteImageIfExist(appFile.Id);
        }

    }

    private byte[] GetDataBytes(string base64)
    {
        if (base64.Length <= 0) return new byte[] { };

        if (!string.IsNullOrEmpty(base64))
        {
            return ConvertBase64ToByteArr(base64);
        }
        throw new Exception();
    }

    private byte[] ConvertBase64ToByteArr(string base64)
    {
        if (IsBase64String(base64, out var file))
        {
            var bytes = Convert.FromBase64String(file);
            return bytes;
        }

        throw new ApiException(ApiExceptionType.InvalidImage);
    }

    public async void ConvertFormFileToByteArr(IFile dtoFile)
    {
        var bytes = new byte[] { };
        await using (var memorySteam = new MemoryStream())
        {
            //await dtoFile.File.CopyToAsync(memorySteam);
            bytes = memorySteam.ToArray();
            // dtoFile.Bytes = bytes;
        }
    }


    public static bool IsBase64String(string base64, out string imageString)
    {
        if (base64.Contains(','))
        {
            base64 = base64.Split(',')[1];
        }

        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        imageString = base64;
        return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
    }

    bool ValidateImageFormat(byte[] bytes, out string validationError)
    {
        validationError = "";
        if (bytes == null) return true;

        if (bytes.Length / 1024 > _imgOption.AllowedSizeInKB)
        {
            validationError = "File is too large";
            return false;
        }

        return GetImageFormat(bytes) != ImageFormat.Exif;
    }


    private ImageFormat GetImageFormat(byte[] bytes)
    {
        try
        {
            var bmp = Encoding.ASCII.GetBytes("BM"); // BMP
            var gif = Encoding.ASCII.GetBytes("GIF"); // GIF
            var png = new byte[] { 137, 80, 78, 71 }; // PNG
            var tiff = new byte[] { 73, 73, 42 }; // TIFF
            var tiff2 = new byte[] { 77, 77, 42 }; // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.Bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.Gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.Png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.Tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.Tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.Jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.Jpeg;


            using MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image.FromStream(ms).Dispose();
            return ImageFormat.Jpeg;
        }
        catch (ArgumentException)
        {
            return ImageFormat.Exif;
        }
    }


    private void DeleteImageIfExist<T>(T id)
    {
        var imageDir = Path.Combine(_env.WebRootPath, _imgOption.CachedImageDir);

        try
        {
            if (Directory.Exists(imageDir))
            {
                var files = Directory.GetFiles(imageDir, $"{id}*");
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error to delete image with id: {id}", ex.Message);
            throw new ApiException(ApiExceptionType.FaildDeleteImage);
        }
    }

    private void CreateFolders(string foldersPath)
    {
        var exists = Directory.Exists(foldersPath);
        if (!exists)
            Directory.CreateDirectory(foldersPath);
    }


}