using CommandPattern.Models;

namespace CommandPattern.Commands
{
    public class DepositCommand : AccountCommand
    {
        public DepositCommand(BankAccount account, int amount) : base(account, amount)
        {
        }

        public override void Execute()
        {
            if (_account.Status == AccountStatus.Open)
            {
                _account.Balance += _amount;
                SuccessMessage = $"Successfully added {_amount.ToString("C2")} to balance. New balance: {_account.Balance.ToString("C2")}";
                Complete = true;
            }

            else
            {
                ErrorMessage = $"Could not deposit {_amount.ToString("C2")}; account is not open.";
            }
        }
    }
}
