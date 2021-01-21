using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TaskBoard.Data.Models;

namespace TaskBoard.UI.Models.TaskViewModels
{
    public class TaskDetailViewModel
    {
        public int TaskId { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string TaskName { get; set; }

        [Required]
        public string Description { get; set; }

        public string Note { get; set; }

        public DateTime? TaskDate { get; set; }

        public DateTime? PreDate { get; set; }

        public DateTime? RealDate { get; set; }

        public int StoryPoint { get; set; }

        public TaskBoard.Data.Enums.TaskStatus TaskStatus { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public virtual IEnumerable<TaskState> TaskStates { get; set; }

        public string StatusMessage { get; set; }

        public virtual IEnumerable<SelectListItem> AvailableUsers { get; set; }

        public virtual IEnumerable<SelectListItem> AvailableStatus { get; set; }
    }
}