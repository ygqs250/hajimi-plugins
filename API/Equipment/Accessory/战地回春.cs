using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomItems.API.Equipment
{
    internal class 战地回春 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "战地回春";

        public override string Description => "使用枪械命中或被枪械命中时，恢复自身附近友军生命值";

        public override ItemType Type => ItemType.Coin;

        public int ison = 0;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            foreach (Player player in Player.List)
            {
                if (player.IsAlive && Vector3.Distance(player.Position, ev.Player.Position) < 5 && Wanjia.ComparePlayerFaction(ev.Player,ev.Attacker))
                {
                    player.Heal(5);
                }
            }
        }


        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if(Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            foreach (Player player in Player.List)
            {
                if (player.IsAlive && Vector3.Distance(player.Position, ev.Player.Position) < 5 && Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                {
                    player.Heal(5);
                }
            }
        }
    }
}
