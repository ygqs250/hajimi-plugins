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
    internal class 尖端供给装置: EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "尖端供给装置";

        public override string Description => "让你的枪拥有无限子弹";

        public override ItemType Type => ItemType.Coin;

        uint isontimer = 0;

        bool HID = false;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void shottedHanlde(PlayerShotWeaponEventArgs ev)
        {
            ev.FirearmItem.StoredAmmo = ev.FirearmItem.MaxAmmo;
        }


        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.DamageHandler is FirearmDamageHandler fd)
            {
                if (fd.Firearm.ItemTypeId == ItemType.GunCom45)
                {
                    fd.Damage *= 0.25f;
                }
            }
        }
    }
}
