using Assets.Scripts.Objects.Electrical;
using IC10_Extender;
using IC10_Extender.Operations;

namespace Code_Traps
{
    // texit
    public class TrapExit : ExtendedOpCode
    {
        public TrapExit() : base("texit") { }
        public override void Accept(int lineNumber, string[] source)
        {
            if (source.Length != 1) throw new ProgrammableChipException(ProgrammableChipException.ICExceptionType.IncorrectArgumentCount, lineNumber);
        }
        public override Operation Create(ChipWrapper chip, int lineNumber, string[] source)
        {
            return new ExitTrapOperation(chip, lineNumber);
        }
        public override HelpString[] Params()
        {
            return new HelpString[] { };
        }
        public override string Description()
        {
            return $"Resumes trap evaluation on this chip after a <color=yellow>tsuspend</color> or trap trigger, jumping back to the line where execution was interrupted. " +
                $"If no <color=yellow>tsuspend</color> or trap is active, this command will noop and proceed to the next instruction.";
        }
        public class ExitTrapOperation : Operation
        {
            public ExitTrapOperation(ChipWrapper chip, int lineNumber) : base(chip, lineNumber) { }
            public override int Execute(int index)
            {
                return TrapContext.For(Chip).Exit(index);
            }
        }
    }
}
