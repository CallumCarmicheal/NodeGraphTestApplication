using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlankApplication.Nodes.Variables {
    //
    // Defines a basic set of nodes
    //      Int
    //      Double
    //      Float
    //      String
    //      Bool


    [Node]
    public class ConstIntNode : CVariableNode<int> {
        public ConstIntNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart) {
            //
        }
    }

    [Node]
    public class ConstDoubleNode : CVariableNode<double> {
        public ConstDoubleNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart) {
            //
        }
    }

    [Node]
    public class ConstFloatNode : CVariableNode<float> {
        public ConstFloatNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart) {
            //
        }
    }

    [Node]
    public class ConstStringNode : CVariableNode<string> {
        public ConstStringNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart) {
            //
        }
    }




}
