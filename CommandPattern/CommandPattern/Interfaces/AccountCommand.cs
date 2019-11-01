using CommandPattern.Interfaces;
using CommandPattern.Models;

namespace CommandPattern.Commands
{
    public abstract class AccountCommand : ICommand
    {
        protected readonly BankAccount _account;
        protected readonly int _amount;

        protected AccountCommand(BankAccount account, int amount)
        {
            _account = account;
            _amount = amount;
        }

        public bool Complete { get; protected set; } = false;

        public string ErrorMessage { get; protected set; }
        public string SuccessMessage { get; protected set; }

        public abstract void Execute();
    }
}
