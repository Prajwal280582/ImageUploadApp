using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploadApp.Shared
{
    public class InMemoryLog
    {
        //On change event to capture the state change
        public event Action OnChange;

        //List object is used to list the log
        public List<string> log { get; set; } = new();

        //Log items are added to list and change event is invoked.
        public void LogItem(string item)
        {
            log.Add(item);
            OnChange?.Invoke();
        }


    }
}
