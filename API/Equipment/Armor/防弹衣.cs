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
    internal class 防弹衣 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "防弹衣";

        public override string Description => "装备之后，对子弹有40%的抗性";
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void DefendHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is FirearmDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as FirearmDamageHandler;
                st.Damage = st.Damage * (float)0.40;
            }

        }
    }

}

