using CommandPattern.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CommandPattern.Models
{
    public class CommandQueue
    {
        List<ICommand> _queue = new List<ICommand>();

        public bool HasPendingCommands { get => _queue.Any(x => !x.Complete); }
        public int PendingCommandCount { get => _queue.Count(x => !x.Complete); }
        public IEnumerable<string> ExecutePendingCommandsAndReport()
        {
            foreach (var cmd in _queue)
            {
                cmd.Execute();
                yield return cmd.Complete ? cmd.SuccessMessage : cmd.ErrorMessage;
            }
        }

        public IEnumerable<string> GetErrorMessagesForFailedCommands()
        {
            return _queue.Where(x => !x.Complete && !string.IsNullOrEmpty(x.ErrorMessage)).Select(p => p.ErrorMessage);
        }

        public void Add(ICommand cmd) => _queue.Add(cmd);

        public void InsertAt(int index, ICommand cmd) => _queue.Insert(index, cmd);
    }
}
