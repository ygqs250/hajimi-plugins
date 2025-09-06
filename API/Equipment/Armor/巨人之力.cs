using LabApi.Events.Arguments.PlayerEvents;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Armor
{
    internal class 巨人之力 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "巨人之力";

        public override string Description => "装备之后，对获得50%的伤害抗性，但是你体型会变大";
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void DefendHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage = st.Damage * (float)0.5;
            }

        }

        public override void EquipHander(Wanjia wj, System.Object extra = null)
        {
            wj.Owner.Scale = new UnityEngine.Vector3(wj.Owner.Scale.x*1.2f, wj.Owner.Scale.y*1.2f, wj.Owner.Scale.z*1.2f);
        }
        public override void RemoveHander(Wanjia wj, System.Object extra = null)
        {
            wj.Owner.Scale = new UnityEngine.Vector3(wj.Owner.Scale.x / 1.2f, wj.Owner.Scale.y / 1.2f, wj.Owner.Scale.z / 1.2f);
        }
    }
}
