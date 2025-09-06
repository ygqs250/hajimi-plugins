using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CustomItems.API.EquipmentBase;

namespace CustomItems.API.Equipment.Armor
{
    internal class 龙之威慑 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "龙之威慑";

        public override string Description => "装备数量小于自身的玩家只能对自身造成微小伤害，大于自身的玩家对自身造成大量伤害。SCP永远视为装备平等";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }

        public override void DefendHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if(ev.Attacker == null || ev.Attacker.IsSCP ) return;
            int plyer_count = 0;
            int attacker_count = 0;
            Wanjia pl = Wanjia.Create(ev.Player);
            Wanjia at = Wanjia.Create(ev.Attacker);
            if (pl.Accessory != null) plyer_count++;
            if (pl.Armor != null) plyer_count++;
            if (pl.Weapon != null) plyer_count++;

            if (at.Accessory != null) attacker_count++;
            if (at.Armor != null) attacker_count++;
            if (at.Weapon != null) attacker_count++;

            if (ev.DamageHandler is StandardDamageHandler st)
            {
                if(plyer_count< attacker_count)
                {
                    st.Damage = st.Damage * 2;
                }
                if(plyer_count > attacker_count)
                {
                    st.Damage = st.Damage * 0.25f;
                }
            }

        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}

