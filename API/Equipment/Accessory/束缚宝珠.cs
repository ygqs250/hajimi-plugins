using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Accessory
{
    internal class 束缚宝珠 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "束缚宝珠";

        public override string Description => "使用枪械攻击人类时，有50%的几率会造成0.5s的禁锢效果，攻击ＳＣＰ时，会有0.1ｓ的禁锢效果";

        public override ItemType Type => ItemType.Coin;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        static bool ison=true;

        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {

            if (Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
                return;
            if (ev.DamageHandler is FirearmDamageHandler)
            {
                if (ison)
                {
                    if (ev.Player.IsSCP)
                    {
                        ev.Player.EnableEffect<CustomPlayerEffects.Ensnared>(1, 0.1f, false);
                    }
                    else
                    {
                        ev.Player.EnableEffect<CustomPlayerEffects.Ensnared>(1, 0.5f, false);
                    }
                    ison=false;
                }
                else
                {
                    ison = true;
                }
            }
        }
    }
}
