using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNodeGraphTest.NGraph {
    public class CNodeList {

        [Flags]
        public enum NodeContextType {
            None, 
            Property = 1 << 0,
            Flow = 1 << 1
        }

        public static Dictionary<Type, NodeContextType> NodeTypes = new Dictionary<Type, NodeContextType>() {
            { typeof( NGraph.Models.Events.EventTickNode ), NodeContextType.None },
            //
            { typeof( NGraph.Models.Functions.FnPrintInteger ), NodeContextType.Flow | NodeContextType.Property },
            { typeof( NGraph.Models.Functions.FnPrintString ), NodeContextType.Flow | NodeContextType.Property },
            //
            { typeof( NGraph.Models.Operator.OpIntegerAdd ), NodeContextType.Property },
            { typeof( NGraph.Models.Operator.OpStringConcat ), NodeContextType.Property },
            //
            { typeof( NGraph.Models.Variables.VarConstantIntegerNode ), NodeContextType.Property },
            { typeof( NGraph.Models.Variables.VarConstantStringNode ), NodeContextType.Property },
        };

        // {
        //    //
        //    typeof( NGraph.Models.Events.EventTickNode ),
        //    // Functions
        //    typeof( NGraph.Models.Functions.FnPrint ),
        //    //
        //    typeof( NGraph.Models.Variables.VarConstantIntegerNode ),
        //    //
        //};
    }

}
