using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using YoCli.Commands;
using YoCli.Services.Implementations;
using YoCli.Services.Interfaces;

namespace YoCli
{
    public class Program
    {
        private readonly IConsole _console;

        public Program(IConsole console)
        {
            _console = console;
        }

        public static async Task<int> Main(string[] args)
        {
            var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<INoteService, NoteService>();
            });

            try
            {
                return await builder.RunCommandLineApplicationAsync<YoCmd>(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        [Option(Description = "The Subject")]
        public string Subject { get; }

        private void OnExecute()
        {
            var name = Prompt.GetString("Your name ?");
            var subject = Subject ?? "world";
            _console.WriteLine("Hello " + name);
        }
    }
}
