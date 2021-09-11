using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploadApp.Shared
{
    public class InMemoryLog
    {
        public event Action OnChange;
        public List<string> log { get; set; } = new();

        public void LogItem(string item)
        {
            log.Add(item);
            OnChange?.Invoke();


        }


    }
}
