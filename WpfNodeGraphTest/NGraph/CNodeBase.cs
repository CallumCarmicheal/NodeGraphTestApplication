using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.NGraph {
    public class CNodeBase : Node {
		#region Properties

		private CNodeType _NodeType;
		public CNodeType NodeType {
			get { return _NodeType; }
			set {
				if (value != _NodeType) {
					_NodeType = value;
					RaisePropertyChanged("NodeType");
				}
			}
		}

		#endregion // Properties

		#region Constructor

		public CNodeBase(NodeGraphManager ngm, Guid guid, FlowChart flowChart, CNodeType nodeType) : base(ngm, guid, flowChart) {
			_NodeType = nodeType;
			AllowEditingHeader = false;
		}

		#endregion // Constructor

	}
}
