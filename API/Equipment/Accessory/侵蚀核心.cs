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
    internal class 侵蚀核心 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "侵蚀核心";

        public override string Description => "攻击有概率击落目标手持物品";

        public override ItemType Type => ItemType.Coin;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }


        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.DamageHandler is FirearmDamageHandler)
            {
                if (ev.Player.IsSCP)
                {

                }
                else
                {
                    Wanjia wj = Wanjia.Create(ev.Player);
                    int ran = Wanjia.rand.Next(0, 10);
                    if (ran < 3)
                    {
                        ev.Player.DropItem(ev.Player.CurrentItem);
                    }
                }
            }
        }
    }
}
