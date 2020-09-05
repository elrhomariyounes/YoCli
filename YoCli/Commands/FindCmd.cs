using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoCli.Services.Interfaces;

namespace YoCli.Commands
{
    /// <summary>
    /// Represent the sub command find to the main command yo
    /// </summary>
    [Command(Name = "find", Description = "Find notes")]
    public class FindCmd : BaseCli
    {
        public FindCmd(INoteService noteService)
        {
            _noteService = noteService;
        }

        [Option(CommandOptionType.SingleOrNoValue, Description = "Find notes that contains this value")]
        public (bool hasValue, string value) Content { get; set; }

        [Option(CommandOptionType.SingleOrNoValue, Description = "Find notes written this day on the current month")]
        public (bool hasValue, int value) Day { get; set; }

        [Option(CommandOptionType.SingleOrNoValue, Description = "Find notes written this month on the current year")]
        public (bool hasValue, int value) Month { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var options = new Dictionary<string, int>();
            options.Add("Day", this.Day.value);
            options.Add("Month", this.Month.value);
            var content = "";
            if (this.Content.hasValue)
                content = this.Content.value;
            return await _noteService.FindNotesAsync(content, options);
        }
    }   
}
