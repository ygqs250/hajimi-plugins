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
    internal class 神奇口袋 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "神奇口袋";

        public override string Description => "摸一下人类，人类就会进入口袋。从口袋出来的人类，会获得随机效果？？？";

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
