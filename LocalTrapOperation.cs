using IC10_Extender;
using IC10_Extender.Operations;
using IC10_Extender.Variables;
using System;

namespace Code_Traps
{
    public class LocalTrapOperation : Operation
    {
        protected Getter<int> IndexGetter { get; private set; }
        protected Getter<int> JumpGetter { get; private set; }
        protected Getter<double> FixpointGetter { get; private set; }
        protected Getter<double> ErrorGetter { get; private set; }

        protected bool relativeJump;
        protected readonly Func<double, double, double, double, bool> comparer;

        public LocalTrapOperation(ChipWrapper chip, int lineNumber, string jumpArg, bool relativeJump, string indexArg, Func<double, double, double, double, bool> comparer) : base(chip, lineNumber)
        {
            IndexGetter = TrapOpCode.Index.Bind(chip, lineNumber, indexArg);
            JumpGetter = relativeJump ? TrapOpCode.Offset.Bind(chip, lineNumber, jumpArg) : TrapOpCode.Jump.Bind(chip, lineNumber, jumpArg);
            this.relativeJump = relativeJump;
            this.comparer = comparer;
        }

        public LocalTrapOperation WithFixpoint(string fixpointArg)
        {
            FixpointGetter = TrapOpCode.Fixpoint.Bind(Chip, LineNumber, fixpointArg);
            return this;
        }

        public LocalTrapOperation WithError(string errorArg)
        {
            ErrorGetter = TrapOpCode.Error.Bind(Chip, LineNumber, errorArg);
            return this;
        }

        protected virtual Func<double> getProvider()
        {
            IndexGetter(out var index, true);
            return Chip.RegisterProvider(index);
        }

        public override int Execute(int index)
        {
            var fixpoint = 0.0;
            var error = Utils.DefaultError;
            FixpointGetter?.Invoke(out fixpoint, true);
            ErrorGetter?.Invoke(out error, true);
            JumpGetter(out var jump, true);

            var ctx = TrapContext.For(Chip);
            ctx.traps.Remove(LineNumber); //remove previous trap created by this line, if any
            ctx.traps[LineNumber] = new Trap(
                ctx, 
                Chip, 
                getProvider(), 
                comparer,
                fixpoint,
                error,
                jump + (relativeJump ? LineNumber : 0)
            );
            return index + 1;
        }
    }
}
