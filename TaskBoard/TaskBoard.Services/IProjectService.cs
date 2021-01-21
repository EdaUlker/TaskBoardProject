using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBoard.Data;
using TaskBoard.Data.Models;

namespace TaskBoard.Services
{
    public interface IProjectService
    {
        Task<Project> GetByIdAsync(int id);

        Task<Project> GetByProjectNameAsync(string projectName);

        IEnumerable<Project> GetAll();

        System.Threading.Tasks.Task Add(Project project);

        System.Threading.Tasks.Task Update(Project project);

        System.Threading.Tasks.Task Delete(Project project);
    }
}