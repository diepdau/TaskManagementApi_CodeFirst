using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAttachmentController : ControllerBase
    {
        private readonly ITaskAttachmentRepository _attachmentRepository;
        private readonly IBlobStorageService _blobService;

        public TaskAttachmentController(ITaskAttachmentRepository attachmentRepository, IBlobStorageService blobService)
        {
            _attachmentRepository = attachmentRepository;
            _blobService = blobService;
        }



        [HttpPost("{taskId}")]
        public async Task<IActionResult> UploadAttachments(int taskId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            List<TaskAttachment> attachments = new List<TaskAttachment>();

            foreach (var file in files)
            {
                string fileUrl = await _blobService.UploadFileAsync(file);

                var attachment = new TaskAttachment
                {
                    TaskId = taskId,
                    FileName = file.FileName,
                    FileUrl = fileUrl,
                    UploadedAt = DateTime.UtcNow
                };

                await _attachmentRepository.AddAttachment(attachment);
                attachments.Add(attachment);
            }

            return Ok(attachments);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetAttachments(int taskId)
        {
            var attachments = await _attachmentRepository.GetAttachmentsByTaskId(taskId);
            return Ok(attachments);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttach(int id)
        {
            var attachment = await _attachmentRepository.GetAttachmentsById(id);
            if (attachment == null)
            {
                return NotFound("Attachment not found.");
            }
            var namefile = await _blobService.DeleteFileAsync(attachment.FileName);

            await _attachmentRepository.DeleteAttachment(id);

            return Ok(new { message = "Attachment deleted successfully.", Attachment = id });
        }

        //[HttpPost("{taskId}")]
        //public async Task<IActionResult> UploadAttachment(int taskId, IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("File is empty.");
        //    }

        //    string fileUrl = await _blobService.UploadFileAsync(file);

        //    var attachment = new TaskAttachment
        //    {
        //        TaskId = taskId,
        //        FileName = file.FileName,
        //        FileUrl = fileUrl,
        //        UploadedAt = DateTime.UtcNow
        //    };

        //    await _attachmentRepository.AddAttachment(attachment);
        //    return Ok(attachment);
        //}
    }
}
