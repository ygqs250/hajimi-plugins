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
    internal class 收集者 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "收集者";

        public override string Description => "秒杀百分之20血量下的人类，对SCP效果为百分之5";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Attacker == null) return;
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                if (ev.Player.Health - st.Damage < ev.Player.MaxHealth / 5)
                {
                    ev.Player.Kill("收集者");
                }
            }
        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}

