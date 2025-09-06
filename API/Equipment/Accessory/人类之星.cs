using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Accessory
{
    internal class 人类之心 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "人类之心";

        public override string Description => "你不能对人类造成伤害，其他人类也不能对你造成伤害";

        public override ItemType Type => ItemType.Coin;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Attacker.IsHuman && ev.DamageHandler is StandardDamageHandler st)
            {
                    st.Damage = 0;
            }
            

        }
        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if (Wanjia.SCPCount == 0)
            {
                wj.Weapon = null;
            }
        }

    }
}
