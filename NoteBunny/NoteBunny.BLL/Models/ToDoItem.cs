using Newtonsoft.Json;
using System;
using System.Text;
using System.Xml.Serialization;

namespace NoteBunny.BLL.Models
{
    public class ToDoItem : BaseEntity
    {
        public string Content { get; set; }

        [XmlIgnore, JsonIgnore]
        public bool IsFinished => FinishedOn.HasValue;

        public DateTime? FinishedOn { get; set; }

        public void Finish()
        {
            FinishedOn = DateTime.Now;
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"{Content} - Finished: ");

            sb.Append(IsFinished
                ? FinishedOn.Value.ToShortDateString()
                : "no") ;

            return sb.ToString();
        }
    }
}
