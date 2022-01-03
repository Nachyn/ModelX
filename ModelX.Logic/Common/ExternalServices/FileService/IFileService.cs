using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ModelX.Logic.Common.ExternalServices.FileService;

public interface IFileService
{
    Task WriteToStorageAsync(IFormFile uploadedFile, string filePath);

    void DeleteFilesFromStorage(string pathFromRoot,
        params string[] filePaths);

    FileContentResult GetFileFromStorage(string filePathFromRoot,
        string fileDownloadName);
}