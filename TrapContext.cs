using IC10_Extender;
using IC10_Extender.Operations;
using System.Collections.Generic;

namespace Code_Traps
{
    public class TrapContext
    {
        private static readonly Dictionary<ChipWrapper, TrapContext> contexts = new Dictionary<ChipWrapper, TrapContext>();

        protected readonly ChipWrapper chip;

        public bool Suspended { get; protected set; } = false;
        public int ReturnIndex { get; protected set; } = -1;

        public readonly Dictionary<int, Trap> traps = new Dictionary<int, Trap>();

        private TrapContext(ChipWrapper chip)
        {
            this.chip = chip;
            chip.PostExecute += EvalTraps;
            chip.OnDelete += (c) => { Remove(c); };
            chip.OnReset += Reset;
        }

        public void Suspend()
        {
            Plugin.Log.LogInfo($"TrapContext: Suspending trap evaluation");
            if (Suspended) return; //no-op if already suspended
            Suspended = true;
            ReturnIndex = -1;
        }

        public void Enter(int returnIndex)
        {
            Plugin.Log.LogInfo($"TrapContext: Entering trap, returnIndex={returnIndex}");
            if (Suspended || ReturnIndex != -1) return; //TODO throw an error?
            Suspended = true;
            ReturnIndex = returnIndex;
        }

        public int Exit(int nextIndex)
        {
            Plugin.Log.LogInfo($"TrapContext: Exiting trap, Suspended={Suspended}, ReturnIndex={ReturnIndex}, nextIndex={nextIndex}");
            if (!Suspended || ReturnIndex == -1)
            {
                Plugin.Log.LogInfo($"TrapContext: Not valid, proceeding to next instruction. ");
                return nextIndex + 1; //no-op and proceed to next instruction if not suspended
            }
            Suspended = false;
            var rv = ReturnIndex;
            ReturnIndex = -1;
            return rv;
        }

        public void Clear()
        {
            Plugin.Log.LogInfo($"TrapContext: Clearing trap state");
            if (!Suspended) return; //no-op if already clear
            Suspended = false;
            ReturnIndex = -1;
        }

        private void EvalTraps(Operation op, ref int nextIndex)
        {
            var e = traps.Values.GetEnumerator();

            while (!Suspended && e.MoveNext())
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
                chip.OnReset -= Reset;
                contexts.Remove(chip);
            }
        }

        private static void Reset(ChipWrapper chip)
        {
            var ctx = For(chip);
            ctx.Suspended = false;
            ctx.ReturnIndex = -1;
            ctx.traps.Clear();
        }
    }
}
