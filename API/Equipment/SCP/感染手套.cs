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
    internal class 感染手套 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "感染手套";

        public override string Description => "摸一下人类，人类就会变成小僵尸";

        public override ItemType Type => ItemType.Coin;

        public int ison = 0;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 0));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {

        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
