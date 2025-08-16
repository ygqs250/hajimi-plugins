using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Armor
{
    internal class 等离子护盾 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "等离子护盾";

        public override string Description => "装备之后，脱离战斗10秒后会按照2点每秒的速度获得一个护盾，护盾最高100点，但是不能使用药物";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void OnPickedUp(PlayerPickedUpItemEventArgs ev)
        {
            ev.Player.MaxHumeShield = 100;
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
             if(wj.DisengageTimer > 10)
            {
                if(wj.Owner.HumeShield < 100)
                {
                    wj.Owner.HumeShield += 2;
                }
            }
        }
    }

}

