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
    internal class 心之钢 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "心之钢";

        public override string Description => "击杀敌人之后，你的生命值上限提升8%，并且你的攻击增加3.5的自身最大生命值伤害";

        public override ItemType Type => ItemType.Coin;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void KillHander(PlayerDeathEventArgs ev)
        {
            ev.Attacker.MaxHealth *= 1.08f;
        }


        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.DamageHandler is FirearmDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as FirearmDamageHandler;
                st.Damage = st.Damage += ev.Attacker.MaxHealth * 0.035f;
            }
        }
    }
}
