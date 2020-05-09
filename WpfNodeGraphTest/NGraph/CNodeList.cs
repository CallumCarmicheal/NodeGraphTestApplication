using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.NGraph {
    public class CNodeList {

        public static List<Type> NodeTypes = new List<Type>() {
            //
            typeof( NGraph.Models.Events.EventTickNode ),
            
            // Functions
            typeof( NGraph.Models.Functions.FnPrint ),

            //
            typeof( NGraph.Models.Variables.VarIntegerNode ),

            //
        };
    }

}
