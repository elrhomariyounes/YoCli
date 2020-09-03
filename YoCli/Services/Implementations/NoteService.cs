using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
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
            bool isSetTodayOption = options["Today"];
            bool isSetYesterdayOption = options["Yesterday"];

            IEnumerable<Note> notes = null;
            if(isSetTodayOption && isSetYesterdayOption)
            {
                _console.ForegroundColor = ConsoleColor.Red;
                _console.WriteLine("Only one option should be set !!");
                _console.ResetColor();
                return Task.FromResult(1);
            }
                
            //Filter notes
            if (isSetYesterdayOption)
                notes = _notes.Where(n => n.WriteDate.Date == DateTime.Today.AddDays(-1));
            else
                notes = _notes.Where(n => n.WriteDate.Date == DateTime.Today);

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
