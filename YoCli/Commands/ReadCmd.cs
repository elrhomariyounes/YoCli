using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;

namespace YoCli.Commands
{
    [Command(Name = "read", Description = "Read notes")]
    public class ReadCmd : BaseCli
    {

        public ReadCmd(INoteService noteService)
        {
            _noteService = noteService;
        }

        [Option(CommandOptionType.NoValue, Description = "Read today notes")]
        public bool Today { get; set; } = false;

        [Option(CommandOptionType.NoValue, Description = "Read yesterday notes")]
        public bool Yesterday { get; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var options = new Dictionary<string, bool>();
            options.Add("Today", this.Today);
            options.Add("Yesterday", this.Yesterday);
            return await _noteService.ReadNotesAsync(options);
        }
    }
}
