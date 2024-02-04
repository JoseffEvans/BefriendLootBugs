using System.Collections.Generic;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace BefriendLootBugs{
    public class HoarderBugMod(HoarderBugAI hoarderBug){
        public static Dictionary<HoarderBugAI, HoarderBugMod> HoarderBugMods {get;} = [];

        public HoarderBugAI HoarderBug {get;} = hoarderBug;
        public bool IsFriendly {get; set;} = false;
        public PlayerControllerB Friend {get; set;}
        public int InsideShipTime {get; protected set;} = 0;
        public bool InsideShip => InsideShipTime > 20;

        public bool FriendOutside => Friend.thisPlayerBody.transform.position.y > -80f;

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

            if(FriendOutside && !HoarderBug.isOutside)
                TeleportOutside();
            if(!FriendOutside && HoarderBug.isOutside)
                TeleportInside();
        }


        public void Update(){
            if(IsFriendly)
                FollowFriend();
            if(InsideShip)
                DropHeldItem();
            UpdateShipTimeCounter();
        }


        public void Grab(GrabbableMod grabbable){
            if(grabbable.IsGifted)
                Befriend(grabbable.LastOwner);
        }

        
        void TeleportOutside()
        {
            Vector3 doorPostition = RoundManager.Instance.GetNavMeshPosition(
                RoundManager.FindMainEntrancePosition(getTeleportPosition: true, true));

            Teleport(doorPostition);

            HoarderBug.isOutside = true;
            HoarderBug.allAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode");

            PlayDoorAudio();
        }

        void TeleportInside(){
            Vector3 doorPostition = RoundManager.Instance.GetNavMeshPosition(
                RoundManager.FindMainEntrancePosition(getTeleportPosition: true, false));

            Teleport(doorPostition);

            HoarderBug.isOutside = false;
            HoarderBug.allAINodes = GameObject.FindGameObjectsWithTag("AINode");
        } 

        void Teleport(Vector3 position){
            if (HoarderBug.IsOwner)
            {
                HoarderBug.agent.enabled = false;
                HoarderBug.transform.position = position;
                HoarderBug.enabled = true;
            }
            else
            {
                HoarderBug.transform.position = position;
            }
            HoarderBug.serverPosition = position;
        }


        void PlayDoorAudio(){
            EntranceTeleport entranceTeleport = RoundManager.FindMainEntranceScript(true);
            if (entranceTeleport.doorAudios != null && entranceTeleport.doorAudios.Length != 0)
            {
                entranceTeleport.entrancePointAudio.PlayOneShot(entranceTeleport.doorAudios[0]);
                WalkieTalkie.TransmitOneShotAudio(entranceTeleport.entrancePointAudio, entranceTeleport.doorAudios[0]);
            }
        }


        void DropHeldItem(){
            NetworkObject heldItem = HoarderBug.heldItem.itemGrabbableObject.GetComponent<NetworkObject>();
            HoarderBug.DropItemAndCallDropRPC(heldItem, false);
        }


        void UpdateShipTimeCounter(){
            if(HoarderBug.isInsidePlayerShip)
                InsideShipTime++;
            else
                InsideShipTime = 0;
        }
    }
}