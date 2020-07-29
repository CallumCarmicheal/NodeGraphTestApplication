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
	[NodeFlowPort( "Input", "Exec", true )]
	[NodeFlowPort( "Output", "", false )]
	public class OpMakeArrayNode : CalculatorNodeBase
	{
		#region Constructor

		public OpMakeArrayNode( Guid guid, FlowChart flowChart ) : base( guid, flowChart, CalculatorNodeType.OpMakeArray )
		{
			Header = "MakeArray";
			HeaderBackgroundColor = Brushes.DarkBlue;
			AllowEditingHeader = false;
		}

		#endregion // Constructor

		#region Callbacks

		public override void OnCreate()
		{
			NodeGraphManager.CreateNodePropertyPort( false, Guid.NewGuid(), this, true, typeof( object ), null,
				"Objects", false, null, "Objects( multiple input )", true );

			NodeGraphManager.CreateNodePropertyPort( false, Guid.NewGuid(), this, false, typeof( ObservableCollection<object> ), new ObservableCollection<object>(),
				"Array", false, null, "Array" );

			base.OnCreate();
		}

		public override void OnPreExecute( Connector prevConnector )
		{
			base.OnPreExecute( prevConnector );
		}

		public override void OnExecute( Connector prevConnector )
		{
			base.OnExecute( prevConnector );

			NodePropertyPort portOutputArray = NodeGraphManager.FindNodePropertyPort( this, "Array" );
			ObservableCollection<object> array = portOutputArray.Value as ObservableCollection<object>;
			array.Clear();

			NodePropertyPort portObjects = NodeGraphManager.FindNodePropertyPort( this, "Objects" );
			List<NodePort> connectedPorts;
			NodeGraphManager.FindConnectedPorts( portObjects, out connectedPorts );

			foreach( var connectedPort in connectedPorts )
			{
				NodePropertyPort portObject = connectedPort as NodePropertyPort;
				array.Add( portObject.Value );
			}
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
