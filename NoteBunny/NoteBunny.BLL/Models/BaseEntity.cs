using System;

namespace NoteBunny.BLL.Models
{
    public abstract class BaseEntity
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
