using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBoard.Data;
using TaskBoard.Data.Models;

namespace TaskBoard.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProjectService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> GetByProjectNameAsync(string projectName)
        {
            return await _applicationDbContext.Projects.FirstOrDefaultAsync(p => p.ProjectName == projectName);
        }

        public IEnumerable<Project> GetAll()
        {
            return _applicationDbContext.Projects.Where(p => p.IsActive);
        }

        public async System.Threading.Tasks.Task Add(Project project)
        {
            project.IsActive = true;
            await _applicationDbContext.AddAsync(project);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Update(Project project)
        {
            _applicationDbContext.Update(project);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(Project project)
        {
            project.IsActive = false;
            await Update(project);
        }
    }
}