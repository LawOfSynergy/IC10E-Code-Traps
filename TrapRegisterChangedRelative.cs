using Assets.Scripts.Objects.Electrical;
using IC10_Extender;
using IC10_Extender.Operations;

namespace Code_Traps
{
    // trrc offset index
    public class TrapRegisterChangedRelative : TrapOpCode
    {
        public static readonly HelpString[] Args = { Offset, Index };

        public TrapRegisterChangedRelative() : base("trrc") { }

        public override void Accept(int lineNumber, string[] source)
        {
            if (source.Length != 3) throw new ProgrammableChipException(ProgrammableChipException.ICExceptionType.IncorrectArgumentCount, lineNumber);
        }

        public override Operation Create(ChipWrapper chip, int lineNumber, string[] source)
        {
            return new LocalTrapOperation(chip, lineNumber, source[1], true, source[2], Utils.Changed());
        }

        public override HelpString[] Params()
        {
            return Args;
        }

        public override string Description()
        {
            return $"Watches for changes to the register at {Index} after every instruction is completed. If any change occurs, then trap checking will be paused, as if by invoking <color=yellow>tsuspend</color> " +
                $"the line that would normally be executed next will be stored (internally), and program execution will immediately jump to the current line plus " +
                $"{Offset}. Once you have completed reacting to the change, you can either <color=yellow>texit</color> to resume trap checking and jump back to where execution was interrupted, " +
                $"or you can <color=yellow>tclear</color> to resume trap checking and resume execution from the current line instead. If you have other traps whose execution is no longer necessary, " +
                $"then, while trap execution is paused, you can have them update their latest state using <color=yellow>treset</color> and specifying the absolute line number that the trap is initialized on. If no trap exists on that line, " +
                $"then it will silently noop.";
        }
    }
}
