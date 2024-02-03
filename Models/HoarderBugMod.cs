using System.Collections.Generic;
using GameNetcodeStuff;

namespace BefriendLootBugs{
    public class HoarderBugMod(HoarderBugAI hoarderBug){
        public static Dictionary<HoarderBugAI, HoarderBugMod> HoarderBugMods {get;} = [];

        public HoarderBugAI HoarderBug {get;} = hoarderBug;
        public bool IsFriendly {get; set;} = false;
        public PlayerControllerB Friend {get; set;}

        public static HoarderBugMod Find(HoarderBugAI hoarderBug){
            if(!HoarderBugMods.ContainsKey(hoarderBug))
                HoarderBugMods[hoarderBug] = new HoarderBugMod(hoarderBug);
            return HoarderBugMods[hoarderBug];
        }


        public void Befriend(PlayerControllerB player){
            IsFriendly = true;
            Friend = player;
        }


        public void FollowFriend(){
            if(Friend is null) return;
            HoarderBug.targetPlayer = Friend;
            HoarderBug.movingTowardsTargetPlayer = true;
            HoarderBug.isAngry = false;
            HoarderBug.annoyanceMeter = 0f;
        }


        public void Update(){
            if(IsFriendly)
                FollowFriend();
        }


        public void Grab(GrabbableMod grabbable){
            if(grabbable.IsGifted)
                Befriend(grabbable.LastOwner);
        }
    }
}