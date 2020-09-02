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
        /// <returns></returns>
        Task<int> ReadTodayNotesAsync();
    }
}
