using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NoteBunny.BLL.Models
{
    public class Tag : BaseEntity
    {
        public virtual string Name { get; set; }

        public Tag(string name)
        {
            Name = name;
        }

        public Tag() { }
        [XmlIgnore,JsonIgnore]
        public virtual IList<Note> Notes { get; set; }
        public override string ToString() => Name;
    }
}
