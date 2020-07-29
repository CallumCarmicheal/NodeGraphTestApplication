using NodeGraph;
using NodeGraph.Model;
using SimpleBlankApplication.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleBlankApplication.UI {
    /// <summary>
    /// Interaction logic for GraphEditor.xaml
    /// </summary>
    public partial class GraphEditor : UserControl {
        // MVVM
        public GraphEditorViewModel ViewModel { get => (GraphEditorViewModel) DataContext; set => DataContext = value; }

        public NodeGraphManager nodeGraphManager { get; private set; }
        public FlowChart flowChart { get; set; }

        public GraphEditor() {
            ViewModel = new GraphEditorViewModel();

            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if (nodeGraphManager == null) {
                nodeGraphManager = new NodeGraphManager();

                flowChart = nodeGraphManager.CreateFlowChart(false, Guid.NewGuid(), typeof(FlowChart));
                ViewModel.FlowChartViewModel = flowChart.ViewModel;

                // Generate context menu's using contextual information like current selected node
                nodeGraphManager.BuildFlowChartContextMenu += NodeGraphManager_BuildFlowChartContextMenu;
                nodeGraphManager.BuildNodeContextMenu += NodeGraphManager_BuildNodeContextMenu;
                nodeGraphManager.BuildFlowPortContextMenu += NodeGraphManager_BuildFlowPortContextMenu;
                nodeGraphManager.BuildPropertyPortContextMenu += NodeGraphManager_BuildPropertyPortContextMenu;

                // Event for when ever a node is destroyed
                nodeGraphManager.NodesDestroyed += (_sender) => { };
            }
        }

        #region Contextual Menu Generation

        private Point _ContextMenuLocation;

        private bool NodeGraphManager_BuildFlowChartContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();
            foreach (var nodeType in CNodeList.NodeTypes.Keys) {
                MenuItem menuItem = new MenuItem();

                var NodeAttrs = nodeType.GetCustomAttributes(typeof(NodeAttribute), false) as NodeAttribute[];
                if (1 != NodeAttrs.Length)
                    throw new ArgumentException(string.Format("{0} must have NodeAttribute", nodeType.Name));

                var NodeDescriptor = nodeType.GetCustomAttributes(typeof(CNodeDescriptor), false) as CNodeDescriptor[];

                if (NodeDescriptor.Length > 0)
                    menuItem.Header = "Create " + NodeDescriptor[0].Title;
                else menuItem.Header = "Create " + nodeType.Name;

                menuItem.CommandParameter = nodeType;
                menuItem.Click += FlowChart_ContextMenuItem_Click;

                items.Add(menuItem);
            }

            return (0 < items.Count);
        }

        private void FlowChart_ContextMenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem menuItem = sender as MenuItem;
            Type nodeType = menuItem.CommandParameter as Type;

            NodeGraph.View.FlowChartView flowChartView = ViewModel.FlowChartViewModel.View;

            var node = nodeGraphManager.CreateNode(
                false, Guid.NewGuid(), ViewModel.FlowChartViewModel.Model, nodeType, _ContextMenuLocation.X, _ContextMenuLocation.Y, 0);

            node.InputFlowPorts.CollectionChanged += NodePortsChanged_Event;
            node.OutputFlowPorts.CollectionChanged += NodePortsChanged_Event;
            node.InputPropertyPorts.CollectionChanged += NodePortsChanged_Event;
            node.OutputPropertyPorts.CollectionChanged += NodePortsChanged_Event;

            if (node is CNodeBase)
                (node as CNodeBase).StateChanged += CNodeBase_NodeStateChanged;

            compileScript();
        }

   

        private bool NodeGraphManager_BuildNodeContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();

            foreach (var nodeType in CNodeList.NodeTypes.Keys) {
                MenuItem menuItem = new MenuItem();

                var NodeAttrs = nodeType.GetCustomAttributes(typeof(NodeAttribute), false) as NodeAttribute[];
                if (1 != NodeAttrs.Length)
                    throw new ArgumentException(string.Format("{0} must have NodeAttribute", nodeType.Name));

                var NodeDescriptor = nodeType.GetCustomAttributes(typeof(CNodeDescriptor), false) as CNodeDescriptor[];

                if (NodeDescriptor.Length > 0)
                    menuItem.Header = "Create " + NodeDescriptor[0].Title;
                else menuItem.Header = "Create " + nodeType.Name;

                menuItem.CommandParameter = nodeType;
                menuItem.Click += FlowChart_ContextMenuItem_Click;

                items.Add(menuItem);
            }

            return (0 < items.Count);
        }

        private bool NodeGraphManager_BuildFlowPortContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();
            foreach (var kv in CNodeList.NodeTypes) {
                Type nodeType = kv.Key;
                var disp = kv.Value;
                if ((disp & CNodeList.NodeContextType.Flow) != CNodeList.NodeContextType.Flow)
                    continue;

                MenuItem menuItem = new MenuItem();

                var NodeAttrs = nodeType.GetCustomAttributes(typeof(NodeAttribute), false) as NodeAttribute[];
                if (1 != NodeAttrs.Length)
                    throw new ArgumentException(string.Format("{0} must have NodeAttribute", nodeType.Name));

                var NodeDescriptor = nodeType.GetCustomAttributes(typeof(CNodeDescriptor), false) as CNodeDescriptor[];

                if (NodeDescriptor.Length > 0)
                    menuItem.Header = "Create " + NodeDescriptor[0].Title;
                else menuItem.Header = "Create " + nodeType.Name;

                menuItem.CommandParameter = nodeType;
                menuItem.Click += FlowChart_ContextMenuItem_Click;

                items.Add(menuItem);
            }

            return (0 < items.Count);
        }

        private bool NodeGraphManager_BuildPropertyPortContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();
            foreach (var kv in CNodeList.NodeTypes) {
                Type nodeType = kv.Key;
                var disp = kv.Value;
                if ((disp & CNodeList.NodeContextType.Property) != CNodeList.NodeContextType.Property)
                    continue;

                MenuItem menuItem = new MenuItem();

                var NodeAttrs = nodeType.GetCustomAttributes(typeof(NodeAttribute), false) as NodeAttribute[];
                if (1 != NodeAttrs.Length)
                    throw new ArgumentException(string.Format("{0} must have NodeAttribute", nodeType.Name));

                var NodeDescriptor = nodeType.GetCustomAttributes(typeof(CNodeDescriptor), false) as CNodeDescriptor[];

                if (NodeDescriptor.Length > 0)
                     menuItem.Header = "Create " + NodeDescriptor[0].Title;
                else menuItem.Header = "Create " + nodeType.Name;

                menuItem.CommandParameter = nodeType;
                menuItem.Click += FlowChart_ContextMenuItem_Click;

                items.Add(menuItem);
            }

            return (0 < items.Count);
        }

        #endregion


        public virtual void NodeStateChanged(CBaseNode node) {
            // Do something when ever a node state is changed
            // this can be anything from new connection
            //   to variables being changed.
            // etc.
        }
    }
}
