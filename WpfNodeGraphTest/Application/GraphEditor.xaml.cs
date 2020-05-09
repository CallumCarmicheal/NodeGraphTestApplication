using NodeGraph;
using NodeGraph.Model;

using System;
using System.Windows;
using System.Windows.Controls;

using WpfNodeGraphTest.NGraph;

namespace WpfNodeGraphTest.Application {

    /// <summary>
    /// Interaction logic for GraphEditor.xaml
    /// </summary>
    public partial class GraphEditor {

        public GraphEditor() {
            InitializeComponent();
        }

        public NodeGraph.ViewModel.FlowChartViewModel FlowChartViewModel {
            get { return (NodeGraph.ViewModel.FlowChartViewModel)GetValue(FlowChartViewModelProperty); }
            set { SetValue(FlowChartViewModelProperty, value); }
        }

        public static readonly DependencyProperty FlowChartViewModelProperty =
            DependencyProperty.Register("FlowChartViewModel", typeof(NodeGraph.ViewModel.FlowChartViewModel),
            typeof(GraphEditor), new PropertyMetadata(null));

        private NodeGraphManager nodeGraphManager;
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (nodeGraphManager == null) {
                nodeGraphManager = new NodeGraphManager();

                FlowChart flowChart = nodeGraphManager.CreateFlowChart(false, Guid.NewGuid(), typeof(FlowChart));
                FlowChartViewModel = flowChart.ViewModel;

                nodeGraphManager.BuildFlowChartContextMenu += NodeGraphManager_BuildFlowChartContextMenu;
                nodeGraphManager.BuildNodeContextMenu += NodeGraphManager_BuildNodeContextMenu;
                nodeGraphManager.BuildFlowPortContextMenu += NodeGraphManager_BuildFlowPortContextMenu;
                nodeGraphManager.BuildPropertyPortContextMenu += NodeGraphManager_BuildPropertyPortContextMenu;
            } else {
                // todo some shit
                var x = 0;
            }
        }

        private Point _ContextMenuLocation;
        private bool NodeGraphManager_BuildFlowChartContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();
            foreach(var nodeType in CNodeList.NodeTypes) {
                MenuItem menuItem = new MenuItem();

                var NodeAttrs = nodeType.GetCustomAttributes(typeof(NodeAttribute), false) as NodeAttribute[];
                if(1 != NodeAttrs.Length)
                    throw new ArgumentException(string.Format("{0} must have NodeAttribute", nodeType.Name));

                var NodeDescriptor = nodeType.GetCustomAttributes(typeof(CNodeDescriptor), false) as CNodeDescriptor[];

                if(NodeDescriptor.Length > 0)
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

            NodeGraph.View.FlowChartView flowChartView = FlowChartViewModel.View;

            Point nodePos = flowChartView.ZoomAndPan.MatrixInv.Transform(
                new Point(_ContextMenuLocation.X, _ContextMenuLocation.Y));

            Node node = nodeGraphManager.CreateNode(
                false, Guid.NewGuid(), FlowChartViewModel.Model, nodeType, nodePos.X, nodePos.Y, 0);
        }

        private bool NodeGraphManager_BuildNodeContextMenu(object sender, BuildContextMenuArgs args) {
            ItemCollection items = args.ContextMenu.Items;
            _ContextMenuLocation = args.ModelSpaceMouseLocation;

            items.Clear();

            foreach(var nodeType in CNodeList.NodeTypes) {
                MenuItem menuItem = new MenuItem();

                var NodeAttrs = nodeType.GetCustomAttributes(typeof(NodeAttribute), false) as NodeAttribute[];
                if(1 != NodeAttrs.Length)
                    throw new ArgumentException(string.Format("{0} must have NodeAttribute", nodeType.Name));

                var NodeDescriptor = nodeType.GetCustomAttributes(typeof(CNodeDescriptor), false) as CNodeDescriptor[];

                if(NodeDescriptor.Length > 0)
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