using IC10_Extender;
using IC10_Extender.Operations;
using System.Collections.Generic;

namespace Code_Traps
{
    public class TrapContext
    {
        private static readonly Dictionary<ChipWrapper, TrapContext> contexts = new Dictionary<ChipWrapper, TrapContext>();

        protected readonly ChipWrapper chip;

        public bool suspended { get; protected set; } = false;
        public int returnIndex { get; protected set; } = -1;

        public readonly Dictionary<int, Trap> traps = new Dictionary<int, Trap>();

        private TrapContext(ChipWrapper chip)
        {
            this.chip = chip;
            chip.PostExecute += EvalTraps;
            chip.OnDelete += (c) => { Remove(c); };
        }

        public void Suspend()
        {
            if (suspended || returnIndex != -1) return; //no-op if already suspended
            suspended = true;
        }

        public void Enter(int returnIndex)
        {
            if (suspended || returnIndex != -1) return; //TODO throw an error?
            suspended = true;
            this.returnIndex = returnIndex;
        }

        public int Exit(int nextIndex)
        {
            if (!suspended || returnIndex == -1)
            {
                return nextIndex+1; //no-op and proceed to next instruction if not suspended
            }
            suspended = false;
            var rv = returnIndex;
            returnIndex = -1;
            return rv;
        }

        public void Clear()
        {
            if (!suspended || returnIndex == -1) return; //no-op if already clear
            suspended = false;
            returnIndex = -1;
        }

        private void EvalTraps(OpContext op, ref int nextIndex)
        {
            var e = traps.Values.GetEnumerator();

            while (!suspended && e.MoveNext())
            {
                e.Current.Eval(ref nextIndex);
            }
        }

        public static TrapContext For(ChipWrapper chip)
        {
            if (!contexts.TryGetValue(chip, out var context))
            {
                context = new TrapContext(chip);
                contexts[chip] = context;
            }
            return context;
        }

        public static void Remove(ChipWrapper chip)
        {
            if (contexts.TryGetValue(chip, out var context))
            {
                chip.PostExecute -= context.EvalTraps;
                contexts.Remove(chip);
            }
        }
    }
}
