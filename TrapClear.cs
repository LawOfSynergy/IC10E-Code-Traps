using Assets.Scripts.Objects.Electrical;
using IC10_Extender;
using IC10_Extender.Operations;

namespace Code_Traps
{
    // tclear
    public class TrapClear : ExtendedOpCode
    {
        public TrapClear() : base("tclear") { }
        public override void Accept(int lineNumber, string[] source)
        {
            if (source.Length != 1) throw new ProgrammableChipException(ProgrammableChipException.ICExceptionType.IncorrectArgumentCount, lineNumber);
        }
        public override Operation Create(ChipWrapper chip, int lineNumber, string[] source)
        {
            return new ClearTrapOperation(chip, lineNumber);
        }
        public override HelpString[] Params()
        {
            return new HelpString[] { };
        }
        public override string Description()
        {
            return $"Resumes trap evaluation on this chip after a <color=yellow>tsuspend</color> or trap trigger, continuing execution from the current line. " +
                $"If no <color=yellow>tsuspend</color> or trap is active, this command will noop.";
        }
        public class ClearTrapOperation : Operation
        {
            public ClearTrapOperation(ChipWrapper chip, int lineNumber) : base(chip, lineNumber) { }
            public override int Execute(int index)
            {
                TrapContext.For(Chip).Clear();
                return index + 1;
            }
        }
    }
}
