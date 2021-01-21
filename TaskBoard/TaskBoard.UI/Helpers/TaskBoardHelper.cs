using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskBoard.Data.Enums;
using TaskBoard.UI.Models;

namespace TaskBoard.UI.Helpers
{
    public class TaskBoardHelper
    {
        public ICollection<SelectListItem> GetTaskStatusListFromEnum()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            itemList.Add(new SelectListItem { Text = "Todo", Value = ((int)TaskStatus.Todo).ToString() });
            itemList.Add(new SelectListItem { Text = "InProgress", Value = ((int)TaskStatus.InProgress).ToString() });
            itemList.Add(new SelectListItem { Text = "Revision", Value = ((int)TaskStatus.Revision).ToString() });
            itemList.Add(new SelectListItem { Text = "Check", Value = ((int)TaskStatus.Check).ToString() });
            itemList.Add(new SelectListItem { Text = "Done", Value = ((int)TaskStatus.Done).ToString() });
            return itemList;
        }

        public string CalculateProjectDeadline(IEnumerable<Data.Models.Task> taskList)
        {
            if(taskList == null || taskList.Count() == 0)
                return "";

            var totalStoryPoint = 0;
            foreach (var task in taskList)
            {
                // Done olan işleri hesaplamada kullanmıyoruz
                if(task.TaskStatus != TaskStatus.Done)
                    totalStoryPoint += task.StoryPoint;
            }

            if(totalStoryPoint <= 0) 
                return "";

            // 3 story point 1 saat, 24 story point 1 gün olarak hesaplandığı için 24 e bölüyoruz ve kalan günü hesaplıyoruz
            var estimatedDay = totalStoryPoint / 24;

            return DateTime.Now.AddDays(estimatedDay).ToShortDateString();
        }
    }
}