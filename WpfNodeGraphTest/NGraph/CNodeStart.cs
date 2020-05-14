using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.NGraph {
    /// <summary>
    /// States the current node is a start node that can be used to start the execution of the script
    ///     (cannot have a input execution)
    /// </summary>
    class CNodeStart : Attribute {
    }
}
