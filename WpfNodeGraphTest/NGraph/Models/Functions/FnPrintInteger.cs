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
    [CNodeDescriptor("Function: Print Integer")]
    [NodeFlowPort("Input", "", true)]
    [NodeFlowPort("Output", "", false)]
    public class FnPrintInteger : CNodeBase {
		// ====================================================
		// ====  Properties

		[NodePropertyPort(displayName: "Integer", isInput: true, valueType: typeof(int), defaultValue: 0, hasEditor: true)]
		public int Integer;


		// ====================================================
		// ====  Functions

#if (Debug_OldBugTesting)
		public FnPrint(Guid guid, FlowChart flowChart) : base(guid, flowChart, CNodeType.FunctionPrint)
#else
		public FnPrintInteger(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.FunctionPrintInteger)
#endif
		{
			Header = "Function: Print Integer";
			HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(71,116,143));

			//var gradient = new LinearGradientBrush();
			//for(int x = 0; x < 10; x++) 
			//	gradient.GradientStops.Add(new GradientStop(Rainbow(x), x / 10));

			//HeaderBackgroundColor = gradient;
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




		// ====================================================
		// ==== Javascript

		public override string CompileAsJavascript() {
			var format = "Print({0});\n";
			string output = "";

			if (InputPropertyPorts[0].Connectors.Count > 0 && InputPropertyPorts[0].Connectors[0].StartPort != null) {
				// Get the integer
				var owner = this.InputPropertyPorts[0].Connectors[0].StartPort.Owner;
				if (owner != null && owner is CNodeBase) 
					output = string.Format(format, (owner as CNodeBase).CompileAsJavascript());
			} else {
				output = string.Format(format, Integer);
			}


			// We have more calls to make
			if (OutputFlowPorts[0].Connectors.Count > 0 && this.OutputFlowPorts[0].Connectors[0].EndPort != null) {
				var owner = this.OutputFlowPorts[0].Connectors[0].EndPort.Owner;
				if (!(owner is CNodeBase))
					return $"application.Tick(() => {{ /** ... **/ }});";

				var code = (owner as CNodeBase).CompileAsJavascript();
				output += code;
			}

			return output;
		}
	}
}
