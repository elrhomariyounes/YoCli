using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;
using YoCli.Commons;
using YoCli.Models;

namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command write to the main command yo
    /// </summary>
    [Command(Name = "write", Description = "Write a note")]
    class Write : ICommand
    {
        private readonly IConsole _console;
        private readonly IContext<Note> _context;

        public Write(IConsole console, IContext<Note> context)
        {
            _console = console;
            _context = context;
        }

        [Argument(0, Description = "Content of the note")]
        public string Note { get; }

        public Task<int> OnExecute()
        {
            // Init note
            Note note = new Note() { WriteDate = DateTime.Now, Content = Note };

            _context.Data.Add(note);
            _context.SaveChanges();

            _console.WriteLine($"Time : {note.WriteDate}, Content : {note.Content}");

            return Task.FromResult(1);
        }
    }
}
