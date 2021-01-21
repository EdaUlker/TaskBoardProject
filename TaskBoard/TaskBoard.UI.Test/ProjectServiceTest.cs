using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskBoard.Data;
using TaskBoard.Data.Models;
using Xunit;

namespace TaskBoard.Services.Test
{
    public class ProjectServiceTest
    {
        public IProjectService _projectService;

        public ProjectServiceTest()
        {
            var services = new ServiceCollection();
            var serviceProvider =
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(
                        "Data Source=localhost;Initial Catalog=TaskBoardDB;Integrated Security=True;MultipleActiveResultSets=true;",
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure();
                        });
                }).BuildServiceProvider();

            var applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            _projectService = new ProjectService(applicationDbContext);
        }

        [Fact]
        public void GetAllProjects_Test()
        {
            var result = _projectService.GetAll();
            Assert.NotNull(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task Project_Test()
        {
            // create project
            var project = new Project { ProjectName = "TestProjeOrnegi" };
            await _projectService.Add(project);

            // update project
            project = await _projectService.GetByProjectNameAsync(project.ProjectName);
            Assert.NotNull(project);
            project.ProjectName = "TestProjeOrnegi2";
            await _projectService.Update(project);

            // delete project
            project = await _projectService.GetByProjectNameAsync(project.ProjectName);
            Assert.NotNull(project);
            Assert.Equal("TestProjeOrnegi2", project.ProjectName);
            await _projectService.Delete(project);

            project = await _projectService.GetByProjectNameAsync(project.ProjectName);
            Assert.NotNull(project);
            Assert.False(project.IsActive);
        }
    }
}
