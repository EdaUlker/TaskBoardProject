using System.ComponentModel.DataAnnotations;

namespace TaskBoard.UI.Models.ProjectViewModels
{
    public class ProjectDetailViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string StatusMessage { get; set; }
    }
}