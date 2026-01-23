using Assets.Scripts.Objects.Electrical;
using IC10_Extender;
using IC10_Extender.Operations;

namespace Code_Traps
{
    // tseq jump dev index fixpoint [error]
    public class TrapStackEqualAbsolute : TrapOpCode
    {
        public static readonly HelpString[] Args = { Jump, Device, Index, Fixpoint, Error };

        public TrapStackEqualAbsolute() : base("tseq") { }

        public override void Accept(int lineNumber, string[] source)
        {
            if (source.Length < 5 || source.Length > 6) throw new ProgrammableChipException(ProgrammableChipException.ICExceptionType.IncorrectArgumentCount, lineNumber);
        }

        public override Operation Create(ChipWrapper chip, int lineNumber, string[] source)
        {
            var rv = new DeviceTrapOperation(chip, lineNumber, source[1], false, source[2], source[3], Utils.Equal()).WithFixpoint(source[4]);
            return source.Length == 6 ? rv.WithError(source[5]) : rv;
        }

        public override HelpString[] Params()
        {
            return Args;
        }

        public override string Description()
        {
            return $"Watches for changes to the stack of {Device} at {Index} after every instruction is completed. If any change occurs, and the new value is equal to {Fixpoint} within the specified margin of {Error}, " +
                $"then trap checking will be paused, as if by invoking <color=yellow>tsuspend</color>, " +
                $"the line that would normally be executed next will be stored (internally), and program execution will immediately jump to the line represented by " +
                $"{Jump}. Once you have completed reacting to the change, you can either <color=yellow>texit</color> to resume trap checking and jump back to where execution was interrupted, " +
                $"or you can <color=yellow>tclear</color> to resume trap checking and resume execution from the current line instead. If you have other traps whose execution is no longer necessary, " +
                $"then, while trap execution is paused, you can have them update their latest state using <color=yellow>treset</color> and specifying the absolute line number that the trap is initialized on. If no trap exists on that line, " +
                $"then it will silently noop.";
        }
    }
}
