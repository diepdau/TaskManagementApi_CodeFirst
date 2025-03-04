
using TaskManagementApi.Models;
namespace TaskManagementApi.Interfaces
{
    public interface ITaskAttachmentRepository
    {
         Task<IEnumerable<TaskAttachment>> GetAttachmentsByTaskId(int taskId);
        System.Threading.Tasks.Task AddAttachment(TaskAttachment attachment);
        System.Threading.Tasks.Task DeleteAttachment(int id);
    }
}
