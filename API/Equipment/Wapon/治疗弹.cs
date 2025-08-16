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
    internal class 治疗弹 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "治疗弹";

        public override string Description => "你的子弹造成不了伤害，但是每发子弹会回复目标10点生命，SCP回复减半";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage = 0;
            }
            if (ev.Player.IsHuman)
            {
                if(ev.Player.Health<ev.Player.MaxHealth)
                {
                    ev.Player.Heal(10);
                }
            }
            else
            {
                if (ev.Player.Health < ev.Player.MaxHealth)
                {
                    ev.Player.Heal(5);
                }
            }

        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
