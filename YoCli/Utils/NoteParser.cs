using System;
using System.Globalization;
using YoCli.Models;

namespace YoCli.Utils
{
    /// <summary>
    /// Class providing methods for parsing file line to Note object
    /// </summary>
    public class NoteParser
    {
        /// <summary>
        /// Parse file line to note object
        /// </summary>
        /// <param name="line">Line in note file</param>
        /// <returns></returns>
        public static Note DeserializeNote(string line)
        {
            //Line
            var noteLine = line.Split('=');

            DateTime writtenDate;
            var content = noteLine[1];

            //Parse date
            try
            {
                writtenDate = DateTime.Parse(noteLine[0]);
            }
            catch (Exception)
            {
                return null;
            }

            
            return new Note
            {
                WriteDate = writtenDate,
                Content = content
            };
        }
    }
}
