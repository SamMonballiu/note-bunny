using Newtonsoft.Json;
using NoteBunny.BLL.Extensions;
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
        [XmlIgnore, JsonIgnore]
        public virtual IList<Tag> Tags { get; set; }
        public virtual IList<string> TagIds { get; set; }
        public bool? IsPinned { get; set; } = null;
        [XmlIgnore, JsonIgnore]
        public bool IsCode => Tags?.Select(x => x.Name.ToLower()).Any(x => x.Equals("code")) ?? false;

        public override string ToString() => Subject.Capitalize();

        [XmlIgnore, JsonIgnore]
        public string CreationDate => CreatedOn.ToShortDateString();

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
