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
    internal class 共情 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Armor;

        public override string Name => "共情";

        public override string Description => "一名队友死亡之后，获得10点生命上限并恢复10点生命值";

        public Faction Faction = 0;

        public Player onwer = null;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void EquipHander(Wanjia wj, System.Object extra = null)
        {
            Log.Info("EquipHander" + wj.Owner.Nickname);
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
            if (players.Player1.Faction == Faction)
            {
                onwer.MaxHealth += 10;
                onwer.Health += 10;
            }
        }




        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}
