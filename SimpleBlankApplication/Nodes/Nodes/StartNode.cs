using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NodeGraph;
using NodeGraph.Model;

namespace SimpleBlankApplication.Nodes.Nodes {
    [NodeFlowPort("Output", "", false)]
    public class StartNode : CBaseNode {

        public StartNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart) {
            //
            Header = "Start Node";
            HeaderFontColor = Brushes.White;
            HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(92, 20, 15));
        }

        
    }
}
