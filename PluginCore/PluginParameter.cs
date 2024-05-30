using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDirectory.PluginCore
{
    public class PluginParameters
    {
        public HashSet<object> Context { get; set; } = new HashSet<object>();
    }
}
