using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Armor
{
    internal class 玫瑰原盾 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "玫瑰原盾";

        public override string Description => "你奔跑时获得75%的减删";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                if (ev.Player.ReferenceHub.roleManager.CurrentRole is IFpcRole fpcRole)
                {
                    if (fpcRole.FpcModule.CurrentMovementState == PlayerMovementState.Sprinting)
                    {
                        st.Damage = st.Damage * (float)0.25;
                    }
                }
               
            }
        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
