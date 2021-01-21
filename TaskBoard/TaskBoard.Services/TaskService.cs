using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBoard.Data;
using TaskBoard.Data.Models;

namespace TaskBoard.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TaskService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Data.Models.Task> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Tasks.FindAsync(id);
        }

        public IEnumerable<Data.Models.Task> GetAll()
        {
            return _applicationDbContext.Tasks;
        }

        public async System.Threading.Tasks.Task Add(Data.Models.Task task)
        {
            task.IsActive = true;
            await _applicationDbContext.AddAsync(task);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Update(Data.Models.Task task)
        {
            _applicationDbContext.Update(task);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(Data.Models.Task task)
        {
            task.IsActive = false;
            await Update(task);
        }

        public IEnumerable<Data.Models.Task> GetProjectTasksByProjectId(int projectId)
        {
            return _applicationDbContext.Tasks
                .Include(x => x.User)
                .Where(x => x.ProjectId == projectId);
        }

        public Data.Models.Task GetTaskById(int id)
        {
            return _applicationDbContext.Tasks
                .Include(x => x.User)
                .Include(x => x.Project)
                .Include(x => x.TaskStates)
                .SingleOrDefault(x => x.Id == id);
        }
    }
}
