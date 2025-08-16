using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomItems.API.Equipment.Accessory
{
    internal class 鹰眼 : EquipmentBase
    {
        public override EquipmentType EquipType => EquipmentType.Accessory;

        public override string Name => "鹰眼";

        public override string Description => "去除战争迷雾，你可以看得更远，并且获得眼镜效果";

        Vector3 quipposition;

        public override ItemType Type => ItemType.Coin;
        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void EquipHander(Wanjia wj, System.Object extra = null)
        {
            wj.Owner.EnableEffect<CustomPlayerEffects.Scp1344>(50, 0, false);
            wj.Owner.EnableEffect<CustomPlayerEffects.FogControl>(1, 0, false);
        }
        public override void RemoveHander(Wanjia wj, System.Object extra = null)
        {
            wj.Owner.DisableEffect<CustomPlayerEffects.Scp1344>();
            wj.Owner.DisableEffect<CustomPlayerEffects.FogControl>();
        }
    }
}
