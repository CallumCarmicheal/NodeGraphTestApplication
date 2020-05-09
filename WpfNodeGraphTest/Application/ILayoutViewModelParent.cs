using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.Application {
    public interface ILayoutViewModelParent {
        bool IsBusy { get; set; }
        string DirAppData { get; }
    }
}
