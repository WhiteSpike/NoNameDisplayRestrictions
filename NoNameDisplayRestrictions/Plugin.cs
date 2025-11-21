using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NoNameDisplayRestrictions.Misc;
using NoNameDisplayRestrictions.Patches;
namespace NoNameDisplayRestrictions
{
    [BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static readonly Harmony harmony = new(Metadata.GUID);
        internal static readonly ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(Metadata.NAME);

        void Awake()
        {
            harmony.PatchAll(typeof(GameNetworkManagerPatcher));
			harmony.PatchAll(typeof(PlayerControllerBPatcher));
			mls.LogInfo($"{Metadata.NAME} {Metadata.VERSION} has been loaded successfully.");
        }
    }   
}
