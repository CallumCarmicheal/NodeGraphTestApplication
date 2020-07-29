using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NodeGraphCalculator.Model
{
	[Node()]
	[NodeFlowPort( "Exec", "Exec", true )]
	[NodeFlowPort( "Completed", "Completed", false )]
	[NodeFlowPort( "LoopBody", "Loop Body", false )]
	public class OpForEachNode : CalculatorNodeBase
	{
		#region Constructor

		public OpForEachNode( NodeGraphManager ngm, Guid guid, FlowChart flowChart ) : base( ngm, guid, flowChart, CalculatorNodeType.OpForEach )
		{
			Header = "ForEach";
			HeaderBackgroundColor = Brushes.DarkBlue;
			AllowEditingHeader = false;
		}

		#endregion // Constructor

		#region Callbacks

		public override void OnCreate()
		{
			NodeGraphManager.CreateNodePropertyPort( false, Guid.NewGuid(), this, true, typeof( ObservableCollection<object> ), new ObservableCollection<object>(),
				"Array", false, null, "Array" );

			NodeGraphManager.CreateNodePropertyPort( false, Guid.NewGuid(), this, false, typeof( object ), null,
				"ArrayElement", false, null, "Array Element" );

			NodeGraphManager.CreateNodePropertyPort( false, Guid.NewGuid(), this, false, typeof( int ), -1,
				"ArrayIndex", false, null, "Array Index" );

			base.OnCreate();
		}

		public override void OnPreExecute( Connector prevConnector )
		{
			base.OnPreExecute( prevConnector );
		}

		public override void OnExecute( Connector prevConnector )
		{
			base.OnExecute( prevConnector );

			NodePropertyPort portInputArray = NodeGraphManager.FindNodePropertyPort( this, "Array" );
			NodePropertyPort portArrayElement = NodeGraphManager.FindNodePropertyPort( this, "ArrayElement" );
			NodePropertyPort portArrayIndex = NodeGraphManager.FindNodePropertyPort( this, "ArrayIndex" );
			NodeFlowPort loopBodyPort = NodeGraphManager.FindNodeFlowPort( this, "LoopBody" );

			List<NodePort> connectedPorts;
			NodeGraphManager.FindConnectedPorts( portInputArray, out connectedPorts );

			ObservableCollection<object> array = portInputArray.Value as ObservableCollection<object>;
			if( 1 == connectedPorts.Count )
			{
				NodePropertyPort portOutputArray = connectedPorts[ 0 ] as NodePropertyPort;
				array = portOutputArray.Value as ObservableCollection<object>;
			}

			Connector connector = ( 1 == loopBodyPort.Connectors.Count ) ? loopBodyPort.Connectors[ 0 ] : null;
			if( ( null != array ) && ( null != connector ) )
			{
				for( int i = 0; i < array.Count; ++i )
				{
					portArrayIndex.Value = i;
					portArrayElement.Value = array[ i ];

					connector.OnPreExecute();
					connector.OnExecute();
					connector.OnPostExecute();
				}
			}
		}

		public override void OnPostExecute( Connector prevConnector )
		{
			base.OnPostExecute( prevConnector );

			NodeFlowPort port = NodeGraphManager.FindNodeFlowPort( this, "Completed" );
			foreach( var connector in port.Connectors )
			{
				connector.OnPreExecute();
				connector.OnExecute();
				connector.OnPostExecute();
			}
		}

		#endregion // Callbacks
	}
}
