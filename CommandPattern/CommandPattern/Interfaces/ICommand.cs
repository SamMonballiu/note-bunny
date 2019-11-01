namespace CommandPattern.Interfaces
{
    public interface ICommand
    {
        bool Complete { get;}
        string ErrorMessage { get; }
        string SuccessMessage { get; }
        void Execute();
    }
}
