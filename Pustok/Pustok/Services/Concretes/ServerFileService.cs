using Pustok.Contracts;
using Pustok.Services.Abstracts;
using System.IO;

namespace Pustok.Services.Concretes;



public class ServerFileService : IFileService
{
   

    public ServerFileService()
    {

    }

    public string Upload(IFormFile file, string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = path + uniqueFileName;

        using var fileStream = new FileStream(fullPath, FileMode.Create);
        file.CopyTo(fileStream);

        return uniqueFileName;
    }

    public string Upload(IFormFile file, CustomUploadDirectories uploadDirectory)
    {
        var path = GetStaticFilesDirectory(uploadDirectory);

        return Upload(file, path);
    }

    public string GetStaticFilesDirectory(CustomUploadDirectories uploadDirectory)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", uploadDirectory.ToString());
    }
}
