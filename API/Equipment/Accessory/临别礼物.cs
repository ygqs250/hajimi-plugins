using InventorySystem.Items.Pickups;
using InventorySystem.Items;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using InventorySystem.Items.ThrowableProjectiles;

namespace CustomItems.API.Equipment.Accessory
{
    public class 临别礼物 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "临别礼物";

        public override string Description => "死亡后，原地留下一个手雷";

        public override ItemType Type => ItemType.Coin;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void DefendDyingHander(PlayerDyingEventArgs ev)
        {
            TimedGrenadeProjectile.SpawnActive(ev.Player.Position, ItemType.GrenadeHE, ev.Player,5);
            Wanjia.Create(ev.Player).Accessory = null;
        }
    }
}
