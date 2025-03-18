using Microsoft.AspNetCore.Mvc;

namespace TaskManagementApi.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string fileName);
        Task<FileStreamResult> DownloadFileAsync(string fileName);
    }
}
