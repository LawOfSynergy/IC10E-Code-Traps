using IC10_Extender;
using System;

namespace Code_Traps
{
    public class Trap
    {
        protected readonly TrapContext context;
        protected readonly ChipWrapper chip;
        protected readonly Func<double> provider;
        protected readonly Func<double, double, double, double, bool> comparer;
        protected readonly double fixpoint;
        protected readonly double error;
        protected readonly int jumpTo;

        protected double previousValue;

        public Trap(TrapContext context, ChipWrapper chip, Func<double> provider, Func<double, double, double, double, bool> comparer, double fixpoint, double error, int jumpTo)
        {
            this.context = context;
            this.chip = chip;
            this.provider = provider;
            this.comparer = comparer;
            this.fixpoint = fixpoint;
            this.error = error;
            this.jumpTo = jumpTo;
            previousValue = provider();
        }

        public void Eval(ref int nextIndex)
        {
            if (context.Suspended) return;

            var val = provider();
            if (val != previousValue)
            {
                if (comparer(previousValue, val, fixpoint, error))
                {
                    context.Enter(nextIndex);
                    nextIndex = jumpTo;
                }
                previousValue = val;
            }
            
        }

        public void Reset() { previousValue = provider(); }
    }
}
