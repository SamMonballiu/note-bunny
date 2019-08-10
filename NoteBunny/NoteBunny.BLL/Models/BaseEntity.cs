using System;

namespace NoteBunny.BLL.Models
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
