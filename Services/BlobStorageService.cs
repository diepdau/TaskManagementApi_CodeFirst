using TaskManagementApi.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
namespace TaskManagementApi.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "task-attachments";

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            try
            {
                var blobClient = blobContainer.GetBlobClient(fileName);
                return await blobClient.DeleteIfExistsAsync();
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("HTTP error code {0}: {1}",
                                    e.Status, e.ErrorCode);
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return false;
            }

        }
        public async Task<FileStreamResult> DownloadFileAsync(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
            {
                return null; 
            }
            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Position = 0; 

            var contentType = (await blobClient.GetPropertiesAsync()).Value.ContentType;
            return new FileStreamResult(memoryStream, contentType)
            {
                FileDownloadName = fileName 
            };
        }


    }
}
