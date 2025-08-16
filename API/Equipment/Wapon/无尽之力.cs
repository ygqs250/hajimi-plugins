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
    internal class 无尽之力 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "无尽之力";

        public override string Description => "你的攻击有20%的几率造成250%伤害";

        public override ItemType Type => ItemType.Coin;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                if(Wanjia.rand.Next(5)==0)
                {
                    st.Damage = (float)(st.Damage*2.5);
                }
            }
        }




        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
