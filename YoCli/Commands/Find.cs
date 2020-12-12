using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoCli.Commons;
using YoCli.Models;

namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command find to the main command yo
    /// </summary>
    [Command(Name = "find", Description = "Find notes")]
    class Find : ICommand
    {
        private readonly IConsole _console;
        private readonly IContext<Note> _context;

        public Find(IConsole console, IContext<Note> context)
        {
            _console = console;
            _context = context;
        }

        [Option(CommandOptionType.SingleOrNoValue, Description = "Find notes that contains this value")]
        public (bool HasValue, string Value) Content { get; set; }

        [Option(CommandOptionType.SingleOrNoValue, Description = "Find notes written this day on the current month")]
        public (bool HasValue, int Value) Day { get; set; }

        [Option(CommandOptionType.SingleOrNoValue, Description = "Find notes written this month on the current year")]
        public (bool HasValue, int Value) Month { get; set; }

        public Task<int> OnExecute()
        {
            var notes = new List<Note>();

            // Check if there is no options
            if (!Content.HasValue && !Day.HasValue && !Month.HasValue)
            {
                ConsoleMessage.PrintError(_console, "Invalid command please choose an option. Run 'yo find --help' for more details");
                return Task.FromResult(-1);
            }

            // Filter by content
            if (Content.HasValue)
            {
                notes.AddRange(_context.Data.Where(n => n.Content.ToLower().Contains(Content.Value.ToLower())));
            }

            // Filter by month
            if (Month.HasValue)
            {
                notes.AddRange(_context.Data.Where(n => n.WriteDate.Date.Month == Month.Value && n.WriteDate.Date.Year == DateTime.Today.Year));
            }

            // Filter by day
            if (Day.HasValue)
            {
                notes.AddRange(_context.Data.Where(n => n.WriteDate.Date.Day == Day.Value &&
                                                         n.WriteDate.Date.Month == DateTime.Today.Month &&
                                                         n.WriteDate.Date.Year == DateTime.Today.Year));
            }

            notes.ToList().ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));

            return Task.FromResult(1);
        }
    }
}
