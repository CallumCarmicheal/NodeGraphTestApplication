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
    public class VarIntegerNode : VariableNode<int> {
#if (Debug_OldBugTesting)
        public VarIntegerNode(Guid guid, FlowChart flowChart) : base(guid, flowChart, CNodeType.VarInteger) {
#else
        public VarIntegerNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.VarInteger) {
#endif
            //
            //HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(157, 248, 67));
            HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(71,146,0));
        }
    }
}
