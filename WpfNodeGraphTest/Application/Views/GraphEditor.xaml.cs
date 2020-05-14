using NodeGraph;
using NodeGraph.Model;

using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using WpfNodeGraphTest.NGraph;

namespace WpfNodeGraphTest.Application.Views {

    /// <summary>
    /// Interaction logic for GraphEditor.xaml
    /// </summary>
    public partial class GraphEditor {
        public GraphEditorViewModel ViewModel { get => (GraphEditorViewModel)DataContext; set => DataContext = value; }

        public GraphEditor() {
            ViewModel = new GraphEditorViewModel();

            InitializeComponent();
        }

        public NodeGraphManager NodeGraphManager { get; private set; }
        public FlowChart FlowChart { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (NodeGraphManager == null) {
                NodeGraphManager = new NodeGraphManager();

                FlowChart = NodeGraphManager.CreateFlowChart(false, Guid.NewGuid(), typeof(FlowChart));
                ViewModel.FlowChartViewModel = FlowChart.ViewModel;

                NodeGraphManager.BuildFlowChartContextMenu += NodeGraphManager_BuildFlowChartContextMenu;
                NodeGraphManager.BuildNodeContextMenu += NodeGraphManager_BuildNodeContextMenu;
                NodeGraphManager.BuildFlowPortContextMenu += NodeGraphManager_BuildFlowPortContextMenu;
                NodeGraphManager.BuildPropertyPortContextMenu += NodeGraphManager_BuildPropertyPortContextMenu;

                NodeGraphManager.NodesDestroyed += NodeGraphManager_NodesDestroyed;
            } else {
                var x = 0;
            }
        }


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
            
            var node = NodeGraphManager.CreateNode(
                false, Guid.NewGuid(), ViewModel.FlowChartViewModel.Model, nodeType, _ContextMenuLocation.X, _ContextMenuLocation.Y, 0);

            node.InputFlowPorts.CollectionChanged += NodePortsChanged_Event;
            node.OutputFlowPorts.CollectionChanged += NodePortsChanged_Event;
            node.InputPropertyPorts.CollectionChanged += NodePortsChanged_Event;
            node.OutputPropertyPorts.CollectionChanged += NodePortsChanged_Event;

            if (node is CNodeBase)
                (node as CNodeBase).StateChanged += CNodeBase_NodeStateChanged;

            compileScript();
        }

        private void CNodeBase_NodeStateChanged(CNodeBase node) {
            compileScript();
        }

        private void NodePortsChanged_Event(object sender, NotifyCollectionChangedEventArgs e) {
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

        private void saveToXmlClicked(object sender, RoutedEventArgs e) {
            compileScript();
        }


        private void NodeGraphManager_NodesDestroyed(object sender) {
            compileScript();
        }

        private void compileScript() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var node in FlowChart.Nodes) {
                var nodeType = node.GetType();
                var nodeStart = nodeType.GetCustomAttributes(typeof(CNodeStart), false) as CNodeStart[];
                if (nodeStart.Length > 0) {
                    var n = node as CNodeBase;
                    sb.AppendLine(n.CompileAsJavascript() + "\n");
                }
            }

            var jsCode = sb.ToString();

            Jsbeautifier.Beautifier beautifier = new Jsbeautifier.Beautifier();
            beautifier.Opts.IndentWithTabs = true;
            beautifier.Opts.BraceStyle = Jsbeautifier.BraceStyle.EndExpand;

            avalonEditer.Text = beautifier.Beautify(jsCode);
        }
    }
}