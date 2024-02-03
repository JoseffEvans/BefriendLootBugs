using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace BefriendLootBugs
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        protected readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);
        public static new ManualLogSource Logger {get; protected set;}

        protected List<Type> Patches {get;} = [ 
            typeof(Plugin),
            typeof(HoarderBugPatch),
            typeof(GrabbablePatch),

            // // debug
            // typeof(RoundManagerPatch),
            // typeof(PlayerPatch)
        ];

        private void Awake()
        {
            Logger ??= base.Logger;
            foreach(Type patch in Patches) _harmony.PatchAll(patch);
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
