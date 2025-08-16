using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomItems.API.Equipment.Accessory
{
    internal class 逆时怀表 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "逆时怀表";

        public override string Description => "死亡瞬间回溯自身至装备该道具时的位置，回复全部血量，触发后装备报废";

        Vector3 quipposition;

        public override ItemType Type => ItemType.Coin;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void EquipHander(Wanjia wj, System.Object extra = null)
        {
            quipposition = wj.Owner.Position;
        }

        public override void DefendDyingHander(PlayerDyingEventArgs ev)
        {
            ev.Player.Position = quipposition;
            Wanjia.Create(ev.Player).Accessory = null;
            ev.IsAllowed = false;
        }

    }
}
