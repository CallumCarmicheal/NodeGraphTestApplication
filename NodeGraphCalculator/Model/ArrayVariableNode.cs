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
	public class ArrayVariableNode<T> : CalculatorNodeBase
	{
		#region Propertiets

		private ObservableCollection<T> _Array = new ObservableCollection<T>();
		public ObservableCollection<T> Array
		{
			get { return _Array; }
			set
			{
				_Array = value;
				RaisePropertyChanged( "Array" );
			}
		}

		#endregion // Properites

		#region Constructor

		public ArrayVariableNode( NodeGraphManager ngm, Guid guid, FlowChart flowChart, CalculatorNodeType nodeType ) : base( ngm, guid, flowChart, nodeType )
		{
			Header = "Array " + typeof( T ).Name;
			HeaderBackgroundColor = Brushes.Black;
			HeaderFontColor = Brushes.White;
			AllowCircularConnection = false;

			_Array.CollectionChanged += _Array_CollectionChanged;
		}

		#endregion // Constructor

		#region Events

		public override void OnCreate()
		{
			NodePropertyPort port = NodeGraphManager.CreateNodePropertyPort(
				false, Guid.NewGuid(), this, false, 
				typeof( ObservableCollection<T> ), Array, "Array", true );
			port.DynamicPropertyPortValueChanged += ArrayPort_DynamicPropertyPortValueChanged;

			base.OnCreate();
		}

		private void ArrayPort_DynamicPropertyPortValueChanged( NodePropertyPort port, object prevValue, object newValue )
		{
			Array = port.Value as ObservableCollection<T>;
			Array.CollectionChanged += _Array_CollectionChanged;
		}

		private void _Array_CollectionChanged( object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e )
		{
			RaisePropertyChanged( "Array" );
		}

		#endregion // Events
	}
}
