﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace _TERMINAL_
{
    public partial class Terminal
    {
        const string history_extension = "." + nameof(Terminal) + ".history.json.txt";
        static string HistoryPath(in string name) => Path.Combine(Application.streamingAssetsPath, name + history_extension);

        [SerializeField] List<string> history;
        int history_index;

        //----------------------------------------------------------------------------------------------------------

        public void SaveHistory(in string name)
        {
            string path = HistoryPath(name);
            lock (history)
                File.WriteAllLines(path, history);
        }

        public void ReadHistory(in string name)
        {
            string path = HistoryPath(name);
            lock (history)
                if (File.Exists(path))
                {
                    history = File.ReadAllLines(path).ToList();
                    history_index = history.Count;
                }
                else
                    history = new List<string>();
        }

        void AddToHistory(in string line)
        {
            lock (history)
            {
                history.Remove(line);
                history.Add(line);
                history_index = history.Count;
            }
        }

        bool GetHistory(in int increment, out string line)
        {
            lock (history)
            {
                if (history.Count == 0)
                {
                    line = null;
                    return false;
                }

                history_index += increment;
                if (history_index < 0)
                    history_index = history.Count - 1;
                else if (history_index > history.Count)
                    history_index = 0;

                if (history_index == history.Count)
                    line = string.Empty;
                else
                    line = history[history_index];

                return true;
            }
        }
    }
}