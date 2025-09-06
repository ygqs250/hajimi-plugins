using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace CustomItems.API.Equipment
{
    internal class 疾行之靴 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "疾行之靴";

        public override string Description => "脱离战斗后6s，获得50%的移速加成，造成伤害和受到伤害会被打断";

        public override ItemType Type => ItemType.Coin;

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void DefendHander(LabApi.Events.Arguments.PlayerEvents.PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Player.GetEffect<CustomPlayerEffects.Stained>().IsEnabled == true)
            {
                ev.Player.DisableEffect<CustomPlayerEffects.MovementBoost>();
            }
        }

        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if (wj.Owner.GetEffect<CustomPlayerEffects.Stained>().IsEnabled == false && wj.DisengageTimer > 6)
            {
                    wj.Owner.EnableEffect< CustomPlayerEffects.MovementBoost>(50,0,false);
            }
        }
    }
}
