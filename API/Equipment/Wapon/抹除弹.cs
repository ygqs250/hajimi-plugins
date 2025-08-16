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
    internal class 抹除弹 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "抹除弹";

        public override string Description => "你的子弹造成不了伤害，但是每发子弹会减低15点生命上限";
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if(ev.Attacker == null) return;

            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;

            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage = 0;
            }
            if(ev.Player.MaxHealth <16)
            {
                ev.Player.MaxHealth = 1;
            }else
            {
                ev.Player.MaxHealth = ev.Player.MaxHealth - 15;
            }
                
        }




        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
