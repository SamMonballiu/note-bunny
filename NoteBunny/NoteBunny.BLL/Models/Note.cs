using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace NoteBunny.BLL.Models
{
    public class Note : BaseEntity, IValidatableObject
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        [XmlIgnore]
        public List<Tag> Tags { get; set; }
        public List<string> TagIds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var vr = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Content) || string.IsNullOrEmpty(Subject))
            {
                vr.Add(new ValidationResult("String and content are required."));
            }

            return vr;
        }
    }
}
