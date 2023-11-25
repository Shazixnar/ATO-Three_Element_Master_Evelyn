using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Obeliskial_Essentials;

namespace TraitMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.stiffmeds.obeliskialessentials")]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal const int ModDate = 20231125;
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;
        private void Awake()
        {
            Essentials.medsTexts["custommainperkburn2d"] = "Burn on enemies deals double damage if the target have 4 or less curses (Burn included)";
            Log = Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} has loaded!");
            // register with Obeliskial Essentials
            Essentials.AddModVersionText(PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION, ModDate.ToString());
            // apply patches
            harmony.PatchAll();
        }
    }
}
