
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TaskBoard.Data.Enums;

namespace TaskBoard.Data.Models
{
    public class TaskState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TaskStatus TaskStatus { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime? TaskStateDate { get; set; }

        public IdentityUser User { get; set; }

        public virtual Task Task { get; set; }

        public int TaskId { get; set; }
    }
}
