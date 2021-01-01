using McMaster.Extensions.CommandLineUtils;
using System.Reflection;
using System.Threading.Tasks;

namespace YoCli.Commands
{
    [Command(Name = "yo", OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version",MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(WriteCmd),
        typeof(ReadCmd),
        typeof(FindCmd),
        typeof(ExportCmd),
        typeof(ImportCmd),
        typeof(RemoveCmd))
    ]
    public class YoCmd : BaseCli
    {
        public YoCmd(IConsole console)
        {
            _console = console;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }

        private static string GetVersion()
            => typeof(YoCmd).Assembly
                            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                            .InformationalVersion;
    }
}
