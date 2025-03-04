using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAttachmentController : ControllerBase
    {
        private readonly ITaskAttachmentRepository _attachmentRepo;
        private readonly IBlobStorageService _blobService;

        public TaskAttachmentController(ITaskAttachmentRepository attachmentRepo, IBlobStorageService blobService)
        {
            _attachmentRepo = attachmentRepo;
            _blobService = blobService;
        }

        [HttpPost("{taskId}")]
        public async Task<IActionResult> UploadAttachment(int taskId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            string fileUrl = await _blobService.UploadFileAsync(file);

            var attachment = new TaskAttachment
            {
                TaskId = taskId,
                FileName = file.FileName,
                FileUrl = fileUrl,
                UploadedAt = DateTime.UtcNow
            };

            await _attachmentRepo.AddAttachment(attachment);
            return Ok(attachment);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetAttachments(int taskId)
        {
            var attachments = await _attachmentRepo.GetAttachmentsByTaskId(taskId);
            return Ok(attachments);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            var attachment = await _attachmentRepo.GetAttachmentsByTaskId(id);
            if (attachment == null)
            {
                return NotFound("Attachment not found.");
            }

            await _blobService.DeleteFileAsync(attachment.First().FileName);
            await _attachmentRepo.DeleteAttachment(id);

            return Ok(new { message = "Attachment deleted successfully.", Attachment = id });
        }
    }
}
