using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.API.Equipment.Accessory
{
    internal class 心灵控制 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "心灵控制";

        public override string Description => "击杀敌人之后，敌人会变成和你同阵营角色复活在你身边";

        public override ItemType Type => ItemType.Coin;
        public override void KillHander(PlayerDeathEventArgs ev)
        {
            Timing.RunCoroutine(changeRoleAfterDelay(ev.Player,ev.Attacker));
        }
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void EquipHander(Wanjia wj, System.Object extra = null)
        {
            foreach (Item item in wj.Owner.Items)
            {
                if (item is FirearmItem firearmItem)
                {
                    firearmItem.StoredAmmo = 0;
                }
            }
        }

        private IEnumerator<float> changeRoleAfterDelay(Player player, Player attater)
        {
            yield return Timing.WaitForSeconds(0.5f);
            player.SetRole(attater.Role);
            player.Position = attater.Position;
        }
    }
}
