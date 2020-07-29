using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphCalculator.Model
{
	[Node()]
	public class VarIntNode : VariableNode<int>
	{
		#region Constructor

		public VarIntNode( Guid guid, FlowChart flowChart ) : base( guid, flowChart, CalculatorNodeType.VarInt )
		{
			
		}

		#endregion // Constructor
	}
}
