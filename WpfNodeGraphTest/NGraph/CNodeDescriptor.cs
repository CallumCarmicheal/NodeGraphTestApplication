using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.NGraph {
    class CNodeDescriptor : Attribute {
        public string Title { get; set; }
        public string Description { get; set; }

        public CNodeDescriptor(string title, string description = null) {
            Title = title;
            Description = description;
        }
    }
}
