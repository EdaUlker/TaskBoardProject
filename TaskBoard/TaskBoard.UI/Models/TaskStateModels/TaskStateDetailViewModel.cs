using System;
using System.ComponentModel.DataAnnotations;
using TaskBoard.Data.Enums;

namespace TaskBoard.UI.Models.TaskStateModels
{
    public class TaskStateDetailViewModel
    {
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int TaskStateId { get; set; }

        public TaskStatus TaskStatus { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime? TaskStateDate { get; set; }

        public string UserName { get; set; }

        public string StatusMessage { get; set; }
    }
}