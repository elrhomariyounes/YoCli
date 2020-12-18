using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using YoCli.Commands;
using YoCli.Commons;
using YoCli.Data;
using YoCli.Models;

namespace YoCli
{
    class Program
    {

        static async Task<int> Main(string[] args)
        {
            var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IContext<Note>, NoteContext>();
            });

            try
            {
                return await builder.RunCommandLineApplicationAsync<Yo>(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();

                return 1;
            }
        }
    }
}
