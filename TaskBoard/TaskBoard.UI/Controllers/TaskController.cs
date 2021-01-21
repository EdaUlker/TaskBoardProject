using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBoard.Data.Models;
using TaskBoard.Services;
using TaskBoard.UI.Helpers;
using TaskBoard.UI.Models.ProjectViewModels;
using TaskBoard.UI.Models.TaskStateModels;
using TaskBoard.UI.Models.TaskViewModels;

namespace TaskBoard.UI.Controllers
{
    public class TaskController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly ITaskStateService _taskStateService;
        private readonly TaskBoardHelper _taskBoardHelper;

        public TaskController(
            UserManager<IdentityUser> userManager,
            IProjectService projectService, 
            ITaskService taskService,
            ITaskStateService taskStateService,
            TaskBoardHelper taskBoardHelper)
        {
            _projectService = projectService;
            _userManager = userManager;
            _taskService = taskService;
            _taskStateService = taskStateService;
            _taskBoardHelper = taskBoardHelper;
        }

        public async Task<IActionResult> Index(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if(project == null)
            {
                return NotFound();
            }

            var taskList = _taskService.GetProjectTasksByProjectId(id).ToList();
            if (taskList == null)
            {
                return NotFound();
            }

            var model = new BoardViewModel
            {
                ProjectId = id,
                ProjectName = project.ProjectName,
                Tasks = taskList
            };

            model.EstimatedProjectDeadline = _taskBoardHelper.CalculateProjectDeadline(model.Tasks);
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int id, int taskid)
        {
            var statusList = _taskBoardHelper.GetTaskStatusListFromEnum();
            var userList = _userManager.Users.Select(u => u).ToList();

            if (userList == null || userList.Count == 0)
                return View(new TaskDetailViewModel { StatusMessage = "Kullanıcı bulunamadı" });
            var users = userList.Select(user => new SelectListItem { Value = user.Id.ToString(), Text =  user.Email }).ToList();

            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return View(new TaskDetailViewModel { StatusMessage = $"'{id}' ID sine sahip proje bulunamadı", AvailableUsers = users, AvailableStatus = statusList});

            if (taskid <= 0)
                return View(new TaskDetailViewModel 
                { 
                    TaskId = 0,
                    ProjectId = id,
                    ProjectName = project.ProjectName,
                    AvailableUsers = users,
                    AvailableStatus = statusList
                });

            var task = _taskService.GetTaskById(taskid);
            if (task == null)
            {
                throw new ApplicationException($"'{taskid}' ID sine sahip görev bulunamadı");
            }

            return View(new TaskDetailViewModel
            {
                TaskId = task.Id,
                ProjectId = task.Project.Id,
                ProjectName = task.Project.ProjectName,
                Description = task.Description,
                Note = task.Note,
                TaskDate = task.TaskDate,
                PreDate = task.PreDate,
                RealDate = task.RealDate,
                StoryPoint = task.StoryPoint,
                TaskStatus = task.TaskStatus,
                UserId = task.User.Id,
                TaskStates = task.TaskStates,
                AvailableUsers = users,
                AvailableStatus = statusList
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(TaskDetailViewModel model)
        {
            var statusList = _taskBoardHelper.GetTaskStatusListFromEnum();
            var userList = _userManager.Users.Select(u => u).ToList();
            if (userList == null || userList.Count == 0)
                return View(new TaskDetailViewModel { StatusMessage = "Kullanıcı bulunamadı" });
            var users = userList.Select(user => new SelectListItem { Value = user.Id.ToString(), Text =  user.Email }).ToList();

            var taskDetailViewModel = new TaskDetailViewModel
            {
                TaskId = model.TaskId,
                ProjectId = model.ProjectId,
                ProjectName = model.ProjectName,
                AvailableUsers = users,
                AvailableStatus = statusList
            };

            if (model.ProjectId < 1)
            {
                taskDetailViewModel.StatusMessage = "Proje alanı zorunlu olmalıdır";
                return View(taskDetailViewModel);
            }

            var project = await _projectService.GetByIdAsync(model.ProjectId);
            if(project == null)
            {
                taskDetailViewModel.StatusMessage = $"'{model.ProjectId}' ID sine sahip proje bulunamadı";
                return View(taskDetailViewModel);
            }

            if (string.IsNullOrEmpty(model.UserId))
            {
                taskDetailViewModel.StatusMessage = "Kullanıcı alanı zorunlu olmalıdır";
                return View(taskDetailViewModel);
            }
            
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if(user == null)
            {
                taskDetailViewModel.StatusMessage = $"'{model.UserId}' ID sine sahip kullanıcı bulunamadı";
                return View(taskDetailViewModel);
            }

            if (model.TaskId > 0)
            {
                var taskStates = _taskStateService.GetTaskStatesByTaskId(model.TaskId);
                var task = await _taskService.GetByIdAsync(model.TaskId);
                task.Description = model.Description;
                task.Note = model.Note;
                task.TaskDate = model.TaskDate;
                task.PreDate = model.PreDate;
                task.RealDate = model.RealDate;
                task.StoryPoint = model.StoryPoint;
                task.TaskStatus = model.TaskStatus;
                task.Project = project;
                task.User = user;

                await _taskService.Update(task);
            }
            else
            {
                var projectTask = new Data.Models.Task
                {
                    Id = 0,
                    Description = model.Description,
                    Note = model.Note,
                    TaskDate = model.TaskDate == null ? DateTime.Now : model.TaskDate,
                    PreDate = model.PreDate,
                    RealDate = model.RealDate,
                    StoryPoint = model.StoryPoint,
                    TaskStatus = model.TaskStatus
                };


                projectTask.Project = project;
                projectTask.User = user;
                await _taskService.Add(projectTask);
            }

            return RedirectToAction(nameof(Index), new{project.Id});
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int taskid)
        {
            if(taskid <= 0)
                return RedirectToAction(nameof(Index));

            var task = await _taskService.GetByIdAsync(taskid);
            if(task == null)
                return RedirectToAction(nameof(Index));

            await _taskService.Delete(task);

            return RedirectToAction(nameof(Index), new{id});
        }

        [HttpGet]
        public async Task<IActionResult> ChangeTaskStatus(int id, int taskid, int taskstatus)
        {
            if(taskid <= 0)
                return RedirectToAction(nameof(Index));

            var task = await _taskService.GetByIdAsync(taskid);
            if(task == null)
                return RedirectToAction(nameof(Index));

            task.TaskStatus = (Data.Enums.TaskStatus)taskstatus;
            await _taskService.Update(task);

            return RedirectToAction(nameof(Index), new{id});
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEditTaskState(int id, int taskid, int taskstateid)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return View(new TaskStateDetailViewModel { StatusMessage = $"'{id}' ID sine sahip proje bulunamadı"});

            var task = await _taskService.GetByIdAsync(taskid);
            if (task == null)
                return View(new TaskStateDetailViewModel { StatusMessage = $"'{task}' ID sine sahip proje bulunamadı"});

            if (taskstateid <= 0)
                return View(new TaskStateDetailViewModel 
                { 
                    ProjectId = id,
                    TaskId = taskid,
                    TaskStateId = -1,
                    UserName = User.Identity.Name
                });

            var taskState = await _taskStateService.GetByIdAsync(taskstateid);
            if (taskState == null)
            {
                throw new ApplicationException($"'{taskstateid}' ID sine sahip görev bulunamadı");
            }

            return View(new TaskStateDetailViewModel
            {
                ProjectId = project.Id,
                TaskId = task.Id,
                TaskStateId = taskState.Id,
                Description = taskState.Description,
                TaskStateDate = taskState.TaskStateDate,
                TaskStatus = taskState.TaskStatus,
                UserName = User.Identity.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEditTaskState(TaskStateDetailViewModel model)
        {
            var taskDetailViewModel = new TaskStateDetailViewModel 
            { 
                ProjectId = model.ProjectId,
                TaskId = model.TaskId,
                TaskStateId = model.TaskStateId,
                UserName = User.Identity.Name
            };

            if (model.ProjectId < 1)
            {
                taskDetailViewModel.StatusMessage = "Proje alanı zorunlu olmalıdır";
                return View(taskDetailViewModel);
            }

            var project = await _projectService.GetByIdAsync(model.ProjectId);
            if(project == null)
            {
                taskDetailViewModel.StatusMessage = $"'{model.ProjectId}' ID sine sahip proje bulunamadı";
                return View(taskDetailViewModel);
            }

            if (model.TaskStateId > 0)
            {
                var taskState = await _taskStateService.GetByIdAsync(model.TaskStateId);
                taskState.Description = model.Description;
                taskState.TaskStatus = model.TaskStatus;
                await _taskStateService.Update(taskState);
            }
            else
            {
                var task = await _taskService.GetByIdAsync(model.TaskId);
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var taskState = new Data.Models.TaskState
                {
                    Id = 0,
                    Description = model.Description,
                    TaskStatus = model.TaskStatus,
                    TaskStateDate = DateTime.Now
                };

                taskState.Task = task;
                taskState.User = user;
                await _taskStateService.Add(taskState);
            }

            return RedirectToAction(nameof(CreateOrEdit), new{id = project.Id, taskid = model.TaskId});
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTaskState(int id, int taskid, int taskstateid)
        {
            if(taskstateid <= 0)
                return RedirectToAction(nameof(CreateOrEdit), new{id = id, taskid = taskid});

            var taskState = await _taskStateService.GetByIdAsync(taskstateid);
            if(taskState == null)
                return RedirectToAction(nameof(CreateOrEdit), new{id = id, taskid = taskid});

            await _taskStateService.Delete(taskState);

            return RedirectToAction(nameof(CreateOrEdit), new{id = id, taskid = taskid});
        }
    }
}
