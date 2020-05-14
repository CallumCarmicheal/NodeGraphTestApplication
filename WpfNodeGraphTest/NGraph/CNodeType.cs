using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.NGraph {
    public enum CNodeType {
        // ===============================
        // Events

        EventTick,

        // ===============================
        // Functions

        FunctionPrintInteger,
        FunctionPrintString,

        // ===============================
        // Variable types

        VarConstantInteger,
        VarConstantString,

        // ===============================
        // Logical operators

        OpIntegerAdd,

        // ===============================
    }
}
