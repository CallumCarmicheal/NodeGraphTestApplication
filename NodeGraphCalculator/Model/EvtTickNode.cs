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
	[NodeFlowPort( "Output", "", false)]
	public class EvtTickNode : CalculatorNodeBase
	{
		#region Fields

		[NodePropertyPort( "Delta", false, typeof( double ), 0.0, false )]
		public double Delta;

		#endregion // Fields

		#region Constructor

		public EvtTickNode( NodeGraphManager ngm, Guid guid, FlowChart flowChart ) : base( ngm, guid, flowChart, CalculatorNodeType.EvtTick )
		{
			Header = "Tick";
			HeaderBackgroundColor = Brushes.Maroon;
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
