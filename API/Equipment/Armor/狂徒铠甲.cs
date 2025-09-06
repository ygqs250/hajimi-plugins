using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment
{
    internal class 狂徒铠甲 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "狂徒铠甲";

        public override string Description => "如果你5s没有收到伤害，将会每秒回复10点生命值";

        public override ItemType Type => ItemType.Coin;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if (wj.SufferTimer > 5)
            {
                wj.Owner.Heal(10);
            }
        }
    }
}
