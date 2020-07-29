using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using NodeGraph;
using NodeGraph.Model;

namespace SimpleBlankApplication.Nodes {
    [Node]
    public class CVariableNode<T> : CBaseNode {

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

        public CVariableNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) : base(ngm, guid, flowChart) {
            Header = typeof(T).Name;
            HeaderBackgroundColor = Brushes.Black;
            HeaderFontColor = Brushes.White;
        }

        #endregion // Constructor


        #region Overridable

        public virtual T CreateDefaultInstance() {
            Type type = typeof(T);
            return (T)Activator.CreateInstance(type);
        }

        #endregion // Overridable


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

        private void ValuePort_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            RegisterStateChange();
        }

        private void ValuePort_PropertyPortValueChanged(NodePropertyPort port, object prevValue, object newValue) {
            Value = (T)port.Value;
            RegisterStateChange();
        }

        #endregion
    }
}
