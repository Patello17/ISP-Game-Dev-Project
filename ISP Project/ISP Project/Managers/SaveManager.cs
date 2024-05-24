using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ISP_Project.Managers
{
    // Code Reference: https://www.youtube.com/watch?v=gYksT0d_xLM
    public class SaveFile
    {
        public int LevelsCompleted { get; set; }
        public int LevelOneFewestMoves { get; set; }
        public int LevelTwoFewestMoves { get; set; }
        public int LevelThreeFewestMoves { get; set; }
    }

    // Code Reference: https://www.youtube.com/watch?v=gYksT0d_xLM
    public class ReadEnvelopesFile
    {
        public bool ReadOne { get; set; }
        public bool ReadTwo { get; set; }
        public bool ReadThree { get; set; }
        public bool ReadFour { get; set; }
    }

    // Code Reference: https://www.youtube.com/watch?v=gYksT0d_xLM
    public static class SaveManager
    {
        private const string SAVE_PATH = "save_file.json";
        private const string READ_ENVELOPES_PATH = "read_envelopes.json";

        /// <summary>
        /// Saves information to a JSON file.
        /// </summary>
        /// <param name="saveFile"></param>
        public static void Save(SaveFile saveFile)
        {
            string serializedText = JsonSerializer.Serialize<SaveFile>(saveFile);
            File.WriteAllText(SAVE_PATH, serializedText);
        }

        /// <summary>
        /// Loads information from a JSON file.
        /// </summary>
        /// <returns></returns>
        public static SaveFile Load()
        {
            var fileContents = File.ReadAllText(SAVE_PATH);
            return JsonSerializer.Deserialize<SaveFile>(fileContents);
        }

        public static void NewSave()
        {
            SaveFile newSave = new SaveFile()
            {
                LevelsCompleted = 0,
                LevelOneFewestMoves = 0,
                LevelTwoFewestMoves = 0,
                LevelThreeFewestMoves = 0
            };
            Save(newSave);
        }

        /// <summary>
        /// Saves information to a JSON file.
        /// </summary>
        /// <param name="saveFile"></param>
        public static void SaveReadEnvelopes(ReadEnvelopesFile saveFile)
        {
            string serializedText = JsonSerializer.Serialize<ReadEnvelopesFile>(saveFile);
            File.WriteAllText(READ_ENVELOPES_PATH, serializedText);
        }
        /// <summary>
        /// Loads information from a JSON file.
        /// </summary>
        /// <returns></returns>
        public static ReadEnvelopesFile LoadReadEnvelopes()
        {
            var fileContents = File.ReadAllText(READ_ENVELOPES_PATH);
            return JsonSerializer.Deserialize<ReadEnvelopesFile>(fileContents);
        }

        public static void NewReadEnvelopesSave()
        {
            ReadEnvelopesFile newSave = new ReadEnvelopesFile()
            {
                ReadOne = false,
                ReadTwo = false,
                ReadThree = false,
                ReadFour = false
            };
            SaveReadEnvelopes(newSave);
        }
    }
}
