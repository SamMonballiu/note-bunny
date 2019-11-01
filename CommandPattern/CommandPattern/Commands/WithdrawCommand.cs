using CommandPattern.Models;

namespace CommandPattern.Commands
{
    public class WithdrawCommand : AccountCommand
    {
        public WithdrawCommand(BankAccount account, int amount) : base(account, amount)
        {
        }

        public override void Execute()
        {
            if (_account.Status != AccountStatus.Open)
            {
                ErrorMessage = $"Could not withdraw {_amount.ToString("C2")} from account - Account is not Open.";
            }

            else if (_account.Balance > _amount)
            {
                _account.Balance -= _amount;
                SuccessMessage = $"Withdrew {_amount.ToString("C2")} from balance. New balance: " + _account.Balance.ToString("C2");
                Complete = true;
            }

            else
            {
                ErrorMessage = $"Could not withdraw {_amount.ToString("C2")} from account - insufficient balance.";
            }
        }
    }
}
