﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
using System.Linq;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-attachments")]
    [ApiController]
    public class TaskAttachmentController : ControllerBase
    {
        private readonly ITaskAttachmentRepository _attachmentRepository;
        private readonly IBlobStorageService _blobService;
        private readonly IGenericRepository<Models.Task> _taskRepository;

        public TaskAttachmentController(ITaskAttachmentRepository attachmentRepository, IBlobStorageService blobService, IGenericRepository<Models.Task> taskRepository)
        {
            _attachmentRepository = attachmentRepository;
            _blobService = blobService;
            _taskRepository = taskRepository;
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

        [HttpPost("{taskId}")]
        public async Task<IActionResult> UploadAttachments(int taskId, IFormFileCollection files)
        {
            var tasksLabel = await _taskRepository.GetAsync(tl => tl.Id == taskId);
            if (tasksLabel == null)
                return NotFound("Task not found.");

            if (files == null || files.Count == 0)
            {
                return BadRequest("No file uploaded.");
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

        [HttpGet("[action]/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var fileResult = await _blobService.DownloadFileAsync(fileName);

            if (fileResult == null)
            {
                return NotFound("File not found.");
            }

            return fileResult;
        }



    }
}
