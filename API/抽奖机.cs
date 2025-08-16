using CustomItems.Core;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace CustomItems.API
{

    public class LotteryItem
    {

        public LotteryItem(ushort id, int Weight)
        {
            this.ItemID = id;
            this.Weight = Weight;
        }

        public ushort ItemID { get; set; }
        public int Weight { get; set; } // 相对权重（如1000、20）
    }

public class 抽奖机 : CustomItem
    {
        public override string Name => "抽奖机";

        public override string Description => "";

        public override ItemType Type => ItemType.SCP1576;

        public override void OnRegistered() { }

        public override void OnUnregistered() { }

        private static List<LotteryItem> LotteryPool = new List<LotteryItem>();

        public readonly Dictionary<Player, bool> usedPlayers = new Dictionary<Player, bool>();

        static public List<抽奖机> Lotterys = new List<抽奖机>();

        public 抽奖机()
        {
            // 自动注册到静态列表
            Lotterys.Add(this);
        }

        static public void Lotterysclear(Player player)
        {
            if(player == null)
                return;
            foreach (var Lotterysmachine in Lotterys)
            {
                if (!Lotterysmachine.usedPlayers.TryGetValue(player, out var isuesed))
                {
                    Lotterysmachine.usedPlayers.Add(player, false);
                }
                else
                {
                    Lotterysmachine.usedPlayers[player] = false;
                }
            }
        }


        public override void OnDropped(PlayerDroppedItemEventArgs ev)
        {
            NetworkServer.UnSpawn(ev.Pickup.Base.gameObject);
            ev.Pickup.Base.gameObject.transform.localScale = Vector3.one * 5;
            Timing.RunCoroutine(latersetKinematic(ev.Pickup));
            //ev.Pickup.Rigidbody.detectCollisions = false;
            NetworkServer.Spawn(ev.Pickup.Base.gameObject);
        }

        private IEnumerator<float> latersetKinematic(Pickup pickup)
        {
            yield return Timing.WaitForSeconds(5.5f);
            if(pickup != null)
            pickup.Rigidbody.isKinematic = true;
        }
        public override void OnPickingUp(PlayerPickingUpItemEventArgs ev) 
        {
            ev.IsAllowed = false;
            ev.Pickup.IsLocked = false;
            ev.Pickup.IsInUse = false;
            if (!usedPlayers.TryGetValue(ev.Player, out var isuesed))
            {
                usedPlayers.Add(ev.Player, false);
            }
            else if (isuesed)
            {
                return;
            }

            ushort Lottery1 = Draw();
            ushort Lottery2 = Draw();
            Item item1;
            CustomItems.TryGive(Lottery1, ev.Player,out item1);
            Item item2;
            CustomItems.TryGive(Lottery2, ev.Player, out item2);


            usedPlayers[ev.Player] = true;

        }

        private static readonly Random _random = new Random();

        // 核心抽奖方法（使用相对权重）
        public static void AddLottery(LotteryItem item)
        {
            LotteryPool.Add(item);
        }
            public static ushort Draw()
            {
                List<LotteryItem> items = LotteryPool;
                // 参数验证
                if (items == null || !items.Any())
                    throw new ArgumentException("奖品列表不能为空");

                // 计算总权重
                int totalWeight = items.Sum(item => item.Weight);
                if (totalWeight <= 0)
                    throw new InvalidOperationException("总权重必须大于0");

                // 生成随机数（范围：0 ~ totalWeight-1）
                int randomValue = _random.Next(totalWeight);

                // 根据权重区间选择物品
                int cumulative = 0;
                foreach (var item in items.OrderBy(x => x.ItemID)) // 确保顺序一致性
                {
                    cumulative += item.Weight;
                    if (randomValue < cumulative)
                        return item.ItemID;
                }

                throw new InvalidOperationException("抽奖失败，请检查权重配置");
            }
    }
}
