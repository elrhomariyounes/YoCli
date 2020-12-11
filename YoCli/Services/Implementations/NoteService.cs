using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoCli.Models;
using YoCli.Services.Interfaces;

namespace YoCli.Services.Implementations
{
    /// <inheritdoc cref="INoteService"/>
    public class NoteService : INoteService
    {
        private readonly string storageFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notes.json");

        private readonly IConsole _console;
        private readonly List<Note> _notes;

        public NoteService(IConsole console)
        {
            _console = console;
            _notes = new List<Note>(this.GetNotes());
        }

        public Task<int> ExportNotesToJsonFileAsync(string filePathToExport)
        {
            if (!Directory.Exists(filePathToExport))
            {
                ConsoleErrorMessage("Invalid path!");
                return Task.FromResult(-1);
            }

            File.Copy(storageFilePath, Path.Combine(filePathToExport, "notes.json"));

            ConsoleSuccessMessage("Notes exported");
            return Task.FromResult(1);
        }

        public Task<int> FindNotesAsync(string content, Dictionary<string, int> options)
        {
            IEnumerable<Note> notes = new List<Note>(_notes);
            int day = options["Day"];
            int month = options["Month"];

            // Check if there is no options
            if (string.IsNullOrEmpty(content) && day == 0 && month == 0)
            {
                ConsoleErrorMessage("Invalid command please choose an option. Run 'yo find --help' for more details");
                return Task.FromResult(-1);
            }

            // Filter by content
            if (!string.IsNullOrEmpty(content))
            {
                notes = notes.Where(n => n.Content.ToLower().Contains(content.ToLower()));
            }

            // Filter by month
            if (month != 0)
            {
                notes = notes.Where(n => n.WriteDate.Date.Month == month && n.WriteDate.Date.Year == DateTime.Today.Year);
            }

            // Filter by day
            if (day != 0)
            {
                notes = notes.Where(n => n.WriteDate.Date.Day == day && n.WriteDate.Date.Month == DateTime.Today.Month && n.WriteDate.Date.Year == DateTime.Today.Year);
            }

            notes.ToList().ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));
            
            return Task.FromResult(1);
        }

        public Task<int> ImportNotesFromJsonFileAsync(string filePathToImport)
        {
            if (!File.Exists(filePathToImport))
            {
                ConsoleErrorMessage("Unable to import notes!");
                return Task.FromResult(-1);
            }

            try
            {
                // Read from file
                var jsonToImport = File.ReadAllText(filePathToImport);

                // Deserialize notes to import
                var notesToImport = JsonConvert.DeserializeObject<List<Note>>(jsonToImport);

                _notes.AddRange(notesToImport);

                // Serialize notes to save
                var jsonToSave = JsonConvert.SerializeObject(_notes, Formatting.Indented);

                // Write to file
                File.WriteAllText(storageFilePath, jsonToSave);

                ConsoleSuccessMessage("Notes imported");
                return Task.FromResult(1);
            }
            catch (Exception)
            {
                ConsoleErrorMessage("Unable to import notes! Please check json file format in the documentation");
                return Task.FromResult(-1);
            }
        }

        public Task<int> ReadNotesAsync(Dictionary<string, bool> options)
        {
            // Check if more than one option is set
            if (options.Values.Count(o => o == true) > 1)
            {
                ConsoleErrorMessage("One option should be set!");
                return Task.FromResult(-1);
            }

            // Get options values
            bool isSetTodayOption = options["Today"];
            bool isSetYesterdayOption = options["Yesterday"];
            bool isSetWeekOption = options["Week"];

            // Filter notes
            if (isSetYesterdayOption)
            {
                _notes.Where(n => n.WriteDate.Date == DateTime.Today.AddDays(-1))
                      .ToList()
                      .ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));
            }
            else if (isSetWeekOption)
            {
                var c = CultureInfo.CurrentCulture.Calendar;
                var rule = CalendarWeekRule.FirstDay;
                var firstDayOfWeek = DayOfWeek.Monday;

                _notes.Where(n => c.GetWeekOfYear(n.WriteDate, rule, firstDayOfWeek) == c.GetWeekOfYear(DateTime.Today, rule, firstDayOfWeek))
                      .ToList()
                      .ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));
            }
            else // isSetTodayOption
            {
                _notes.Where(n => n.WriteDate.Date == DateTime.Today)
                      .ToList()
                      .ForEach(n => _console.WriteLine($"- {n.WriteDate} : {n.Content}"));
            }

            return Task.FromResult(1);
        }

        public Task<int> WriteNoteAsync(string content)
        {
            // Init note
            Note note = new Note() { WriteDate = DateTime.Now, Content = content };
            _notes.Add(note);

            _console.WriteLine($"Time : {note.WriteDate}, Content : {note.Content}");

            SaveNotes();

            return Task.FromResult(1);
        }

        /// <summary>
        /// Read notes from storage file
        /// </summary>
        /// <returns>List of notes</returns>
        private List<Note> GetNotes()
        {
            var notes = new List<Note>();

            if (File.Exists(storageFilePath))
            {
                var json = File.ReadAllText(storageFilePath);
                notes = JsonConvert.DeserializeObject<List<Note>>(json);
            }

            return notes;
        }

        /// <summary>
        /// Write notes to storage file
        /// </summary>
        private void SaveNotes()
        {
            var json = JsonConvert.SerializeObject(_notes, Formatting.Indented);
            File.WriteAllText(storageFilePath, json);
        }

        private void ConsoleErrorMessage(string message)
        {
            _console.ForegroundColor = ConsoleColor.DarkRed;
            _console.WriteLine(message);
            _console.ResetColor();
        }

        private void ConsoleSuccessMessage(string message)
        {
            _console.ForegroundColor = ConsoleColor.Green;
            _console.WriteLine(message);
            _console.ResetColor();
        }
    }
}
