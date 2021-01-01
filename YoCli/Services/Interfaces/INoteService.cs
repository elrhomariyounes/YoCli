using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoCli.Services.Interfaces
{
    /// <summary>
    /// Note service providing methods for writing and reading notes
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Write a note
        /// </summary>
        /// <param name="content">The content of the note</param>
        /// <returns></returns>
        Task<int> WriteNoteAsync(string content);

        /// <summary>
        /// Fetch notes written today
        /// </summary>
        /// <param name="options">Dictionary of sub command options</param>
        /// <returns></returns>
        Task<int> ReadNotesAsync(Dictionary<string, bool> options);

        /// <summary>
        /// Find notes
        /// </summary>
        /// <param name="content"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<int> FindNotesAsync(string content, Dictionary<string, int> options);

        /// <summary>
        /// Exports notes to JSON file
        /// </summary>
        /// <param name="path">Path to save the JSON file</param>
        /// <returns></returns>
        Task<int> ExportNotesToJsonFileAsync(string path);

        /// <summary>
        /// Imports notes from exported JSON file
        /// </summary>
        /// <param name="path">Path to exported JSON file</param>
        /// <returns></returns>
        Task<int> ImportNotesFromJsonFileAsync(string path);

        /// <summary>
        /// Remove note written in a specified date
        /// </summary>
        /// <param name="date">date filter</param>
        /// <returns></returns>
        Task<int> RemoveNoteByDateAsync(string date);
    }
}
