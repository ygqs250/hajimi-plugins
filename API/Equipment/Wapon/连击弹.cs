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
    internal class 连击弹 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "连击弹";

        public override string Description => "每次造成枪械伤害时，增加1%的伤害,最多60%，伤害加成会在停止造成伤害10秒后归零";

        private int DamageTimes = 0;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            DamageTimes++;
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage *= (float)(100 + DamageTimes) / 100;
            }
        }



        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if(wj.AttactTimer>15)
            {
                DamageTimes = 0;
            }
        }
    }
}

