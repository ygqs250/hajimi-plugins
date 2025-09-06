using CustomItems.Core;
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
    internal class 高爆弹 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "高爆弹";

        public override string Description => "你的每发子弹会造成10点5米半径的溅射伤害，不区分敌友";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.DamageHandler is FirearmDamageHandler)
            {
                foreach (Player player in Player.List)
                {
                    if (player.IsAlive && Vector3.Distance(player.Position, ev.Player.Position) < 5)
                    {
                        player.Damage(new CustomReasonDamageHandler("高爆弹", 10, "高爆弹"));
                    }
                }
            }

        }



        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}

