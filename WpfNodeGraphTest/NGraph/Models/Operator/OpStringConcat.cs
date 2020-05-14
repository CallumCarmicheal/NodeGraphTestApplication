using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfNodeGraphTest.NGraph.Models.Operator {
    [Node]
    public class OpStringConcat : CNodeBase {
        public OpStringConcat(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.OpIntegerAdd) {
            var color = Color.FromRgb(0x64, 0xDD, 0x17);
            var gradient = new LinearGradientBrush();
            gradient.GradientStops.Add(new GradientStop() { Color = color, Offset = 0 });
            gradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(255, color.R, color.G, color.B), Offset = 0.8 });
            gradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(80, color.R, color.G, color.B), Offset = 0.96 });
            gradient.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(40, color.R, color.G, color.B), Offset = 1.0 });

            Header = "Op: String + String";
            HeaderBackgroundColor = gradient;
        }

        public NodePropertyPort PortLHS { get; private set; }
        public NodePropertyPort PortSeperator { get; private set; }
        public NodePropertyPort PortRHS { get; private set; }
        public NodePropertyPort PortOutput { get; private set; }

        public override void OnCreate() {
            PortLHS = createPort("Lhs", true, true);
            PortSeperator = createPort("Seperator", true, true);
            PortRHS = createPort("Rhs", true, true);
            PortOutput = createPort("Output", false, false);

            base.OnCreate();
        }

        private NodePropertyPort createPort(string title, bool isInput, bool editor = true) {
            var ValuePort = NodeGraphManager.CreateNodePropertyPort(
                false, Guid.NewGuid(), this,
                isInput, typeof(string), "", title, editor, null, "");

            ValuePort.PropertyChanged += Port_PropertyChanged;

            return ValuePort;
        }

        private void Port_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            RegisterStateChange();
        }

        public override string CompileAsJavascript() {
            var txtLhs = "";
            var txtSeperator = "";
            var txtRhs = "";

            if (InputPropertyPorts[0].Connectors.Count > 0 
                    && InputPropertyPorts[0].Connectors[0].StartPort != null) {
                // Get the integer
                var owner = this.InputPropertyPorts[0].Connectors[0].StartPort.Owner;
                if (owner != null && owner is CNodeBase)
                    txtLhs = string.Format("({0})", (owner as CNodeBase).CompileAsJavascript());
                else txtLhs = $"\"{PortLHS.Value}\"";
            } else {
                txtLhs = $"\"{PortLHS.Value}\"";
            }

            if (InputPropertyPorts[1].Connectors.Count > 0
                   && InputPropertyPorts[1].Connectors[0].StartPort != null) {
                // Get the integer
                var owner = this.InputPropertyPorts[1].Connectors[0].StartPort.Owner;
                if (owner != null && owner is CNodeBase)
                    txtRhs = string.Format("({0})", (owner as CNodeBase).CompileAsJavascript());
                else txtSeperator = "" + PortSeperator.Value;
            } else {
                txtSeperator = $"\"{PortSeperator.Value}\"";
            }

            if (InputPropertyPorts[2].Connectors.Count > 0
                    && InputPropertyPorts[2].Connectors[0].StartPort != null) {
                // Get the integer
                var owner = this.InputPropertyPorts[2].Connectors[0].StartPort.Owner;
                if (owner != null && owner is CNodeBase)
                    txtRhs = string.Format("({0})", (owner as CNodeBase).CompileAsJavascript());
                else txtRhs = $"\"{PortRHS.Value}\"";
            } else {
                txtRhs = $"\"{PortRHS.Value}\"";
            }

            return "({0}) + ({1}) + ({2})".Format(txtLhs, txtSeperator, txtRhs);
        }
    }
}
