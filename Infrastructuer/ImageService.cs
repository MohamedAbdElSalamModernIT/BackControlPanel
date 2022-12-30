using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Options;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Infrastructure.WriterHelper;

namespace Infrastructure {
  public class ImageService : IImageService {
    private readonly IHttpContextAccessor _httpContext;
    private readonly IHostingEnvironment _env;
    private readonly ILogger<ImageService> _logger;
    private readonly ImageOption _imgOption;

    public ImageService(IHttpContextAccessor httpContext, IHostingEnvironment env, IOptions<ImageOption> imgOption, ILogger<ImageService> logger) {
      _httpContext = httpContext;
      _env = env;
      _logger = logger;
      _imgOption = imgOption.Value;
    }

    public async Task<Image> SaveImageAsync(Image image, byte[] dtoFile) {
      try {
        //if (ValidateImageFormate(dtoFile, out string validationError) == false) {
        //  throw new ApiException(ApiExceptionType.InvalidImage, validationError);
        //}
        if (_imgOption.SaveInDatabse)
          image.Data = dtoFile;
        else {
          string path = Path.Combine(_env.WebRootPath, "Image");
          CreateFolders(path);
          var imageUrl = Path.Combine(path, image.Id);
          await File.WriteAllBytesAsync(imageUrl, dtoFile);
        }
        if (!string.IsNullOrEmpty(image.Id))
          DeleteImageIfExist(image.Id);
        return image;
      } catch (ApiException e) {
        throw e;
      } catch (Exception e) {
        throw new ApiException(ApiExceptionType.FaildSaveImage, e.Message);
      }
    }

    public async Task<Image> SaveImageAsync(Image image, string dtoFile) {
      if (IsBase64String(dtoFile, out var file)) {
        var bytes = Convert.FromBase64String(file);
        return await SaveImageAsync(image, bytes);
      }
      throw new ApiException(ApiExceptionType.InvalidImage);
    }


    public async Task<Image> SaveImageAsync(Image image, IFormFile dtoFile) {
      var bytes = new byte[] { };
      await using (var memorySteam = new MemoryStream()) {
        await dtoFile.CopyToAsync(memorySteam);
        bytes = memorySteam.ToArray();
      }

      return (await SaveImageAsync(image, bytes));
    }

    private void DeleteImageIfExist<T>(T id) {
      var imageDir = Path.Combine(_env.WebRootPath, _imgOption.CachedImageDir);

      try {
        var files = Directory.GetFiles(imageDir, $"{id}*");
        foreach (var file in files) {
          File.Delete(file);
        }
      } catch (Exception ex) {
        _logger.LogError($"Error to delete image with id: {id}", ex.Message);
        throw new ApiException(ApiExceptionType.FaildDeleteImage);
      }
    }
    public static bool IsBase64String(string base64, out string imageString) {
      if (base64.Contains(',')) {
        base64 = base64.Split(',')[1];
      }
      Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
      imageString = base64;
      return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);

    }
    bool ValidateImageFormate(byte[] bytes, out string validationError) {
      validationError = "";
      if (bytes == null) return true;

      if (bytes.Length / 1024 > _imgOption.AllowedSizeInKB) {
        validationError = "File is too large";
        return false;
      }

      return GetImageFormat(bytes) != ImageFormat.unknown;
    }


    private ImageFormat GetImageFormat(byte[] bytes) {
      var bmp = Encoding.ASCII.GetBytes("BM"); // BMP
      var gif = Encoding.ASCII.GetBytes("GIF"); // GIF
      var png = new byte[] { 137, 80, 78, 71 }; // PNG
      var tiff = new byte[] { 73, 73, 42 }; // TIFF
      var tiff2 = new byte[] { 77, 77, 42 }; // TIFF
      var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
      var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

      if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
        return ImageFormat.bmp;

      if (gif.SequenceEqual(bytes.Take(gif.Length)))
        return ImageFormat.gif;

      if (png.SequenceEqual(bytes.Take(png.Length)))
        return ImageFormat.png;

      if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
        return ImageFormat.tiff;

      if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
        return ImageFormat.tiff;

      if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
        return ImageFormat.jpeg;

      if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
        return ImageFormat.jpeg;
      return ImageFormat.unknown;
    }

    //oldService 
    public string GetImagePath(string imageFullPath) {
      return $"{_httpContext.HttpContext?.Request?.Scheme}/{_httpContext.HttpContext?.Request?.Host.Value}/{imageFullPath} ";
    }




    public async Task<string> UploadImg(string base64, string folderName, string fileName) {
      return await UploadImg(Convert.FromBase64String(base64), folderName, fileName);
    }

    public async Task<string> UploadImg(byte[] img, string folderName, string fileName = null) {
      try {
        if (string.IsNullOrEmpty(fileName)) {
          fileName = Path.GetRandomFileName();
        }

        string path = Path.Combine(_env.WebRootPath, "Image", folderName);
        CreateFolders(path);
        var imageUrl = Path.Combine(path, fileName);
        await File.WriteAllBytesAsync(imageUrl, img);
        return imageUrl;
      } catch (Exception) {
        throw;
      }
    }

    public async Task<string> UploadImg(IFormFile file, string folderName) {
      try {
        if (file.Length > 0) {
          byte[] fileBytes;
          await using (var ms = new MemoryStream()) {
            await file.CopyToAsync(ms);
            fileBytes = ms.ToArray();
          }
          return await UploadImg(fileBytes, folderName);
        }
        return null;
      } catch (Exception) {
        throw;
      }
    }

    public Task<Image> AddImage(string base64, string folderName, string fileName) {
      ///Todo
      throw new NotImplementedException();
    }

    private void CreateFolders(string foldersPath) {
      var exists = Directory.Exists(foldersPath);
      if (!exists)
        Directory.CreateDirectory(foldersPath);
    }

    private bool CheckIfImageFile(IFormFile file) {
      byte[] fileBytes;
      using (var ms = new MemoryStream()) {
        file.CopyTo(ms);
        fileBytes = ms.ToArray();
      }

      return WriterHelper.GetImageFormat(fileBytes) != WriterHelper.ImageFormat.unknown;
    }

    private bool CheckIfImageFile(byte[] fileBytes) {
      return WriterHelper.GetImageFormat(fileBytes) != WriterHelper.ImageFormat.unknown;
    }
  }

  public class WriterHelper {
    public enum ImageFormat {
      bmp,
      jpeg,
      gif,
      tiff,
      png,
      unknown
    }

    public static ImageFormat GetImageFormat(byte[] bytes) {
      var bmp = Encoding.ASCII.GetBytes("BM"); // BMP
      var gif = Encoding.ASCII.GetBytes("GIF"); // GIF
      var png = new byte[] { 137, 80, 78, 71 }; // PNG
      var tiff = new byte[] { 73, 73, 42 }; // TIFF
      var tiff2 = new byte[] { 77, 77, 42 }; // TIFF
      var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
      var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

      if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
        return ImageFormat.bmp;

      if (gif.SequenceEqual(bytes.Take(gif.Length)))
        return ImageFormat.gif;

      if (png.SequenceEqual(bytes.Take(png.Length)))
        return ImageFormat.png;

      if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
        return ImageFormat.tiff;

      if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
        return ImageFormat.tiff;

      if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
        return ImageFormat.jpeg;

      if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
        return ImageFormat.jpeg;

      return ImageFormat.unknown;
    }
  }
}