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
	public class OpPrintNode : CalculatorNodeBase
	{
		#region Constructor

		public OpPrintNode( Guid guid, FlowChart flowChart ) : base( guid, flowChart, CalculatorNodeType.OpPrint )
		{
			Header = "Print";
			HeaderBackgroundColor = Brushes.DarkBlue;
			AllowEditingHeader = false;
		}

		#endregion // Constructor

		#region Callbacks

		public override void OnCreate()
		{
			NodeGraphManager.CreateNodePropertyPort( false, Guid.NewGuid(), this, true, typeof( object ), null,
				"Objects", false, null, "Objects", true );

			base.OnCreate();
		}

		public override void OnPreExecute( Connector prevConnector )
		{
			base.OnPreExecute( prevConnector );
		}

		public override void OnExecute( Connector prevConnector )
		{
			base.OnExecute( prevConnector );

			NodePropertyPort portObject = NodeGraphManager.FindNodePropertyPort( this, "Objects" );

			List<NodePort> connectedPorts;
			NodeGraphManager.FindConnectedPorts( portObject, out connectedPorts );

			if( 0 < connectedPorts.Count )
			{
				foreach( var connectedPort in connectedPorts )
				{
					NodePropertyPort port = connectedPort as NodePropertyPort;
					string result = string.Empty;
					if( null != port.Value )
					{
						result = string.Format( "{0}", port.Value.ToString() );
						NodeGraphManager.AddScreenLog( Owner, result );
					}
				}
			}
			else
			{
				ExecutionState = NodeExecutionState.Failed;
			}
		}

		public override void OnPostExecute( Connector prevConnector )
		{
			if( NodeExecutionState.Failed == ExecutionState )
			{
				return;
			}

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
