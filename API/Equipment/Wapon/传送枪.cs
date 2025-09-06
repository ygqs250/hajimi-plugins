using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CustomItems.API.EquipmentBase;

namespace CustomItems.API.Equipment.Armor
{
    internal class 传送枪 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "传送枪";

        public override string Description => "你的子弹会在攻击50血以下的人类时，有50%的概率将他传送到老头空间";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.DamageHandler is FirearmDamageHandler fd)
            {
                if (ev.Player.Health < 50)
                {
                    if (Wanjia.rand.Next(0, 2) == 0)
                    {
                        PocketDimension.ForceInside(ev.Player);
                    }
                }
            }
        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}

