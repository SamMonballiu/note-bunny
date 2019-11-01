using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern.Models
{
    public enum AccountStatus : byte
    {
        Created,
        Open,
        Closed
    }
    public class BankAccount
    {
        public BankAccount(string ownerName, double balance = 0)
        {
            OwnerName = ownerName;
            Balance = balance;
        }

        public string OwnerName { get; set; }
        public double Balance { get; set; }
        public AccountStatus Status = AccountStatus.Created;
        public override string ToString() => $"{OwnerName} - {Balance.ToString("C2")} - {Status}";
        
    }
}
