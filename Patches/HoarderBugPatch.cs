using HarmonyLib;
using Unity.Netcode;


namespace BefriendLootBugs
{

    [HarmonyPatch(typeof(HoarderBugAI))]
    public class HoarderBugPatch{


        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Update(HoarderBugAI __instance) =>
            HoarderBugMod.Find(__instance).Update();
        

        [HarmonyPatch("GrabItem")]
        [HarmonyPostfix]
        public static void OnGrabItem(NetworkObject item, HoarderBugAI __instance) =>
            HoarderBugMod.Find(__instance).Grab(
                GrabbableMod.Find(item.gameObject.GetComponent<GrabbableObject>()));
    }
}
