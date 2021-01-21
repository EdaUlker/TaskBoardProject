using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBoard.Data;
using TaskBoard.Data.Models;

namespace TaskBoard.Services
{
    public class TaskStateService : ITaskStateService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TaskStateService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<TaskState> GetByIdAsync(int id)
        {
            return await _applicationDbContext.TaskStates.FindAsync(id);
        }

        public async System.Threading.Tasks.Task Add(TaskState taskState)
        {
            await _applicationDbContext.AddAsync(taskState);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Update(TaskState taskState)
        {
            _applicationDbContext.Update(taskState);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(TaskState taskState)
        {
            _applicationDbContext.Remove(taskState);
            await _applicationDbContext.SaveChangesAsync();
        }

        public IEnumerable<Data.Models.TaskState> GetTaskStatesByTaskId(int taskId)
        {
            return _applicationDbContext.TaskStates
                .Include(x => x.Task)
                .Where(x => x.Task.Id == taskId);
        }
    }
}