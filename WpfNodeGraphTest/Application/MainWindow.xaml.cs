using System;
using System.Linq;
using System.Windows;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace WpfNodeGraphTest.Application {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        AnchorableShowStrategy AnchorStrat = AnchorableShowStrategy.Top;
        private void makeDocument_Click(object sender, RoutedEventArgs e) {
            //LayoutAnchorable la = new LayoutAnchorable { Title = "Graph Editor", FloatingHeight = 400, FloatingWidth = 500, Content = new GraphEditor() };
            //la.AddToLayout(dockingManager, AnchorStrat);

            LayoutDocument ld = new LayoutDocument { Title = "Graph Editor", CanClose = true, Content = new GraphEditor() };
            _layoutDocumentPane.Children.Add(ld);

            //la.Float();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _anchorStrat.ItemsSource = Enum.GetValues(typeof(AnchorableShowStrategy)).Cast<AnchorableShowStrategy>();
            _anchorStrat.SelectedIndex = 0;
        }

        private void newAnchoredGraph_Click(object sender, RoutedEventArgs e) {
            LayoutAnchorable la = new LayoutAnchorable { Title = "Graph Editor: Anchored", FloatingHeight = 400, FloatingWidth = 500, Content = new GraphEditor(), CanClose = true };
            la.AddToLayout(dockingManager, AnchorStrat);
        }

        private void newFLoatingGraph_Click(object sender, RoutedEventArgs e) {
            LayoutAnchorable la = new LayoutAnchorable { Title = "Graph Editor: Floating", FloatingHeight = 400, FloatingWidth = 500, Content = new GraphEditor(), CanClose=true };
            la.AddToLayout(dockingManager, AnchorStrat);
            la.Float();
        }
    }
}