using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace YoCli.Commands
{
    public abstract class BaseCli
    {
        protected IConsole _console;

        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {
            return Task.FromResult(0);
        }
    }

     
}
