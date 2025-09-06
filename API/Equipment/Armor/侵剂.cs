using CustomItems.Core;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Armor
{
    internal class 侵剂 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "侵剂";

        public override string Description => "一名敌人死亡之后，获得8点生命上限并恢复5点生命值";

        public Faction Faction = 0;

        public Player onwer = null;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void EquipHander(Wanjia wj, System.Object extra = null)
        {
            Faction = wj.Owner.Faction;
            onwer = wj.Owner;
            Wanjia.DeadHandler += AddHeath;
        }

        public override void RemoveHander(Wanjia wj, System.Object extra = null)
        {
            onwer = null;
            Wanjia.DeadHandler -= AddHeath;
        }

        private void AddHeath(object sender, DualPlayerEventArgs players)
        {
            if (players.Player1.Faction != Faction)
            {
                onwer.MaxHealth += 8;
                onwer.Health += 5;
            }
        }



        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
