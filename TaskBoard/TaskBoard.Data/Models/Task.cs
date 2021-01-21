using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TaskBoard.Data.Enums;

namespace TaskBoard.Data.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public string Note { get; set; }

        public DateTime? TaskDate { get; set; }

        public DateTime? PreDate { get; set; }

        public DateTime? RealDate { get; set; }

        public int StoryPoint { get; set; }

        public TaskStatus TaskStatus { get; set; }

        public bool IsActive { get; set;}

        public virtual IdentityUser User { get; set; }

        public virtual Project Project { get; set; }

        public int ProjectId { get; set; }

        public virtual IEnumerable<TaskState> TaskStates { get; set; }
    }
}
