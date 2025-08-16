using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Accessory
{
    internal class 名刀司令 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "名刀司令";

        public override string Description => "收到致命伤害会进入3s的无敌模式，然后该装备会报废";

        public override ItemType Type => ItemType.Coin;

        int deadtimer = 3;

        bool isDead = false;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void DefendDyingHander(PlayerDyingEventArgs ev)
        {
            isDead = true;
            ev.IsAllowed= false;
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if (isDead)
            {
                deadtimer--;
            }
            if(deadtimer <= 0)
            {
                wj.Accessory = null;
            }
        }
    }
}
