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
    internal class 恶魔魔典 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "恶魔魔典";

        public override string Description => "你的子弹会让敌人在20秒内无法使用药物";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.Attacker == null) return;
            if (ev.DamageHandler is FirearmDamageHandler fd)
            {
                Wanjia.Create(ev.Player).CanUseMedical = 20;
            }
        }



        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}

