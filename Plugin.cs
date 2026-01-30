using BepInEx;
using BepInEx.Logging;
using IC10_Extender;

namespace Code_Traps
{
    [BepInPlugin("net.lawofsynergy.stationeers.trap", "[IC10E] Code Traps", "0.0.0.1")]
    [BepInDependency("net.lawofsynergy.stationeers.ic10e")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        void Awake()
        {
            Log = Logger;

            IC10Extender.Register(new TrapRegisterChangedAbsolute());
            IC10Extender.Register(new TrapRegisterChangedRelative());
            IC10Extender.Register(new TrapStackChangedAbsolute());
            IC10Extender.Register(new TrapStackChangedRelative());
            IC10Extender.Register(new TrapRegisterGreaterThanAbsolute());
            IC10Extender.Register(new TrapRegisterGreaterThanRelative());
            IC10Extender.Register(new TrapStackGreaterThanAbsolute());
            IC10Extender.Register(new TrapStackGreaterThanRelative());
            IC10Extender.Register(new TrapRegisterLessThanAbsolute());
            IC10Extender.Register(new TrapRegisterLessThanRelative());
            IC10Extender.Register(new TrapStackLessThanAbsolute());
            IC10Extender.Register(new TrapStackLessThanRelative());
            IC10Extender.Register(new TrapRegisterGreaterThanOrEqualAbsolute());
            IC10Extender.Register(new TrapRegisterGreaterThanOrEqualRelative());
            IC10Extender.Register(new TrapStackGreaterThanOrEqualAbsolute());
            IC10Extender.Register(new TrapStackGreaterThanOrEqualRelative());
            IC10Extender.Register(new TrapRegisterLessThanOrEqualAbsolute());
            IC10Extender.Register(new TrapRegisterLessThanOrEqualRelative());
            IC10Extender.Register(new TrapStackLessThanOrEqualAbsolute());
            IC10Extender.Register(new TrapStackLessThanOrEqualRelative());
            IC10Extender.Register(new TrapRegisterEqualAbsolute());
            IC10Extender.Register(new TrapRegisterEqualRelative());
            IC10Extender.Register(new TrapStackEqualAbsolute());
            IC10Extender.Register(new TrapStackEqualRelative());
            IC10Extender.Register(new TrapRegisterNotEqualAbsolute());
            IC10Extender.Register(new TrapRegisterNotEqualRelative());
            IC10Extender.Register(new TrapStackNotEqualAbsolute());
            IC10Extender.Register(new TrapStackNotEqualRelative());
            IC10Extender.Register(new TrapSuspend());
            IC10Extender.Register(new TrapClear());
            IC10Extender.Register(new TrapExit());
            IC10Extender.Register(new TrapReset());
        }
    }
}
