using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfNodeGraphTest.NGraph.Models.Functions {
    [Node]
    [CNodeDescriptor("Function: Print Text")]
    [NodeFlowPort("Input", "", true)]
    [NodeFlowPort("Output", "", false)]
    public class FnPrint : CNodeBase {
		// ====================================================
		// ====  Properties

		[NodePropertyPort(displayName: "Integer", isInput: true, valueType: typeof(int), defaultValue: 0, hasEditor: true)]
		public int Integer;


		// ====================================================
		// ====  Functions


		public FnPrint(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.FunctionPrint) {
			Header = "Function: Print";
			HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(71,116,143));
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
