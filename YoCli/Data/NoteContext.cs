using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using YoCli.Commons;
using YoCli.Models;

namespace YoCli.Data
{
    class NoteContext : IContext<Note>
    {
        private readonly string storageFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notes.json");

        public List<Note> Data { get; private set; }

        public NoteContext()
        {
            Data = GetAll();
        }

        public void SaveChanges()
        {
            var json = JsonSerializer.Serialize(Data);
            File.WriteAllText(storageFilePath, json);
        }

        private List<Note> GetAll()
        {
            var notes = new List<Note>();

            if (File.Exists(storageFilePath))
            {
                var json = File.ReadAllText(storageFilePath);
                notes = JsonSerializer.Deserialize<List<Note>>(json);
            }

            return notes;
        }
    }
}
