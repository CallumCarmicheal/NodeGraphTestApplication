using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfNodeGraphTest.NGraph {
    [Node()]
    public abstract class CVariableNode<T> : CNodeBase {
        #region Propertiets

        private object _Value;
        public object Value {
            get { return _Value; }
            set {
                if (_Value != value) {
                    _Value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public NodePropertyPort ValuePort { get; private set; }

        #endregion // Properites

        #region Constructor
#if (Debug_OldBugTesting)
        public VariableNode(Guid guid, FlowChart flowChart, CNodeType nodeType) : base(guid, flowChart, nodeType)
#else
        public CVariableNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart, CNodeType nodeType) : base(ngm, guid, flowChart, nodeType)
#endif
        {
            Header = typeof(T).Name;
            HeaderBackgroundColor = Brushes.Black;
            HeaderFontColor = Brushes.White;
            AllowCircularConnection = false;
        }

        #endregion // Constructor

        #region Events

        public override void OnCreate() {
            Type type = typeof(T);

            ValuePort = NodeGraphManager.CreateNodePropertyPort(
                false, Guid.NewGuid(), this, 
                false, type, CreateDefaultInstance(), "Value", true, null, "");

            ValuePort.PropertyChanged += ValuePort_PropertyChanged;
            ValuePort.DynamicPropertyPortValueChanged += ValuePort_PropertyPortValueChanged;

            base.OnCreate();
        }

        public virtual T CreateDefaultInstance() {
            Type type = typeof(T);
            return (T)Activator.CreateInstance(type);
        }

        private void ValuePort_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            RegisterStateChange();
        }

        private void ValuePort_PropertyPortValueChanged(NodePropertyPort port, object prevValue, object newValue) {
            Value = (T)port.Value;
            RegisterStateChange();
        }


        #endregion // Events
    }
}
