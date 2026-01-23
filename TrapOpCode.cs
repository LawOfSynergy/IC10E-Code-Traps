using Assets.Scripts.Objects.Pipes;
using IC10_Extender;
using IC10_Extender.Operations;
using IC10_Extender.Variables;
using IC10_Extender.Wrappers.Variable;
using static IC10_Extender.HelpString;

namespace Code_Traps
{
    public abstract class TrapOpCode : ExtendedOpCode
    {
        public static readonly Variable<IMemoryReadable> Device = Variables.Readable.Named("device");
        public static readonly Variable<int> Jump = Variables.LineNumber.Named("jump");
        public static readonly Variable<int> Offset = Variables.Int.Named("offset");
        public static readonly Variable<int> Index = Variables.Int.Named("index");
        public static readonly Variable<double> Fixpoint = Variables.Double.Named("fixpoint");
        public static readonly Variable<double> Error = Variables.Double.Named("error").Optional();

        public TrapOpCode(string opcode) : base(opcode) { }
    }
}
