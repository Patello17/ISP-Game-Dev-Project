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
    }

    // Code Reference: https://www.youtube.com/watch?v=gYksT0d_xLM
    public static class SaveManager
    {
        private const string PATH = "save_file.json";

        /// <summary>
        /// Saves information to a JSON file.
        /// </summary>
        /// <param name="saveFile"></param>
        public static void Save(SaveFile saveFile)
        {
            string serializedText = JsonSerializer.Serialize<SaveFile>(saveFile);
            File.WriteAllText(PATH, serializedText);
        }

        /// <summary>
        /// Loads information from a JSON file.
        /// </summary>
        /// <returns></returns>
        public static SaveFile Load()
        {
            var fileContents = File.ReadAllText(PATH);
            return JsonSerializer.Deserialize<SaveFile>(fileContents);
        }

        public static void NewSave()
        {
            SaveFile newSave = new SaveFile()
            {
                LevelsCompleted = 0,
                LevelOneFewestMoves = 0,
                LevelTwoFewestMoves = 0,
            };
            Save(newSave);
        }
    }
}
