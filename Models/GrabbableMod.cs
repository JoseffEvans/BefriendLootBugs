using System;
using System.Collections.Generic;
using GameNetcodeStuff;

namespace BefriendLootBugs{
    public class GrabbableMod(GrabbableObject grabbable)
    {
        public static Dictionary<GrabbableObject, GrabbableMod> GrabbableMods {get;} = [];

        public GrabbableObject Grabbable { get; } = grabbable;
        public PlayerControllerB LastOwner {get; set;}
        public DateTime LastOwnedTime {get; set;}
        public int GiftableTime {get;} = 20;

        public bool IsGifted => LastOwner is not null && LastOwnedTime.AddSeconds(GiftableTime) > DateTime.Now;

        public static GrabbableMod Find(GrabbableObject grabbable){
            if(!GrabbableMods.ContainsKey(grabbable))
                GrabbableMods[grabbable] = new GrabbableMod(grabbable);
            return GrabbableMods[grabbable];
        }


        public void Update(){
            if(Grabbable.playerHeldBy is not null){
                LastOwner = Grabbable.playerHeldBy;
                LastOwnedTime = DateTime.Now;
            }
        }
    }
}