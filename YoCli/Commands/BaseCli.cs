using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;

namespace YoCli.Commands
{
    public abstract class BaseCli
    {
        protected IConsole _console;
        protected INoteService _noteService;

        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {
            return Task.FromResult(0);
        }
    }

     
}
