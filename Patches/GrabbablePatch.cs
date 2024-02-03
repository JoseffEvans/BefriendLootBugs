using HarmonyLib;

namespace BefriendLootBugs{

    [HarmonyPatch(typeof(GrabbableObject))]
    public class GrabbablePatch{

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Update(GrabbableObject __instance) =>
            GrabbableMod.Find(__instance).Update();
    }
}