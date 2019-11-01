using CommandPattern.Commands;
using CommandPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var account = new BankAccount("Sam");
            Console.WriteLine(account);
            var commandQueue = new CommandQueue();

            commandQueue.Add(new DepositCommand(account, 1200));
            commandQueue.Add(new WithdrawCommand(account, 570));

            foreach (var commandReport in commandQueue.ExecutePendingCommandsAndReport())
            {
                Console.WriteLine(commandReport);
            }

            Console.WriteLine(commandQueue.HasPendingCommands ? $"{commandQueue.PendingCommandCount} commands could not be run." : "All commands were run.");

            Console.WriteLine("****");
            commandQueue.InsertAt(0, new SetStatusCommand(account, AccountStatus.Open));
            foreach (var commandReport in commandQueue.ExecutePendingCommandsAndReport())
            {
                Console.WriteLine(commandReport);
            }
            Console.WriteLine(commandQueue.HasPendingCommands ? $"{commandQueue.PendingCommandCount} commands could not be run." : "All commands were run.");

        }
    }
}
