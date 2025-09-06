using CustomItems.Core;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace CustomItems.API.Equipment
{
    internal class 暗影的庇护 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "暗影的庇护";

        public override string Description => "脱离战斗后15s，在设施内部进入隐身状态，使用枪械和受到伤害会被打断";

        public override ItemType Type => ItemType.Coin;

        uint  isontimer = 0;

        bool HID = false;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Player.GetEffect<CustomPlayerEffects.Invisible>().IsEnabled == true)
            {
                ev.Player.DisableEffect<CustomPlayerEffects.Invisible>();
            }
            isontimer = 0;
        }

        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Player.GetEffect<CustomPlayerEffects.Invisible>().IsEnabled == true)
            {
                ev.Player.DisableEffect<CustomPlayerEffects.Invisible>();
            }
            isontimer = 0;
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if (HID)
            {
                isontimer = 0;
            }
            isontimer++;
            if (wj.Owner.GetEffect<CustomPlayerEffects.Invisible>().IsEnabled == true && wj.Owner.Room.Zone == MapGeneration.FacilityZone.Surface)
            {
                wj.Owner.DisableEffect<CustomPlayerEffects.Invisible>();
                return;
            }
            if (wj.Owner.GetEffect<CustomPlayerEffects.Invisible>().IsEnabled == false && isontimer > 16 && HID == false && wj.Owner.Room.Zone != MapGeneration.FacilityZone.Surface)
            {
                wj.Owner.EnableEffect<CustomPlayerEffects.Invisible>(50, 0, false);
            }


        }
            
        public override void shottedHanlde(PlayerShotWeaponEventArgs ev)
        {
            if (ev.Player.GetEffect<CustomPlayerEffects.Invisible>().IsEnabled == true)
            {
                ev.Player.DisableEffect<CustomPlayerEffects.Invisible>();
            }
            isontimer = 0;
        }

        public override void SelectedItemHander(PlayerChangedItemEventArgs ev) 
        {
            if(ev.NewItem == null)
            {
                HID = false;
                return;
            }
            if(ev.NewItem.Type == ItemType.MicroHID)
            {
                HID = true;
                if (ev.Player.GetEffect<CustomPlayerEffects.Invisible>().IsEnabled == true)
                {
                    ev.Player.DisableEffect<CustomPlayerEffects.Invisible>();
                }
            }
            else
            {
                HID = false;
            }
        }
    }
}
