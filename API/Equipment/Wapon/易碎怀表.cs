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
    internal class 易碎怀表 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "易碎怀表";

        public override string Description => "当你的生命值较高时，你造成的伤害翻倍，当你的生命低于30时，你失去此装备";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Attacker == null) return;
            if (ev.DamageHandler is StandardDamageHandler st)
            {
                st.Damage *= 2;
            }
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if(wj.Owner.Health<30)
            {
                wj.Weapon = null;
            }
        }


    }
}

