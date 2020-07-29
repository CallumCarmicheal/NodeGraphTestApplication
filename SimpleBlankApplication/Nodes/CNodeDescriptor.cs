using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlankApplication.Nodes {
    class CNodeDescriptor : Attribute {
        public string Title { get; set; }
        public string Description { get; set; }

        public CNodeDescriptor(string title, string description = null) {
            Title = title;
            Description = description;
        }
    }
}
