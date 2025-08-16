using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomItems.API.Equipment.Accessory
{
    internal class EMP手电 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "EMP手电";

        public override string Description => "打开手电之后会造成当前区域的停电，冷却时间60s";

        public uint Cooldown = 0;

        public override ItemType Type => ItemType.Coin;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void ToggledWeaponFlashlightHander(PlayerToggledWeaponFlashlightEventArgs ev)
        {
            if(Cooldown == 0 && ev.NewState == true)
            {
                Cooldown = 60;
                ev.Player.Room.LightController.FlickerLights(15);
                foreach (var door in ev.Player.Room.Doors)
                {
                    if (door.IsLocked) continue;
                    door.IsLocked = true;

                    Timing.CallDelayed(15f, () =>
                    {
                        door.IsLocked = false;
                    });
                }
                
            }
        }

        public override void ToggledFlashlightHander(PlayerToggledFlashlightEventArgs ev)
        {
            if (Cooldown == 0 && ev.NewState == true)
            {
                Cooldown = 60;
                ev.Player.Room.LightController.FlickerLights(15);
                foreach (var door in ev.Player.Room.Doors)
                {
                    if (door.IsLocked) continue;
                    door.IsLocked = true;

                    Timing.CallDelayed(15f, () =>
                    {
                        door.IsLocked = false;
                    });
                }
            }
        }
        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if(Cooldown != 0)
            {
                Cooldown--;
            }
        }
    }
}
