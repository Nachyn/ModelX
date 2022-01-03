using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.ExternalServices.FileService;

namespace ModelX.Infrastructure.Services.FileService;

public class FileService : IFileService
{
    private readonly RootFileFolderDirectory _rootDirectory;

    public FileService(IOptions<RootFileFolderDirectory> rootDirectory)
    {
        _rootDirectory = rootDirectory.Value;
    }

    public async Task WriteToStorageAsync(IFormFile uploadedFile, string filePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        await using var fileStream = new FileStream(filePath, FileMode.CreateNew);
        await uploadedFile.CopyToAsync(fileStream);
    }

    public void DeleteFilesFromStorage(string pathFromRoot,
        params string[] filePaths)
    {
        if (pathFromRoot == null)
        {
            throw new ArgumentNullException(nameof(pathFromRoot));
        }

        var countCatalogs = GetCountCatalogs(pathFromRoot);

        foreach (var filePath in filePaths)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                continue;
            }

            DeleteFromStorage(Path.Combine(_rootDirectory.RootFileFolder,
                pathFromRoot, filePath), countCatalogs);
        }
    }

    public FileContentResult GetFileFromStorage(string filePathFromRoot,
        string fileDownloadName)
    {
        if (string.IsNullOrWhiteSpace(filePathFromRoot))
        {
            throw new ArgumentNullException(nameof(filePathFromRoot));
        }

        var fileInfo = new FileInfo(Path.Combine(_rootDirectory.RootFileFolder,
            filePathFromRoot));

        new FileExtensionContentTypeProvider().TryGetContentType(fileInfo.Name,
            out var contentType);

        var bytes = File.ReadAllBytes(fileInfo.FullName);

        return new FileContentResult(bytes, contentType ?? "application/octet-stream")
        {
            FileDownloadName = fileDownloadName
        };
    }

    private int GetCountCatalogs(string path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? 0
            : Regex.Split(path, "[\\\\*|/+]").Count(s => s.Length > 0);
    }

    /// <summary>
    ///     Delete the file and the parent directory if it is empty.
    /// </summary>
    /// <param name="fileFullPath">File path</param>
    /// <param name="rootCatalogsDepth">
    ///     0 - do not delete the directory.
    ///     1 - delete the current directory and so on.
    /// </param>
    private void DeleteFromStorage(string fileFullPath, int rootCatalogsDepth)
    {
        var file = new FileInfo(fileFullPath);
        if (!file.Exists)
        {
            return;
        }

        File.Delete(file.FullName);

        var directory = new DirectoryInfo(file.DirectoryName);

        for (var i = 0; i < rootCatalogsDepth; i++)
        {
            if (directory.EnumerateFiles("*.*", SearchOption.AllDirectories).Any())
            {
                break;
            }

            directory.Delete(true);
            directory = directory.Parent;
        }
    }
}