using System;

namespace NoteBunny.BLL.Models
{
    public class ToDoItem : BaseEntity
    {
        public string Content { get; set; }
        public bool IsFinished { get; set; }
        public DateTime FinishedOn { get; set; }
    }
}
