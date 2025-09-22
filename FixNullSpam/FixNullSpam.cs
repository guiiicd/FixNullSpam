using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace FixNullSpam;

public class FixNullSpam : BasePlugin
{
    public override string ModuleName    => "FixNullSpam";
    public override string ModuleVersion => "1.0.0.1";
    public override string ModuleAuthor  => "Nuko";

    // ReSharper disable InconsistentNaming
    private MemoryFunctionVoid<IntPtr, int>? Plat_DebugString_Buffered;

    // ReSharper restore InconsistentNaming

    public override void Load(bool hotReload)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Console.WriteLine("This plogon is only for linux.");

            return;
        }

        Plat_DebugString_Buffered = new (GameData.GetSignature("Plat_DebugString_Buffered"), Addresses.Tier0Path);
        Plat_DebugString_Buffered.Hook(hk_Plat_DebugString_Buffered, HookMode.Pre);
    }

    private static HookResult hk_Plat_DebugString_Buffered(DynamicHook arg)
        => HookResult.Stop;

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);

        Plat_DebugString_Buffered?.Unhook(hk_Plat_DebugString_Buffered, HookMode.Pre);
    }
}
