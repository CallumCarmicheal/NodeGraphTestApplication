using NodeGraph;
using NodeGraph.Model;
using NodeGraph.View;
using NodeGraphCalculator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NodeGraphCalculator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields

		private DispatcherTimer _Ticker = new DispatcherTimer();

		#endregion // Fields

		#region Properties

		public NodeGraph.ViewModel.FlowChartViewModel FlowChartViewModel
		{
			get { return ( NodeGraph.ViewModel.FlowChartViewModel )GetValue( FlowChartViewModelProperty ); }
			set { SetValue( FlowChartViewModelProperty, value ); }
		}
		public static readonly DependencyProperty FlowChartViewModelProperty =
			DependencyProperty.Register( "FlowChartViewModel", typeof( NodeGraph.ViewModel.FlowChartViewModel ), typeof( MainWindow ), new PropertyMetadata( null ) );

		#endregion // Properties

		#region Constructor

		public MainWindow()
		{
			InitializeComponent();

			Loaded += MainWindow_Loaded;
			Unloaded += MainWindow_Unloaded;

			_Ticker.Interval = TimeSpan.FromMilliseconds( 1.0 );
			_Ticker.Tick += _Ticker_Tick;
			_Ticker.Start();
		}

		#endregion // Constructor

		#region Tick Events

		private void _Ticker_Tick( object sender, EventArgs e )
		{
			if( null == FlowChartViewModel )
				return;

			FlowChart flowChart = FlowChartViewModel.Model;

			NodeGraphManager.ClearScreenLogs( flowChart );
			NodeGraphManager.ClearNodeExecutionStates( flowChart );

			List< Node > nodes = NodeGraphManager.FindNode( FlowChartViewModel.Model, "Tick" );
			if( 0 == nodes.Count )
			{
				NodeGraphManager.AddScreenLog( flowChart, "You need to place a tick node." );
			}
			else if( 1 == nodes.Count )
			{
				EvtTickNode node = nodes[ 0 ] as EvtTickNode;

				node.Delta = 0.0;
				node.RaisePropertyChanged( "Delta" );

				node.OnPreExecute( null );
				node.OnExecute( null );
				node.OnPostExecute( null );
			}
			else
			{
				NodeGraphManager.AddScreenLog( flowChart, "You need to remove ticker nodes except one node." );
			}

		}

		#endregion // Tick Events

		#region ContextMenu

		private Point _ContextMenuLocation;

		private bool NodeGraphManager_BuildFlowChartContextMenu( object sender, BuildContextMenuArgs args )
		{
			ItemCollection items = args.ContextMenu.Items;
			_ContextMenuLocation = args.ModelSpaceMouseLocation;
			return ( 0 < items.Count );
		}

		private bool NodeGraphManager_BuildNodeContextMenu( object sender, BuildContextMenuArgs args )
		{
			ItemCollection items = args.ContextMenu.Items;
			return ( 0 < items.Count );
		}

		private bool NodeGraphManager_BuildFlowPortContextMenu( object sender, BuildContextMenuArgs args )
		{
			ItemCollection items = args.ContextMenu.Items;
			return ( 0 < items.Count );
		}

		private bool NodeGraphManager_BuildPropertyPortContextMenu( object sender, BuildContextMenuArgs args )
		{
			ItemCollection items = args.ContextMenu.Items;
			return ( 0 < items.Count );
		}

		protected virtual void FlowChart_ContextMenuItem_Click( object sender, RoutedEventArgs e )
		{
		}

		#endregion // ContextMenu

		#region Events

		private void MainWindow_Loaded( object sender, RoutedEventArgs e )
		{
			NodeGraphManager.OutputDebugInfo = true;
			NodeGraphManager.SelectionMode = NodeGraph.SelectionMode.Include;

			FlowChart flowChart = NodeGraphManager.CreateFlowChart( false, Guid.NewGuid(), typeof( FlowChart ) );
			FlowChartViewModel = flowChart.ViewModel;

			NodeGraphManager.BuildFlowChartContextMenu += NodeGraphManager_BuildFlowChartContextMenu;
			NodeGraphManager.BuildNodeContextMenu += NodeGraphManager_BuildNodeContextMenu;
			NodeGraphManager.BuildFlowPortContextMenu += NodeGraphManager_BuildFlowPortContextMenu;
			NodeGraphManager.BuildPropertyPortContextMenu += NodeGraphManager_BuildPropertyPortContextMenu;

			NodeGraphManager.NodeSelectionChanged += NodeGraphManager_NodeSelectionChanged;

			NodeGraphManager.DragEnter += NodeGraphManager_DragEnter;
			NodeGraphManager.DragLeave += NodeGraphManager_DragLeave;
			NodeGraphManager.DragOver += NodeGraphManager_DragOver;
			NodeGraphManager.Drop += NodeGraphManager_Drop;

			KeyDown += MainWindow_KeyDown;
		}

		private void MainWindow_Unloaded( object sender, RoutedEventArgs e )
		{
			Guid flowChartGuid = FlowChartViewModel.Model.Guid;
			FlowChartViewModel = null;
			NodeGraphManager.DestroyFlowChart( flowChartGuid );
		}

		#endregion // Events

		#region Keyboard Events

		private void MainWindow_KeyDown( object sender, KeyEventArgs e )
		{
			if( Keyboard.IsKeyDown( Key.LeftCtrl ) )
			{
				if( Key.S == e.Key )
				{
					NodeGraphManager.Serialize( @"SerializationTest.xml" );
				}
				else if( Key.O == e.Key )
				{
					Guid flowChartGuid = FlowChartViewModel.Model.Guid;
					FlowChartViewModel = null;
					NodeGraph.NodeGraphManager.DestroyFlowChart( flowChartGuid );

					if( NodeGraphManager.Deserialize( @"SerializationTest.xml" ) )
					{
						FlowChart flowChart = NodeGraphManager.FlowCharts.First().Value;
						FlowChartViewModel = flowChart.ViewModel;
						FlowChartViewModel.View.ZoomAndPan.StartX = 0.0;
						FlowChartViewModel.View.ZoomAndPan.StartY = 0.0;
						FlowChartViewModel.View.ZoomAndPan.Scale = 1.0;
					}
					else
					{
						FlowChart flowChart = NodeGraphManager.CreateFlowChart( false, Guid.NewGuid(), typeof( FlowChart ) );
						FlowChartViewModel = flowChart.ViewModel;
					}
				}
			}
		}

		#endregion // Keyboard Events

		#region Selection Events

		private void NodeGraphManager_NodeSelectionChanged( FlowChart flowChart, ObservableCollection<Guid> nodes, NotifyCollectionChangedEventArgs args )
		{

		}

		#endregion // Selection Events

		#region Drag & Drop Events

		Type[] _NodeTypes = new Type[]
		{
			typeof( VarIntNode ),
			typeof( VarIntArrayNode ),
			typeof( EvtTickNode ),
			typeof( OpPlusNode ),
			typeof( OpMinusNode ),
			typeof( OpMultiplyNode ),
			typeof( OpDivideNode ),
			typeof( OpMakeArrayNode ),
			typeof( OpForEachNode ),
			typeof( OpPrintNode ),
		};

		private void NodeGraphManager_Drop( object sender, NodeGraphDragEventArgs args )
		{
			FlowChartView flowChartView = FlowChartViewModel.View;

			CalculatorNodeType eType = ( CalculatorNodeType )args.DragEventArgs.Data.GetData( typeof( CalculatorNodeType ) );
			Type nodeType = _NodeTypes[ ( int )eType ];

			FlowChart flowChart = flowChartView.ViewModel.Model;
			flowChart.History.BeginTransaction( "Creating node" );
			{
				Node node = NodeGraphManager.CreateNode(
					false, Guid.NewGuid(), FlowChartViewModel.Model, nodeType,
					args.ModelSpaceMouseLocation.X, args.ModelSpaceMouseLocation.Y, 0 );
			}
			flowChart.History.EndTransaction( false );
		}

		private void NodeGraphManager_DragOver( object sender, NodeGraphDragEventArgs args )
		{
			
		}

		private void NodeGraphManager_DragLeave( object sender, NodeGraphDragEventArgs args )
		{
			
		}

		private void NodeGraphManager_DragEnter( object sender, NodeGraphDragEventArgs args )
		{
			
		}

		#endregion // Drag & Drop Events
	}
}
