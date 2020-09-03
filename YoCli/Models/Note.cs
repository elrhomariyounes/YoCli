using System;

namespace YoCli.Models
{
    /// <summary>
    /// Note model
    /// </summary>
    public class Note
    {
        /// <summary>
        /// The date when the note was written
        /// </summary>
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// The content of the note
        /// </summary>
        public string  Content { get; set; }

    }
}
