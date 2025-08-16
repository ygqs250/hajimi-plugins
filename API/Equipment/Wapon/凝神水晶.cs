using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static CustomItems.API.EquipmentBase;

namespace CustomItems.API.Equipment.Armor
{
    internal class 凝神水晶 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "凝神水晶";

        public override string Description => "你对15m内的敌人，提高50%的删害";
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
                if(Vector3.Distance(ev.Attacker.Position,ev.Player.Position) < 15)
                {
                    st.Damage *= (float)1.5;
                }
            }
        }



        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}

