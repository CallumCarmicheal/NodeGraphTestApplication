using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphCalculator.ViewModel {
    public class MainWindowViewModel : ViewModelBase {

        protected NodeGraph.ViewModel.FlowChartViewModel _FlowChartViewModel;
        public NodeGraph.ViewModel.FlowChartViewModel FlowChartViewModel {
            get { return _FlowChartViewModel; }
            set {
                if (value != null || value != _FlowChartViewModel) _FlowChartViewModel = value;
                OnPropertyChanged("FlowChartViewModel");
            }
        }

    }
}
