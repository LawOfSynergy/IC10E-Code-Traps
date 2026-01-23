using Assets.Scripts.Objects.Electrical;
using IC10_Extender;
using IC10_Extender.Operations;

namespace Code_Traps
{
    // tsuspend
    public class TrapSuspend : ExtendedOpCode
    {
        public TrapSuspend() : base("tsuspend") { }
        public override void Accept(int lineNumber, string[] source)
        {
            if (source.Length != 1) throw new ProgrammableChipException(ProgrammableChipException.ICExceptionType.IncorrectArgumentCount, lineNumber);
        }
        public override Operation Create(ChipWrapper chip, int lineNumber, string[] source)
        {
            return new SuspendTrapOperation(chip, lineNumber);
        }
        public override HelpString[] Params()
        {
            return new HelpString[] { };
        }
        public override string Description()
        {
            return $"Pauses all trap evaluation on this chip until a subsequent <color=yellow>tclear</color> command. <color=yellow>texit</color> is not valid to exit since there is no return address to jump back to. " +
                $"This is useful if you need to update multiple register/stack addresses atomically before a trap is executed, or if you need to <color=yellow>treset</color> (skip) a trap after a change before it can trigger.";
        }

        public class SuspendTrapOperation : Operation
        {
            public SuspendTrapOperation(ChipWrapper chip, int lineNumber) : base(chip, lineNumber) { }
            public override int Execute(int index)
            {
                TrapContext.For(Chip).Suspend();
                return index + 1;
            }
        }
    }
}
