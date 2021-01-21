using System.Collections.Generic;
using TaskBoard.Data.Models;

namespace TaskBoard.Services
{
    public interface ITaskStateService
    {
        System.Threading.Tasks.Task<TaskState> GetByIdAsync(int id);

        System.Threading.Tasks.Task Add(TaskState taskState);

        System.Threading.Tasks.Task Update(TaskState taskState);

        System.Threading.Tasks.Task Delete(TaskState taskState);

        IEnumerable<Data.Models.TaskState> GetTaskStatesByTaskId(int taskId);
    }
}
