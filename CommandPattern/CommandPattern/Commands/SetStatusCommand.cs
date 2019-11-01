using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandPattern.Interfaces;
using CommandPattern.Models;

namespace CommandPattern.Commands
{
    public class SetStatusCommand : ICommand
    {
        private readonly BankAccount _account;
        private readonly AccountStatus _status;
        public SetStatusCommand(BankAccount account, AccountStatus status)
        {
            _account = account;
            _status = status;
        }

        public bool Complete { get; private set; } = false;

        public string ErrorMessage { get; private set; }

        public string SuccessMessage { get; private set; }

        public void Execute()
        {
            if (_status != _account.Status)
            {
                _account.Status = _status;
                SuccessMessage = "Set account status to " + _account.Status;
                Complete = true;
            }

            else
            {
                ErrorMessage = $"Account already set to '{_status}' status.";
            }
        }
    }
}
