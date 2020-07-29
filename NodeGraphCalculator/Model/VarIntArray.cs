using NodeGraph;
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

		public VarIntArrayNode( NodeGraphManager ngm, Guid guid, FlowChart flowChart ) : base( ngm, guid, flowChart, CalculatorNodeType.VarInt )
		{

		}

		#endregion // Constructor
	}
}
