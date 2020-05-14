using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMVC.Helpers
{
    public class BufferController
    {
        private Dictionary<string, object> buffer = new Dictionary<string, object>();
        public object this[string key]
        {
            get
            {
                if (buffer.ContainsKey(key))
                    return buffer[key];
                else
                    return null;
            }
            set
            {
                if (buffer.ContainsKey(key))
                    buffer[key] = value;
                else
                    buffer.Add(key, value);
            }
        }
    }
}
