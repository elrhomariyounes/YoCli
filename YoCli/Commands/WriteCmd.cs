using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YoCli.Commands
{

    [Command(Name = "write", Description = "Write a note")]
    public class WriteCmd : BaseCli
    {
        public WriteCmd(IConsole console)
        {
            _console = console;
        }

        [Argument(0)]
        public string Note { get; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            //TODO : Move to NoteService
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notes.txt");
            string note = DateTime.Now.ToString() + ":" + Note;
            _console.WriteLine(note);
            await File.AppendAllLinesAsync(path, new string[] { note });
            return 1;
        }
    }
}
