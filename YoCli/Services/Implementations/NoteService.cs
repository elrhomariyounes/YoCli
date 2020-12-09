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
using System.Text.Json;
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

        public Task<int> ExportNotesToJsonFileAsync(string path)
        {
            if (Directory.Exists(path))
            {
                var json = JsonSerializer.Serialize(_notes);
                File.WriteAllText(Path.Combine(path, "notes.json"), json);
                _console.ForegroundColor = ConsoleColor.Green;
                _console.WriteLine("Notes exported");
                _console.ResetColor();
                return Task.FromResult(1);
            }

            _console.ForegroundColor = ConsoleColor.DarkRed;
            _console.WriteLine("Invalid path!");
            _console.ResetColor();
            return Task.FromResult(-1);
        }

        public Task<int> FindNotesAsync(string content, Dictionary<string, int> options)
        {
            IEnumerable<Note> notes = new List<Note>(this._notes);
            int day = options["Day"];
            int month = options["Month"];

            //Check if there is no options
            if (String.IsNullOrEmpty(content) && day == 0 && month == 0)
            {
                _console.ForegroundColor = ConsoleColor.DarkRed;
                _console.WriteLine("Invalid command please choose an option. Run 'yo find --help' for more details");
                _console.ResetColor();
                return Task.FromResult(-1);
            }
            else
            {
                int monthFilter = month;
                if (month == 0)
                    monthFilter = DateTime.Today.Month;
                    
                // Filter the content
                if (!String.IsNullOrEmpty(content))
                {
                    notes = notes.Where(n => n.Content.ToLower().Contains(content.ToLower()));
                }

                //Filter by month
                if(month != 0)
                {
                    notes = notes.Where(
                        n => n.WriteDate.Date.Month == month &&
                        n.WriteDate.Date.Year == DateTime.Today.Year);
                }

                //Filter by day
                if (day != 0)
                {
                    notes = notes.Where(
                        n => n.WriteDate.Date.Day == day &&
                        n.WriteDate.Date.Month == monthFilter &&
                        n.WriteDate.Date.Year == DateTime.Today.Year);
                }

            }

            foreach (var note in notes)
            {
                _console.WriteLine($"- {note.WriteDate} : {note.Content}");
            }

            return Task.FromResult(1);
        }

        public Task<int> ImportNotesFromJsonFileAsync(string path)
        {
            if (File.Exists(path))
            {
                // Reading from file
                var json = File.ReadAllText(path);

                //Path to save notes
                var appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notes.txt");

                //Delete if already exists
                if (File.Exists(appPath))
                    File.Delete(appPath);
                try
                {
                    var notes = JsonSerializer.Deserialize<List<Note>>(json);
                    foreach (var note in notes)
                    {
                        //Formated note
                        string formatedNote = $"{note.WriteDate}={note.Content}";
                        File.AppendAllLines(appPath, new string[] { formatedNote });
                    }

                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.WriteLine("Notes imported");
                    _console.ResetColor();
                    return Task.FromResult(1);
                }
                catch (Exception)
                {
                    _console.ForegroundColor = ConsoleColor.DarkRed;
                    _console.WriteLine("Unable to import notes! Please check json file format in the documentation");
                    _console.ResetColor();
                    return Task.FromResult(-1);
                }
            }

            _console.ForegroundColor = ConsoleColor.DarkRed;
            _console.WriteLine("Unable to import notes!");
            _console.ResetColor();
            return Task.FromResult(-1);
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
                _console.ForegroundColor = ConsoleColor.DarkRed;
                _console.WriteLine("Only one option should be set !!");
                _console.ResetColor();
                return Task.FromResult(-1);
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

            //Remove equals
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
            if (File.Exists(path))
            {
                string[] fileLines = File.ReadAllLines(path);

                var notes = new List<Note>();
                foreach (var line in fileLines)
                {
                    //Parse each line into a note
                    notes.Add(NoteParser.DeserializeNote(line));
                }

                return notes;
            }

            return new List<Note>();
        }
    }
}
