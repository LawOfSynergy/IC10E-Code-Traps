using Assets.Scripts.Objects.Pipes;
using IC10_Extender;
using IC10_Extender.Variables;
using System;

namespace Code_Traps
{
    public class DeviceTrapOperation : LocalTrapOperation
    {
        protected Getter<IMemoryReadable> ReadableGetter { get; private set; }

        public DeviceTrapOperation(ChipWrapper chip, int lineNumber, string jumpArg, bool relativeJump, string deviceArg, string indexArg, Func<double, double, double, double, bool> comparer) : base(chip, lineNumber, jumpArg, relativeJump, indexArg, comparer)
        {
            ReadableGetter = TrapOpCode.Device.Build(new Binding(chip, lineNumber, deviceArg));
        }

        protected override Func<double> getProvider()
        {
            ReadableGetter(out var readable, true);
            IndexGetter(out var index, true);

            return readable.StackProvider(index);
        }
    }
}
