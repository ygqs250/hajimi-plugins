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
    public class 诅咒权杖 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "诅咒权杖";

        public override string Description => "你的伤害翻2倍，但是不能使用药品";

        public override ItemType Type => ItemType.Coin;


        public override void EquipHander(Wanjia wj, System.Object extra = null)
        {
            wj.CanUseMedical = 1024;
        }
        public override void RemoveHander(Wanjia wj, System.Object extra = null)
        {
            wj.CanUseMedical = 0;
        }
        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            wj.CanUseMedical = 1024;
        }
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Attacker == null) return;
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage *= 3;
            }
        }
    }
}
