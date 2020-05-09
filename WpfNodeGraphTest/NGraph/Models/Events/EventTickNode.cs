using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfNodeGraphTest.NGraph.Models.Events {
	[Node()]
	[NodeFlowPort("Output", "", false)]
	[CNodeDescriptor(title: "Event Tick")]
	public class EventTickNode : CNodeBase {
		public EventTickNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.EventTick) {
			Header = "Event: Tick";
			HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(92,20,15));
		}

		// ====================================================
		// ====  Events

		public override void OnPreExecute(Connector prevConnector) {
			base.OnPreExecute(prevConnector);
		}

		public override void OnExecute(Connector prevConnector) {
			base.OnExecute(prevConnector);
		}

		public override void OnPostExecute(Connector prevConnector) {
			base.OnPostExecute(prevConnector);

			NodeFlowPort port = NodeGraphManager.FindNodeFlowPort(this, "Output");
			foreach (var connector in port.Connectors) {
				connector.OnPreExecute();
				connector.OnExecute();
				connector.OnPostExecute();
			}
		}
	}
}
