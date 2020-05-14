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
    [CNodeStart]
    public class EventTickNode : CNodeBase {
#if (Debug_OldBugTesting)
		public EventTickNode(Guid guid, FlowChart flowChart) : base( guid, flowChart, CNodeType.EventTick) {
#else
        public EventTickNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.EventTick) {
#endif
            Header = "Event: Tick";
            HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(92, 20, 15));

            //var color = Color.FromRgb(92, 20, 15);
            //var gradient = new LinearGradientBrush();
            //gradient.GradientStops.Add(new GradientStop() { Color = color, Offset = 0 });
            //gradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, color.R, color.G, color.B), Offset = 0.8 });
            //gradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(80, color.R, color.G, color.B), Offset = 0.96 });
            //gradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(40, color.R, color.G, color.B), Offset = 1.0 });
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

        public override string CompileAsJavascript() {
            if (this.OutputFlowPorts.Count == 0 
                || this.OutputFlowPorts[0].Connectors.Count == 0
                || this.OutputFlowPorts[0].Connectors[0].EndPort == null) {
                return $"application.Tick(() => {{ /* ... */ }});";
            }

            var owner = this.OutputFlowPorts[0].Connectors[0].EndPort.Owner;
            if (!(owner is CNodeBase))
                return $"application.Tick(() => {{ /** ... **/ }});";

            var code = (owner as CNodeBase).CompileAsJavascript();

            // Get the output node
            return $"application.Tick(() => {{{code}}});";
        }
    }
}
