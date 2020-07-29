using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleBlankApplication.Nodes.Nodes {
    [Node]
    [NodeFlowPort("Input", "", true)]
    public class FinishNode : CBaseNode {


        public FinishNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart) {
            //
            Header = "Return";
            HeaderFontColor = Brushes.White;
            HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(51, 20, 30));
        }
    }
}
