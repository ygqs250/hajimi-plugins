using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Armor
{
    internal class 石化铠甲 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "石化铠甲";

        public override string Description => "装备之后，对所以伤害有60%的抗性，但是你会被减速";
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void DefendHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage = st.Damage * (float)0.6;
            }

        }
        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if (wj.Owner.GetEffect<CustomPlayerEffects.Stained>().IsEnabled == false)
            {
                wj.Owner.EnableEffect<CustomPlayerEffects.Stained>();
            }
        }

        public override void RemoveHander(Wanjia wj, System.Object extra = null)
        {
            wj.Owner.DisableEffect<CustomPlayerEffects.Stained>();
        }
    }

}

