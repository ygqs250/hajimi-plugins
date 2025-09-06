using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment
{
    public class 皮糙肉厚 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "皮糙肉厚";

        public override string Description => "SCP血量提升，并且获得25%的减伤";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 0));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage = st.Damage * (float)0.7;
            }
        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
