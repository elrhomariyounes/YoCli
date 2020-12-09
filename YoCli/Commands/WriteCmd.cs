using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;

namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command write to the main command yo
    /// </summary>
    [Command(Name = "write", Description = "Write a note")]
    public class WriteCmd : BaseCli
    {
        public WriteCmd(IConsole console, INoteService noteService)
        {
            _console = console;
            _noteService = noteService;
        }

        [Argument(0, Description = "Content of the note")]
        public string Note { get; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            return await _noteService.WriteNoteAsync(Note);
        }
    }
}
