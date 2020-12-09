using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;
namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command export to the main command yo
    /// </summary>
    [Command(Name = "export", Description = "Export notes to Json file")]
    public class ExportCmd : BaseCli
    {
        public ExportCmd(IConsole console, INoteService noteService)
        {
            _console = console;
            _noteService = noteService;
        }

        [Argument(0, Description = "The directory where to save the file")]
        public string Path { get; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            return await _noteService.ExportNotesToJsonFileAsync(Path);
        }
    }
}
