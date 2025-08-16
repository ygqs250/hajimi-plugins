using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomItems.API.Equipment.Armor
{
    internal class 破舰者 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "破舰者";

        public override string Description => "你附件15米没有友军的话，获得60%的减伤";

        bool isOn=true;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                if (isOn == true)
                {
                   st.Damage = st.Damage * (float)0.4;
                }

            }
        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            foreach (Player player in Player.List)
            {
                if (player.IsAlive && Vector3.Distance(player.Position, wj.Owner.Position) < 15 && Wanjia.ComparePlayerFaction(wj.Owner, player))
                {
                    isOn = false;
                }else
                {
                    isOn = true;
                }
            }
        }
    }
}
