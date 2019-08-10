namespace NoteBunny.BLL.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public Tag(string name)
        {
            Name = name;
        }

        public Tag() { }
        public override string ToString() => Name;
    }
}
