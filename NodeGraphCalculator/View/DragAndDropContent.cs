using NodeGraphCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NodeGraphCalculator.View
{
	public class DragAndDropContent : ContentControl
	{
		#region Properties

		public CalculatorNodeType NodeType
		{
			get { return ( CalculatorNodeType )GetValue( NodeTypeProperty ); }
			set { SetValue( NodeTypeProperty, value ); }
		}
		public static readonly DependencyProperty NodeTypeProperty =
			DependencyProperty.Register( "NodeType", typeof( CalculatorNodeType ), typeof( DragAndDropContent ), new PropertyMetadata( CalculatorNodeType.Count ) );

		#endregion // Properties

		#region Template

		static DragAndDropContent()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof( DragAndDropContent ), new FrameworkPropertyMetadata( typeof( DragAndDropContent ) ) );
		}

		#endregion // Template

		#region Mouse Events

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			if( MouseButtonState.Pressed == e.LeftButton )
			{
				DragDrop.DoDragDrop( this, NodeType, DragDropEffects.All );
			}

		}

		#endregion // Mouse Events
	}
}
