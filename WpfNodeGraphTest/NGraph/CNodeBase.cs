using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.NGraph {
    public abstract class CNodeBase : Node {
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

#if (Debug_OldBugTesting)
		public CNodeBase(Guid guid, FlowChart flowChart, CNodeType nodeType) : base(guid, flowChart) {
#else
		public CNodeBase(NodeGraphManager ngm, Guid guid, FlowChart flowChart, CNodeType nodeType) : base(ngm, guid, flowChart) {
#endif
			_NodeType = nodeType;
			AllowEditingHeader = true;
		}

		#endregion // Constructor

		#region Events

		public override void OnCreate() {
			base.OnCreate();

			foreach (var x in InputFlowPorts)
				x.Connectors.CollectionChanged += CollectionChanged;

			foreach (var x in OutputFlowPorts)
				x.Connectors.CollectionChanged += CollectionChanged;

			foreach (var x in InputPropertyPorts)
				x.Connectors.CollectionChanged += CollectionChanged;

			foreach (var x in OutputPropertyPorts)
				x.Connectors.CollectionChanged += CollectionChanged;
		}

		private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			StateChanged?.Invoke(this);
		}



		#endregion

		public abstract string CompileAsJavascript();


		public delegate void DNodeStateChanged(CNodeBase node);
        public event DNodeStateChanged StateChanged;

		protected void RegisterStateChange() {
			StateChanged?.Invoke(this);
		}
	}
}
