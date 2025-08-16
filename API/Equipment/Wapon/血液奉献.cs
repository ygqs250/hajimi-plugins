using LabApi.Events.Arguments.PlayerEvents;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Wapon
{
    internal class 血液奉献 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "血液奉献";

        public override string Description => "你攻击一次队友，会减少你的4点生命值，并为队友提供6点生命上限，最高提高到150点生命上限";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (!Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
            }
            ev.Player.MaxHealth += 6;
            ev.Player.Heal(6);
            ev.Attacker.Damage(new CustomReasonDamageHandler("血液奉献",4,"血液奉献"));
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
