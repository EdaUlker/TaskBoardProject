using System.Collections.Generic;

namespace TaskBoard.UI.Models.TaskViewModels
{
    public class BoardViewModel
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string EstimatedProjectDeadline { get; set; }

        public IEnumerable<Data.Models.Task> Tasks { get; set; }
    }
}