using System.Collections.Generic;
using TaskBoard.Data.Models;

namespace TaskBoard.Services
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<Task> GetByIdAsync(int id);

        IEnumerable<Task> GetAll();

        System.Threading.Tasks.Task Add(Task task);

        System.Threading.Tasks.Task Update(Task task);

        System.Threading.Tasks.Task Delete(Task task);

        IEnumerable<Data.Models.Task> GetProjectTasksByProjectId(int projectId);

        Data.Models.Task GetTaskById(int id);
    }
}