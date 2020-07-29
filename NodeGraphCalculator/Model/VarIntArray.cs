using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphCalculator.Model
{
	[Node()]
	public class VarIntArrayNode : ArrayVariableNode<int>
	{
		#region Constructor

		public VarIntArrayNode( Guid guid, FlowChart flowChart ) : base( guid, flowChart, CalculatorNodeType.VarInt )
		{

		}

		#endregion // Constructor
	}
}
