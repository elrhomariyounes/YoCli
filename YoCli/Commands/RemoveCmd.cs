using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;
namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command write to the main command yo
    /// </summary>
    [Command(Name = "remove", Description = "Remove a note by a specified date")]
    public class RemoveCmd : BaseCli
    {
        public RemoveCmd(IConsole console, INoteService noteService)
        {
            _console = console;
            _noteService = noteService;
        }

        [Argument(0, Description = "Date criteria in local date format")]
        public string Date { get; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            return await _noteService.RemoveNoteByDateAsync(Date);
        }
    }
}
