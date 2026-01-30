using Assets.Scripts.Objects.Electrical;
using IC10_Extender;
using IC10_Extender.Operations;
using IC10_Extender.Variables;
using IC10_Extender.Wrappers;
using IC10_Extender.Wrappers.Variable;
using System.Collections.Generic;

namespace Code_Traps
{
    // treset <line number of trap>
    public class TrapResetRelative : ExtendedOpCode
    {
        public static readonly Variable<int> Line = Variables.Int.Named("lineOffset");
        public TrapResetRelative() : base("tresetr") { }
        public override void Accept(int lineNumber, string[] source)
        {
            if (source.Length < 2) throw new ProgrammableChipException(ProgrammableChipException.ICExceptionType.IncorrectArgumentCount, lineNumber);
        }
        public override Operation Create(ChipWrapper chip, int lineNumber, string[] source)
        {
            return new TrapResetOperation(chip, lineNumber, source);
        }
        public override HelpString[] Params()
        {
            return new HelpString[] { Line };
        }

        public override HelpString[] VarArgParams()
        {
            return new HelpString[] { Line };
        }

        public override string Description()
        {
            return $"Updates the internal state of the trap(s) initialized at the specified line offset(s) to its current observed value, effectively skipping any pending trigger. " +
                $"Labels cannot be used here, since they cannot share the same line as a trigger. " +
                $"If no trap exists on that line, this command will noop for that line.";
        }
        public class TrapResetOperation : Operation
        {
            private readonly List<Getter<int>> LineGetters = new List<Getter<int>>();

            public TrapResetOperation(ChipWrapper chip, int lineNumber, string[] src) : base(chip, lineNumber)
            {
                for (int i = 1; i < src.Length; i++)
                {
                    LineGetters.Add(Line.Build(new Binding(chip, lineNumber, src[i])));
                }
            }
            public override int Execute(int index)
            {
                var ctx = TrapContext.For(Chip);
                for (int i = 0; i < LineGetters.Count; i++)
                {
                    LineGetters[i](out var line, true);
                    ctx.traps.TryGetValue(LineNumber + line, out var trap);
                    trap?.Reset();
                }
                return index + 1;
            }
        }
    }
}
