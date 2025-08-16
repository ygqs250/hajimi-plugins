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
    internal class 死亡之舞 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "死亡之舞";

        public override string Description => "你受到的攻击造成的伤害会变成4秒的流血效果，而不是直接扣除";

        float bloodNum = 0;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
             if (ev.DamageHandler is AttackerDamageHandler)
            {
                AttackerDamageHandler st = ev.DamageHandler as AttackerDamageHandler;
                bloodNum += st.Damage;
                st.Damage = 0;

            }
        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if(bloodNum>0.5)
            {
                wj.Owner.Damage(new CustomReasonDamageHandler("死亡之舞", bloodNum / 3, "死亡之舞"));
                bloodNum -= bloodNum / 4;
            }
        }
    }
}
