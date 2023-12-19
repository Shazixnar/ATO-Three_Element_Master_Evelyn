using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Obeliskial_Essentials;

namespace ThreeElementMasterEvelyn
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.stiffmeds.obeliskialessentials")]
    [BepInDependency("com.stiffmeds.obeliskialcontent")]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal const int ModDate = 20231217;
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} has loaded!");
            // register with Obeliskial Essentials
            Essentials.RegisterMod(
                _name: PluginInfo.PLUGIN_NAME,
                _author: "Shazixnar",
                _description: "Redone Evelyn traits and element cards, as well as a perk.",
                _version: PluginInfo.PLUGIN_VERSION,
                _date: ModDate,
                _link: @"https://across-the-obelisk.thunderstore.io/package/Shazixnar/Three_Element_Master_Evelyn/",
                _contentFolder: "Three Element Master Evelyn",
                _type: new string[5] { "content", "hero", "trait", "card", "perk" }
            );
            Essentials.medsTexts["custommainperkburn2d"] = "Burn on enemies deals double damage if the target have 4 or less curses (Burn included)";
            // apply patches
            harmony.PatchAll();
        }
    }
}
