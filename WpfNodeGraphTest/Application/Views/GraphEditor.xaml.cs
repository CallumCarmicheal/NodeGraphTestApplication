using NodeGraph;
using NodeGraph.Model;

using System;
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
            } else {
                var x = 0;
            }
        }

        private Point _ContextMenuLocation;

        private bool NodeGraphManager_BuildFlowChartContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();
            foreach (var nodeType in CNodeList.NodeTypes) {
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
            
            NodeGraphManager.CreateNode(
                false, Guid.NewGuid(), ViewModel.FlowChartViewModel.Model, nodeType, _ContextMenuLocation.X, _ContextMenuLocation.Y, 0);
        }

        private bool NodeGraphManager_BuildNodeContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();

            foreach (var nodeType in CNodeList.NodeTypes) {
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
            return false;
        }

        private bool NodeGraphManager_BuildPropertyPortContextMenu(object sender, BuildContextMenuArgs args) {
            return false;
        }
    }
}