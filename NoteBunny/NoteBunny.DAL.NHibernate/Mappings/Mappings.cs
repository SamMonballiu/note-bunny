using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace NoteBunny.DAL.NHibernate.Mappings
{
    class NoteMapping : ClassMap<Note>
    {
        public NoteMapping()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.Subject).Not.Nullable();
            Map(x => x.Content).Nullable();
            Map(x => x.CreatedOn).Not.Nullable();
            HasManyToMany(x => x.Tags)
               .Table("NoteTags")
               .ParentKeyColumn("NoteId")
               .ChildKeyColumn("TagId")
               .LazyLoad();
            Table("Notes");
        }
    }

    class TagMapping : ClassMap<Tag>
    {
        public TagMapping()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.Name);
            Map(x => x.CreatedOn);
            HasManyToMany(x => x.Notes)
                .Table("NoteTags")
                .ParentKeyColumn("TagId")
                .ChildKeyColumn("NoteId");
            Table("Tags");
        }
    }
}
