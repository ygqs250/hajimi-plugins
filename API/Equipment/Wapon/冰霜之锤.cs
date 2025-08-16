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
    internal class 冰霜之锤 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "冰霜之锤";

        public override string Description => "你的子弹会造成持续3秒的40减速效果,对scp有10秒冷却";

        uint cooldown = 0;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.DamageHandler is FirearmDamageHandler)
            {
                if (ev.Player.IsSCP)
                {
                    if (cooldown > 0)
                    {
                        return;
                    }
                    cooldown = 10;
                }
                ev.Player.EnableEffect<CustomPlayerEffects.Slowness>(50, 3, false);
            }
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if (cooldown > 0)
            {
                --cooldown;
            }
        }


    }
}

