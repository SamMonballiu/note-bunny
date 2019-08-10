using System.Collections.Generic;

namespace NoteBunny.BLL.Models
{
    public class Note : BaseEntity
    {
        public string Content { get; set; }
        public List<Tag> Tags { get; set; }
        public List<string> TagIds { get; set; }
    }
}
