using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoCli.Models;
using YoCli.Services.Interfaces;
using YoCli.Utils;

namespace YoCli.Services.Implementations
{
    /// <inheritdoc cref="INoteService"/>
    public class NoteService : INoteService
    {
        private readonly IConsole _console;
        private readonly List<Note> _notes;
        public NoteService(IConsole console)
        {
            _console = console;
            _notes = new List<Note>(this.GetNotes());
        }

        public Task<int> ReadNotesAsync(Dictionary<string, bool> options)
        {
            // Get options values
            bool isSetTodayOption = options["Today"];
            bool isSetYesterdayOption = options["Yesterday"];
            bool isSetWeekOption = options["Week"];

            //Check if more than one option is set
            var dictionaryValues = options.Values.ToList();
            if(dictionaryValues.Where(o => o == true).Count() > 1)
            {
                _console.ForegroundColor = ConsoleColor.Red;
                _console.WriteLine("Only one option should be set !!");
                _console.ResetColor();
                return Task.FromResult(1);
            }

            // Init collection
            IEnumerable<Note> notes = null;
                
            //Filter notes
            if (isSetYesterdayOption)
            {
                notes = _notes.Where(n => n.WriteDate.Date == DateTime.Today.AddDays(-1));
            }

            else
            {
                if (isSetWeekOption)
                {
                    notes = _notes.Where(n => CheckIfSameWeek(n.WriteDate, DateTime.Today));
                }
                else
                {
                    notes = _notes.Where(n => n.WriteDate.Date == DateTime.Today);
                }
            }
                

            foreach (var note in notes)
            {
                _console.WriteLine($"- {note.WriteDate} : {note.Content}");
            }

            return Task.FromResult(1);
        }

        public async Task<int> WriteNoteAsync(string content)
        {
            //Note file path
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notes.txt");

            //Remove double dots
            if (content.Contains("="))
            {
                content = content.Replace("=", "->");
            }

            //Format note
            string note = $"{DateTime.Now}={content}";
            _console.WriteLine($"Time : {DateTime.Now}, Content : {content}");

            //Write to file
            await File.AppendAllLinesAsync(path, new string[] { note });

            return 1;
        }

        private bool CheckIfSameWeek(DateTime date1, DateTime date2)
        {
            var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        }

        /// <summary>
        /// Read notes file and parse lines to notes
        /// </summary>
        /// <returns>List of notes</returns>
        private List<Note> GetNotes()
        {
            //File path
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notes.txt");

            //Read file
            string[] fileLines = File.ReadAllLines(path);

            var notes = new List<Note>();
            foreach (var line in fileLines)
            {
                //Parse each line into a note
                notes.Add(NoteParser.DeserializeNote(line));
            }

            return notes;
        }
    }
}
