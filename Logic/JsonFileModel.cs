using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace databaseEditor.Logic
{
    public class Entry
    {
        public Label label { get; set; }
        public JsonDocument data { get; set; }
    }

    public class Label
    {
        public bool existence { get; set; }
        public bool property { get; set; }
        public bool executive { get; set; }
    }
}
