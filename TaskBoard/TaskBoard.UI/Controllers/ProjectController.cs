using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskBoard.Data.Models;
using TaskBoard.Services;
using TaskBoard.UI.Models.ProjectViewModels;

namespace TaskBoard.UI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProjectService _projectService;

        [TempData]
        public string StatusMessage { get; set; }

        public ProjectController(IProjectService projectService, UserManager<IdentityUser> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var projects = _projectService.GetAll().Select(x => new ProjectDetailViewModel
            {
                Id = x.Id,
                ProjectName = x.ProjectName
            });

            var model = new ProjectIndexViewModel
            {
                Projects = projects
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int id)
        {
            if(id <= 0) 
                return View();

            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                throw new ApplicationException($"'{id}' ID sine sahip proje bulunamadı");
            }

            return View(new ProjectDetailViewModel
            {
                Id = id,
                ProjectName = project.ProjectName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(ProjectDetailViewModel model)
        {
            if(model == null)
            {
                return View(new ProjectDetailViewModel());
            }

            if (string.IsNullOrEmpty(model.ProjectName))
            {
                model.StatusMessage = "Proje Adı alanı zorunludur";
                return View(model);
            }

            if(model.Id > 0)
            {
                var project = await _projectService.GetByIdAsync(model.Id);
                project.ProjectName = model.ProjectName;
                await _projectService.Update(project);
            }
            else
            {
                await _projectService.Add(new Project
                {
                    ProjectName = model.ProjectName
                });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProjectDetailViewModel model)
        {
            if(model.Id > 0)
            {
                var project = await _projectService.GetByIdAsync(model.Id);
                if(project == null)
                {
                    model.StatusMessage = $"Silinmek istenen '{model.Id}' ye sahip proje bulunamadı";
                    return View(model);
                }
                await _projectService.Delete(project);
            }
            else
            {
                model.StatusMessage = $"Silinmek istenen '{model.Id}' ye sahip proje bulunamadı";
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}