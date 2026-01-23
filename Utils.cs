using Assets.Scripts.Objects.Pipes;
using IC10_Extender;
using System;

namespace Code_Traps
{
    public static class Utils
    {
        public const double DefaultError = double.Epsilon;

        public static Func<double> RegisterProvider(this ChipWrapper chip, int registerIndex)
        {
            return () => chip.Registers[registerIndex];
        }

        public static Func<double> StackProvider(this IMemoryReadable readable, int stackIndex)
        {
            return () => readable.ReadMemory(stackIndex);
        }

        public static Func<double, double, double, double, bool> Changed()
        {
            return (prev, curr, fixpoint, error) => prev != curr;
        }

        public static Func<double, double, double, double, bool> GreaterThan()
        {
            return (prev, curr, fixpoint, error) => curr > fixpoint;
        }

        public static Func<double, double, double, double, bool> LessThan()
        {
            return (prev, curr, fixpoint, error) => curr < fixpoint;
        }

        public static Func<double, double, double, double, bool> GreaterThanOrEqual()
        {
            return (prev, curr, fixpoint, error) => curr >= fixpoint;
        }

        public static Func<double, double, double, double, bool> LessThanOrEqual()
        {
            return (prev, curr, fixpoint, error) => curr <= fixpoint;
        }

        public static Func<double, double, double, double, bool> Equal()
        {
            return (prev, curr, fixpoint, error) => Math.Abs(curr - fixpoint) <= error;
        }

        public static Func<double, double, double, double, bool> NotEqual()
        {
            return (prev, curr, fixpoint, error) => Math.Abs(curr - fixpoint) > error;
        }
    }
}
