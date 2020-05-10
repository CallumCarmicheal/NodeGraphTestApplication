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

#if (Debug_OldBugTesting)
		public FnPrint(Guid guid, FlowChart flowChart) : base(guid, flowChart, CNodeType.FunctionPrint)
#else
		public FnPrint(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.FunctionPrint)
#endif
		{
			Header = "Function: Print";
			HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(71,116,143));

			//var gradient = new LinearGradientBrush();
			//for(int x = 0; x < 10; x++) 
			//	gradient.GradientStops.Add(new GradientStop(Rainbow(x), x / 10));

			//HeaderBackgroundColor = gradient;
		}

		private Color Rainbow(float progress) {
			float div = (Math.Abs(progress % 1) * 6);
			byte ascending = (byte)((div % 1) * 255);
			byte descending = (byte)(255 - ascending);

			switch ((int)div) {
			case 0:
				return Color.FromArgb(255, 255, ascending, 0);
			case 1:
				return Color.FromArgb(255, descending, 255, 0);
			case 2:
				return Color.FromArgb(255, 0, 255, ascending);
			case 3:
				return Color.FromArgb(255, 0, descending, 255);
			case 4:
				return Color.FromArgb(255, ascending, 0, 255);
			default: // case 5:
				return Color.FromArgb(255, 255, 0, descending);
			}
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
