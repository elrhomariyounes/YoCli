using McMaster.Extensions.CommandLineUtils;
using System.Reflection;
using System.Threading.Tasks;
using YoCli.Commons;

namespace YoCli.Commands
{
    [Command(Name = "yo", OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(Write),
        typeof(Read),
        typeof(Find),
        typeof(Export),
        typeof(Import))
    ]
    class Yo : ICommand
    {
        private readonly CommandLineApplication _app;

        public Yo(CommandLineApplication app)
        {
            _app = app;
        }

        public Task<int> OnExecute()
        {
            _app.ShowHelp();

            return Task.FromResult(0);
        }

        private static string GetVersion => typeof(Yo).Assembly
                                                      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                                      .InformationalVersion;
    }
}
