using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlankApplication.Nodes {
    /// <summary>
    /// This file list all of the nodes to help with 
    /// the generation of the context menu.
    /// </summary>
    class CNodeList {


        [Flags]
        public enum NodeContextType {
            None,
            Property = 1 << 0,
            Flow = 1 << 1
        }

        /// <summary>
        /// Here we describe if a node has a flow and/or property port
        ///    This is used to generate contextual suggestions in the context menu
        /// </summary>
        public static Dictionary<Type, NodeContextType> NodeTypes = new Dictionary<Type, NodeContextType>() {
            { typeof( Variables.ConstIntNode ),         NodeContextType.Property },
            { typeof( Variables.ConstDoubleNode ),      NodeContextType.Property },
            { typeof( Variables.ConstFloatNode),        NodeContextType.Property },
            { typeof( Variables.ConstStringNode ),      NodeContextType.Property },

            //
            { typeof( Nodes.StartNode ),                NodeContextType.Flow },

        };
    }
}
