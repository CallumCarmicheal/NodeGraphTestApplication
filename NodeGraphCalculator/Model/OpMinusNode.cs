using NodeGraph;
using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NodeGraphCalculator.Model
{
	[Node()]
	[NodeFlowPort( "Input", "", true )]
	[NodeFlowPort( "Output", "", false )]
	public class OpMinusNode : CalculatorNodeBase
	{
		#region Fields

		[NodePropertyPort( "A", true, typeof( int ), 0, true )]
		public int A;

		[NodePropertyPort( "B", true, typeof( int ), 0, true )]
		public int B;

		[NodePropertyPort( "A - B", false, typeof( int ), 0, false )]
		public int Result;

		#endregion // Fields

		#region Constructor

		public OpMinusNode( NodeGraphManager ngm, Guid guid, FlowChart flowChart ) : base( ngm, guid, flowChart, CalculatorNodeType.OpMinus )
		{
			Header = "Minus";
			HeaderBackgroundColor = Brushes.DarkBlue;
			AllowEditingHeader = false;
		}

		#endregion // Constructor

		#region Callbacks

		public override void OnPreExecute( Connector prevConnector )
		{
			base.OnPreExecute( prevConnector );
		}

		public override void OnExecute( Connector prevConnector )
		{
			base.OnExecute( prevConnector );

			NodePropertyPort portA = NodeGraphManager.FindNodePropertyPort( this, "A" );
			NodePropertyPort portB = NodeGraphManager.FindNodePropertyPort( this, "B" );

			List<NodePort> connectedPorts;
			NodeGraphManager.FindConnectedPorts( portA, out connectedPorts );
			NodePropertyPort otherPortA = ( 0 < connectedPorts.Count ) ? connectedPorts[ 0 ] as NodePropertyPort : null;
			int a = ( null != otherPortA ) ? ( int )otherPortA.Value : A;

			NodeGraphManager.FindConnectedPorts( portB, out connectedPorts );
			NodePropertyPort otherPortB = ( 0 < connectedPorts.Count ) ? connectedPorts[ 0 ] as NodePropertyPort : null;
			int b = ( null != otherPortB ) ? ( int )otherPortB.Value : B;

			Result = a - b;
			RaisePropertyChanged( "Result" );
		}

		public override void OnPostExecute( Connector prevConnector )
		{
			base.OnPostExecute( prevConnector );

			NodeFlowPort port = NodeGraphManager.FindNodeFlowPort( this, "Output" );
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
