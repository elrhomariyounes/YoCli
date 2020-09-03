using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;

namespace YoCli.Commands
{

    [Command(Name = "write", Description = "Write a note")]
    public class WriteCmd : BaseCli
    {
        public WriteCmd(IConsole console, INoteService noteService)
        {
            _console = console;
            _noteService = noteService;
        }

        [Argument(0)]
        public string Note { get; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            return await _noteService.WriteNoteAsync(Note);
        }
    }
}
