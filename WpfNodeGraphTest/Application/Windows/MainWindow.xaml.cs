using NodeGraph;
using System;
using System.Linq;
using System.Windows;
using WpfNodeGraphTest.Application.Views;
using Xceed.Wpf.AvalonDock.Layout;

namespace WpfNodeGraphTest.Application.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private AnchorableShowStrategy AnchorStrat = AnchorableShowStrategy.Top;

        private void makeDocument_Click(object sender, RoutedEventArgs e) {
            //LayoutAnchorable la = new LayoutAnchorable { Title = "Graph Editor", FloatingHeight = 400, FloatingWidth = 500, Content = new GraphEditor() };
            //la.AddToLayout(dockingManager, AnchorStrat);

            LayoutDocument ld = new LayoutDocument { Title = "Graph Editor", CanClose = true, Content = new GraphEditor() };
            _layoutDocumentPane.Children.Add(ld);

            //la.Float();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _anchorStrat.ItemsSource = Enum.GetValues(typeof(AnchorableShowStrategy)).Cast<AnchorableShowStrategy>();
            _anchorStrat.SelectedValue = AnchorableShowStrategy.Top;
        }

        private void newAnchoredGraph_Click(object sender, RoutedEventArgs e) {
            LayoutAnchorable la = new LayoutAnchorable { Title = "Graph Editor: Anchored", FloatingHeight = 400, FloatingWidth = 500, Content = new GraphEditor(), CanClose = true };
            la.AddToLayout(dockingManager, AnchorStrat);
        }

        private void newFLoatingGraph_Click(object sender, RoutedEventArgs e) {
            LayoutAnchorable la = new LayoutAnchorable { Title = "Graph Editor: Floating", FloatingHeight = 400, FloatingWidth = 500, Content = new GraphEditor(), CanClose = true };
            la.AddToLayout(dockingManager, AnchorStrat);
            la.Float();
        }

        private void current_Click(object sender, RoutedEventArgs e) {
            if (dockingManager.ActiveContent != null && dockingManager.ActiveContent is GraphEditor) {
                var graph = dockingManager.ActiveContent as GraphEditor;

#if (Debug_OldBugTesting)
                NodeGraphManager.AddScreenLog(graph.FlowChart, "Hello world!");
#else
                graph.nodeGraphManager.AddScreenLog(graph.FlowChart, $"Found + {graph.ViewModel.FlowChartViewModel.InstanceGuid}!", 2000);
#endif
            }
        }

        private void makeGraphEditorProp_Click(object sender, RoutedEventArgs e) {
            //LayoutAnchorable la = new LayoutAnchorable { Title = "Graph Editor Property Dependancy: Floating", 
            //    FloatingHeight = 400, FloatingWidth = 500, Content = new GraphEditorPropDepen(), CanClose = true };
            //la.AddToLayout(dockingManager, AnchorStrat);
            //la.Float();
        }
    }
}