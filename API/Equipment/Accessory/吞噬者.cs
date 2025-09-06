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
    internal class 吞噬者 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "吞噬者";

        public override string Description => "你的攻击有概率吞噬目标的一件装备";

        public override ItemType Type => ItemType.Coin;

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
                if (ev.Player.IsSCP)
                {

                }
                else
                {
                    Wanjia wj = Wanjia.Create(ev.Player);
                    int ran=Wanjia.rand.Next(0, 10);
                    switch (ran)
                    {
                        case 0:
                            if(wj.Weapon != null)
                            {
                                ev.Attacker.SendBroadcast($"你吞噬了{ev.Player.Nickname}的{wj.Weapon.Name}", 8);
                                Wanjia.Create(ev.Player).Weapon = null;
                            }
                            break;
                        case 1:
                            if (wj.Armor != null)
                            {
                                ev.Attacker.SendBroadcast($"你吞噬了{ev.Player.Nickname}的{wj.Armor.Name}", 8);
                                Wanjia.Create(ev.Player).Armor = null;
                            }
                            break;
                        case 2:
                            if (wj.Accessory != null)
                            {
                                ev.Attacker.SendBroadcast($"你吞噬了{ev.Player.Nickname}的{wj.Accessory.Name}", 8);
                                Wanjia.Create(ev.Player).Accessory = null;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
