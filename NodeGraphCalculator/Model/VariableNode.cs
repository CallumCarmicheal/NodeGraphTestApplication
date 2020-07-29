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
	public class VariableNode<T> : CalculatorNodeBase
	{
		#region Propertiets

		private object _Value;
		public object Value
		{
			get { return _Value; }
			set
			{
				if( _Value != value )
				{
					_Value = value;
					RaisePropertyChanged( "Value" );
				}
			}
		}

		#endregion // Properites

		#region Constructor

		public VariableNode( Guid guid, FlowChart flowChart, CalculatorNodeType nodeType ) : base( guid, flowChart, nodeType )
		{
			Header = typeof( T ).Name;
			HeaderBackgroundColor = Brushes.Black;
			HeaderFontColor = Brushes.White;
			AllowCircularConnection = false;
		}

		#endregion // Constructor

		#region Events

		public override void OnCreate()
		{
			Type type = typeof( T );

			NodePropertyPort port = NodeGraphManager.CreateNodePropertyPort(
				false, Guid.NewGuid(), this, false, type, Activator.CreateInstance( type ), "Value", true, null, "" );

			port.DynamicPropertyPortValueChanged += ValuePort_PropertyPortValueChanged;

			base.OnCreate();
		}

		private void ValuePort_PropertyPortValueChanged( NodePropertyPort port, object prevValue, object newValue )
		{
			Value = ( T )port.Value;
		}

		#endregion // Events
	}
}
