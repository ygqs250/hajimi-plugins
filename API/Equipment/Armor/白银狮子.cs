using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Armor
{
    internal class 白银狮子 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "白银狮子";

        public override string Description => "使你受到的单次伤害不会超过50";
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                if(st.Damage>=50 || st.Damage<0)
                {
                    st.Damage = (float)49.5;
                }
            }

        }



        //public override void DefendHander(Player onwer, Player suffer, DamageHandlerBase damgehander, System.Object extra = null)
        //{

        //}

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
