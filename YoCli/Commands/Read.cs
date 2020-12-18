using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YoCli.Commons;
using YoCli.Models;

namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command read to the main command yo
    /// </summary>
    [Command(Name = "read", Description = "Read notes")]
    class Read : ICommand
    {
        private readonly IConsole _console;
        private readonly IContext<Note> _context;

        public Read(IConsole console, IContext<Note> context)
        {
            _console = console;
            _context = context;
        }

        [Option(CommandOptionType.NoValue, Description = "Read today notes")]
        public bool Today { get; set; }

        [Option(CommandOptionType.NoValue, Description = "Read yesterday notes")]
        public bool Yesterday { get; set; }

        [Option(CommandOptionType.NoValue, Description = "Read current week notes")]
        public bool Week { get; set; }

        public Task<int> OnExecute()
        {
            // Check if more than one option is set
            if (new List<bool>() { Today, Yesterday, Week }.Count(o => o == true) > 1)
            {
                ConsoleMessage.PrintError(_console, "Only one option should be set!"); ;
                return Task.FromResult(-1);
            }

            // Filter notes
            if (Yesterday)
            {
                _context.Data.Where(n => n.WriteDate.Date == DateTime.Today.AddDays(-1))
                             .ToList()
                             .ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));
            }
            else if (Week)
            {
                var c = CultureInfo.CurrentCulture.Calendar;
                var rule = CalendarWeekRule.FirstDay;
                var firstDayOfWeek = DayOfWeek.Monday;

                _context.Data.Where(n => c.GetWeekOfYear(n.WriteDate, rule, firstDayOfWeek) == c.GetWeekOfYear(DateTime.Today, rule, firstDayOfWeek))
                             .ToList()
                             .ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));
            }
            else // Today
            {
                _context.Data.Where(n => n.WriteDate.Date == DateTime.Today)
                             .ToList()
                             .ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));
            }

            return Task.FromResult(1);
        }
    }
}
