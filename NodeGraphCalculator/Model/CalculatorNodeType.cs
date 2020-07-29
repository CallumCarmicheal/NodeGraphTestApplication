using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraphCalculator.Model
{
	public enum CalculatorNodeType
	{
		VarInt,
		VarIntArray,

		EvtTick,
		
		OpPlus,
		OpMinus,
		OpMultiply,
		OpDivide,
		OpMakeArray,
		OpForEach,
		OpPrint,

		Count,
	}
}
