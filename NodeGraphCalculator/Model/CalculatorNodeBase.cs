using NodeGraph.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphCalculator.Model
{
	public class CalculatorNodeBase : Node
	{
		#region Properties

		private CalculatorNodeType _NodeType;
		public CalculatorNodeType NodeType
		{
			get { return _NodeType; }
			set
			{
				if( value != _NodeType )
				{
					_NodeType = value;
					RaisePropertyChanged( "NodeType" );
				}
			}
		}

		#endregion // Properties

		#region Constructor

		public CalculatorNodeBase( Guid guid, FlowChart flowChart, CalculatorNodeType nodeType ) : base( guid, flowChart )
		{
			_NodeType = nodeType;
		}

		#endregion // Constructor
	}
}
