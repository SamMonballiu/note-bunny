namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels
{
    public class NoteViewModel
    {
        public NoteViewModel(string subject, string id)
        {
            Subject = subject;
            Id = id;
        }

        public string Subject { get; private set; }
        public string Id { get; private set; }
    }
}
