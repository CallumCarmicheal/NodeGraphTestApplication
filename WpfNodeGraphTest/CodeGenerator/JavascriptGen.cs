using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfNodeGraphTest.NGraph;

namespace WpfNodeGraphTest.CodeGenerator {
    class JavascriptGen {

        private NodeGraphManager ngm;
        public JavascriptGen(string xml) {
            ngm = new NodeGraphManager();
            ngm.DeserialzeFromString(xml);
        }

        public string CompileJavascript() {
            return "";
        }
    }


    public class Scope {
        public Guid Guid { get; private set; }

        public Dictionary<Guid, Scope> Scopes { get; private set; } = new Dictionary<Guid, Scope>();
        public List<IJavascriptOperation> Operations = new List<IJavascriptOperation>();

        public Scope(Guid guid) {
            this.Guid = guid;
        }
    }

    public interface IJavascriptOperation {
        string GetCode();
    }

    public class FunctionOperation : IJavascriptOperation {
        public string FunctionName = "console.log";
        public List<IJavascriptOperation> Parameters = new List<IJavascriptOperation>();
        public Dictionary<Guid, Local> Locals = new Dictionary<Guid, Local>();

        public class Parameter {
            public Local LocalData = null;
            public string Value = "";
        }

        public class Local {
            public Guid Guid;
            public string ValueCode = "";
        }

        public string GetCode() => "";
    }

    public class VariableOperation {

    }



    [Node]
    public class EventTest : NGraph.CNodeBase, ILexerableNode {
        public EventTest(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.EventTick) {
            Header = "COMP: I32_Add";
            HeaderBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }

        public NodePropertyPort PortLHS { get; private set; }
        public NodePropertyPort PortRHS { get; private set; }
        public NodePropertyPort PortOutput { get; private set; }

        public override void OnCreate() {
            PortLHS = createPort("Lhs", true, true);
            PortRHS = createPort("Rhs", true, true);
            PortOutput = createPort("Output", false, false);

            base.OnCreate();
        }

        private NodePropertyPort createPort(string title, bool isInput, bool editor = true) {


            var ValuePort = NodeGraphManager.CreateNodePropertyPort(
                false, Guid.NewGuid(), this,
                isInput, typeof(int), Activator.CreateInstance(typeof(int)), title, editor, null, "");

            return ValuePort;
        }

        public override string CompileAsJavascript() => "";

        public LexerScope GetScope(Lexer lexer, LexerScope scope) {
            int targetPort = 0;

            // var scope = lexer.NewScope(this);
            scope.AddOperation(new Op_I32_Add());

            if (InputPropertyPorts[targetPort].Connectors.Count > 0
                   && InputPropertyPorts[targetPort]?.Connectors[0]?.StartPort?.Owner != null
                   && InputPropertyPorts[targetPort]?.Connectors[0]?.StartPort?.Owner is CNodeBase) {
                // Pass reference
                scope.AddOperation(new Ref_NodePort(InputPropertyPorts[0].Connectors[0].StartPort));
            } else {
                // Get data
                scope.AddOperation(new Const_I32 { Constant = (int)PortLHS.Value });
            }

            targetPort = 1;
            if (InputPropertyPorts[targetPort].Connectors.Count > 0
                  && InputPropertyPorts[targetPort]?.Connectors[0]?.StartPort?.Owner != null
                  && InputPropertyPorts[targetPort]?.Connectors[0]?.StartPort?.Owner is CNodeBase) {
                // Pass reference
                scope.AddOperation(new Ref_NodePort(InputPropertyPorts[0].Connectors[0].StartPort));
            } else {
                // Get data
                scope.AddOperation(new Const_I32 { Constant = (int)PortLHS.Value });
            }

            scope.AddOperation(new Ret_I32 { });

            return scope;
        }

        public Guid GetNodeGuid() => this.Guid;
    }

    [Node]
    [CNodeDescriptor("Comp: Comp_FnPrintInt")]
    [NodeFlowPort("Input", "", true)]
    [NodeFlowPort("Output", "", false)]
    public class Comp_FnPrintInt : CNodeBase, ILexerableNode {
        // ====================================================
        // ====  Properties

        [NodePropertyPort(displayName: "Integer", isInput: true, valueType: typeof(int), defaultValue: 0, hasEditor: true)]
        public int Integer;

        // ====================================================
        // ====  Functions
        public Comp_FnPrintInt(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart, CNodeType.FunctionPrintInteger) {
            Header = "Comp_FnPrintInt";
            HeaderBackgroundColor = new SolidColorBrush(Color.FromRgb(71, 116, 143));
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

        public Guid GetNodeGuid() => this.Guid;

        public LexerScope GetScope(Lexer lexer, LexerScope scope) {
            int targetPort = 0;

            // var scope = lexer.NewScope(this);
            scope.AddOperation(new Op_CallFunction { Function = "console.log" });

            if (InputPropertyPorts[targetPort].Connectors.Count > 0
                   && InputPropertyPorts[targetPort]?.Connectors[0]?.StartPort?.Owner != null
                   && InputPropertyPorts[targetPort]?.Connectors[0]?.StartPort?.Owner is CNodeBase) {
                // Pass reference
                scope.AddOperation(new Ref_NodePort(InputPropertyPorts[0].Connectors[0].StartPort));
            } else {
                // Get data
                //scope.AddOperation(new Const_I32 { Constant = (int)PortLHS.Value });
            }

            return null;
        }
    }


    public interface ILexerableNode {
        Guid GetNodeGuid();
        LexerScope GetScope(Lexer lexer, LexerScope scope);
    }

    public class Lexer {
        public Dictionary<ILexerableNode, LexerScope> Scopes = new Dictionary<ILexerableNode, LexerScope>();
        public Dictionary<Guid, LexerScope> ScopeLookup = new Dictionary<Guid, LexerScope>();


        public LexerScope NewScope(ILexerableNode node) {
            Guid guid = Guid.NewGuid();

            LexerScope lexerScope = new LexerScope(node, this, guid);
            Scopes.Add(node, lexerScope);
            ScopeLookup.Add(guid, lexerScope);

            return lexerScope;
        }
    }

    public class LexerScope {
        private Lexer Lexer;
        private ILexerableNode currentNode;
        public Guid Guid { get; private set; }
        public List<LexerOperations> LexerOperations = new List<LexerOperations>();


        public LexerScope(ILexerableNode node, Lexer lexer, Guid guid) {
            Lexer = lexer;
            Guid = guid;
            currentNode = node;
        }

        public void AddOperation(LexerOperations op) {
            LexerOperations.Add(op);
        }

        public void NewScopeAndRender(ILexerableNode node) {
            var scope = Lexer.NewScope(node);

            AddOperation(new Op_RenderScope() { Scope = scope }); ;
        }
    }

    public class LexerOperations {
        //
    }

    public class Op_I32_Add : LexerOperations {

    }

    public class Op_CallFunction : LexerOperations {
        public string Function { get; set; }

        public List<LexerOperations> ParamOps = new List<LexerOperations>();

        public void AddParamter(LexerOperations op) {
            
        }
    }

    public class Op_Paramter : LexerOperations { 
        //
    }

    public class Const_I32 : LexerOperations {
        public int Constant = 0;
    }

    public class Ret_I32 : LexerOperations {

    }

    public class Ref_NodePort : LexerOperations {
        public NodePort Port { get; set; }

        public Ref_NodePort(NodePort port) {
            Port = port;
        }
    }

    public class Op_RenderScope : LexerOperations {
        public LexerScope Scope { get; set; }
    }
}
