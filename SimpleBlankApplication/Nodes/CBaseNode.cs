using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlankApplication.Nodes {
    public abstract class CBaseNode : Node {


        public CBaseNode(NodeGraphManager ngm, Guid guid, FlowChart flowChart) 
                : base(ngm, guid, flowChart) {
            // 
        }

        #region Changed Events 

        public override void OnCreate() {
            base.OnCreate();

            // Add a collection changed listen to each of our ports
            //  this will allow us to call the StateChanged event when ever something is added/removed.
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

        public delegate void DNodeStateChanged(CBaseNode node);
        public event DNodeStateChanged StateChanged;

        protected void RegisterStateChange() {
            StateChanged?.Invoke(this);
        }

        #endregion
    }
}
