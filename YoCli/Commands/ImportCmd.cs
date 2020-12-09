using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;

namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command import to the main command yo
    /// </summary>
    [Command(Name = "import", Description = "Import notes from Json file")]
    public class ImportCmd : BaseCli
    {
        public ImportCmd(IConsole console, INoteService noteService)
        {
            _console = console;
            _noteService = noteService;
        }

        [Argument(0, Description = "Path to Json file to import from")]
        public string Path { get; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            return await _noteService.ImportNotesFromJsonFileAsync(Path);
        }
    }
}
