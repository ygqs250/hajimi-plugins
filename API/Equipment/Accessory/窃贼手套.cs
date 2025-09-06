using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment
{
    internal class 窃贼手套 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "窃贼手套";

        public override string Description => "装备此物品后，每隔一段时间，你的武器和防具会随机变化";

        public override ItemType Type => ItemType.Coin;

        uint timer= 0;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {
            if(timer == 60)
            {
                timer = 0;
                for (int i = 0;i< 100;i++)
                {
                    ushort Lottery1 = 抽奖机.Draw();
                    CustomItem item1 = CustomItems.GetById(Lottery1);
                    if(item1 is EquipmentBase item)
                    {
                        if (item.EquipType == EquipmentType.Weapon)
                        {
                            wj.Weapon = (EquipmentBase)Activator.CreateInstance(item.GetType());
                                break;
                        }
                    }
                }

                for (int i = 0; i < 100; i++)
                {
                    ushort Lottery1 = 抽奖机.Draw();
                    CustomItem item1 = CustomItems.GetById(Lottery1);
                    if (item1 is EquipmentBase item)
                    {
                        if (item.EquipType == EquipmentType.Armor)
                        {
                            wj.Armor = (EquipmentBase)Activator.CreateInstance(item.GetType());
                                break;
                        }
                    }
                }
            }
            timer++;
        }
    }
}
