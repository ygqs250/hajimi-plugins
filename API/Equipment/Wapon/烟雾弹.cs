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
    internal class 烟雾弹 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "烟雾弹";

        public override string Description => "你的子弹会对敌人造成3秒的烟雾效果，让起失去远处视野";

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
                    ev.Player.EnableEffect<CustomPlayerEffects.FogControl>(4, 3f, false);
            }
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
