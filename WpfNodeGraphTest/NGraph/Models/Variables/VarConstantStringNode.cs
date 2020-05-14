using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfNodeGraphTest.NGraph.Models.Variables {
    [Node]
    public class VarConstantStringNode : CVariableNode<string> {
#if (Debug_OldBugTesting)
        public VarConstantStringNode(Guid guid, FlowChart flowChart) : base(guid, flowChart, CNodeType.VarInteger) {
#else
        public VarConstantStringNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.VarConstantInteger) {
#endif
            //
            //HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(157, 248, 67));
            HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(0x64, 0xDD, 0x17));
        }

        public override string CompileAsJavascript() {
            return $"\"{Value}\"";
        }

        public override string CreateDefaultInstance() {
            return "";
        }
    }
}
