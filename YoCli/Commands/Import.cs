using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YoCli.Commons;
using YoCli.Models;

namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command import to the main command yo
    /// </summary>
    [Command(Name = "import", Description = "Import notes from Json file")]
    class Import : ICommand
    {
        private readonly IConsole _console;
        private readonly IContext<Note> _context;

        public Import(IConsole console, IContext<Note> context)
        {
            _console = console;
            _context = context;
        }

        [Argument(0, Description = "Path to Json file to import from")]
        public string Path { get; }

        public Task<int> OnExecute()
        {
            if (!File.Exists(Path))
            {
                ConsoleMessage.PrintError(_console, "Unable to import notes!");
                return Task.FromResult(-1);
            }

            try
            {
                // Read from file
                var jsonToImport = File.ReadAllText(Path);
                var notesToImport = JsonConvert.DeserializeObject<List<Note>>(jsonToImport);

                _context.Data.AddRange(notesToImport);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ConsoleMessage.PrintError(_console, "Unable to import notes! Please check json file format in the documentation");
                return Task.FromResult(-1);
            }

            ConsoleMessage.PrintSuccess(_console, "Notes imported");
            return Task.FromResult(1);
        }
    }
}
