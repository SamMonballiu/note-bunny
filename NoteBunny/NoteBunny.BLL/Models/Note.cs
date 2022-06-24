using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;

namespace NoteBunny.BLL.Models
{
    public class Note : BaseEntity, IValidatableObject
    {
        public virtual string Subject { get; set; }
        public virtual string Content { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public virtual IList<Tag> Tags { get; set; }
        public virtual IList<string> TagIds { get; set; }
        public bool? IsPinned { get; set; } = null;

        public override string ToString() => Subject.First().ToString().ToUpper() + Subject.Substring(1);

        [XmlIgnore, JsonIgnore]
        public string Details { get => CreatedOn.ToShortDateString() + " • " + String.Join(", ", Tags?.Select(t => t.Name)); }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
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
