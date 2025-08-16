using CustomItems.API.Equipment.Accessory;
using CustomItems.Core;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static InventorySystem.Items.Usables.Scp330.Scp330Translations;

namespace CustomItems.API
{
    public class Wanjia
    {
        // 静态映射表：Player -> Wanjia
        private static readonly ConcurrentDictionary<Player, Wanjia> _playerMap =
            new ConcurrentDictionary<Player, Wanjia>();

        // 实例属性

        public static System.Random rand = new System.Random();
        public Player Owner { get; }
        private EquipmentBase _weapon=null;
        public EquipmentBase Weapon
        {
            get => _weapon; // 返回后备字段，避免递归
            set
            {
                if (_weapon == null && value != null)
                {
                    value.EquipHander(this); // 注意：这里应该用 value，而不是 _weapon（还未赋值）
                }
                if (value == null && _weapon != null)
                {
                    _weapon.RemoveHander(this); // 这里用 _weapon，因为旧值还在里面
                }
                _weapon = value; // 最后赋值
            }
        }

        private EquipmentBase _armor=null;
        public EquipmentBase Armor
        {
            get => _armor;
            set
            {
                if (_armor == null && value != null)
                {
                    value.EquipHander(this);
                }
                if (value == null && _armor != null)
                {
                    _armor.RemoveHander(this);
                }
                _armor = value;
            }
        }

        private EquipmentBase _accessory = null;
        public EquipmentBase Accessory
        {
            get => _accessory;
            set
            {
                if (_accessory == null && value != null)
                {
                    value.EquipHander(this);
                }
                if (value == null && _accessory != null)
                {
                    _accessory.RemoveHander(this);
                }
                _accessory = value;
            }
        }

        //多少秒没有受到伤害
        public uint SufferTimer { get; set; } = 0;

        //多少秒没有造成伤害
        public uint AttactTimer { get; set; } = 0;

        //脱离战斗多少秒
        public uint DisengageTimer { get; set; } = 0;

        //b不动多少秒
        public uint moveTimer { get; set; } = 0;

        public Vector3 lastPostion { get; set; } = new();

        public bool CanUseMedical = true;

        public uint scpKillHealCD = 0;



        public static event EventHandler<DualPlayerEventArgs> DeadHandler;


        public static void DoDeadHandler(Player p1, Player p2)
        {
            DeadHandler?.Invoke(null, new DualPlayerEventArgs(p1, p2));
        }


        public void attack(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            Weapon?.AttactHander(ev, extra);
            Armor?.AttactHander(ev, extra);
            Accessory?.AttactHander( ev, extra);
        }

        public void defend(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            Weapon?.DefendHander(ev, extra);
            Armor?.DefendHander(ev, extra);
            Accessory?.DefendHander(ev, extra);
        }

        public void attackdying(PlayerDyingEventArgs ev)
        {
            Weapon?.AttactDyingHander(ev);
            Armor?.AttactDyingHander(ev);
            Accessory?.AttactDyingHander(ev);
        }

        public void defenddying(PlayerDyingEventArgs ev)
        {
            Weapon?.DefendDyingHander(ev);
            Armor?.DefendDyingHander(ev);
            Accessory?.DefendDyingHander(ev);
        }

        public void attackdeath(PlayerDeathEventArgs ev)
        {
            Weapon?.KillHander(ev);
            Armor?.KillHander(ev);
            Accessory?.KillHander(ev);
        }

        public void defenddeath(PlayerDeathEventArgs ev)
        {
            Weapon?.KilledHander(ev);
            Armor?.KilledHander(ev);
            Accessory?.KilledHander(ev);
        }

        public void ClearEquip()
        {
            Weapon=null;
            Armor = null;
            Accessory = null;
        }

        public static void ClearEquip(Player player)
        {
            Wanjia wj = Wanjia.Create(player);
            wj.Weapon = null;
            wj.Armor = null;
            wj.Accessory = null;
        }

        /// <summary>
        /// 私有构造函数，强制通过 Create 方法创建实例
        /// </summary>
        private Wanjia(Player owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// 为玩家创建 Wanjia 实例并注册映射
        /// </summary>
        public static Wanjia Create(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            // 如果已存在则返回现有实例
            if (_playerMap.TryGetValue(player, out Wanjia existing))
                return existing;

            // 创建新实例并注册
            var newWanjia = new Wanjia(player);

            if (!_playerMap.TryAdd(player, newWanjia))
            {
                // 添加失败时尝试再次获取
                return _playerMap[player];
            }

            return newWanjia;
        }


        /// <summary>
        /// 通过玩家获取对应的 Wanjia 实例
        /// </summary>
        public static Wanjia Get(Player player)
        {
            if (player == null)
                return null;

            if (_playerMap.TryGetValue(player, out Wanjia wanjia))
            {
                return wanjia;
            }

            // 如果不存在，自动创建新实例
            return Create(player);
        }

        /// <summary>
        /// 移除玩家的 Wanjia 实例
        /// </summary>
        public static void Remove(Player player)
        {
            if (player == null)
                return;

            if (_playerMap.TryRemove(player, out Wanjia wanjia))
            {
                // 清理资源
                wanjia.ClearEquipment();
            }
        }

        /// <summary>
        /// 清理所有 Wanjia 实例
        /// </summary>
        public static void ClearAll()
        {
            foreach (var wanjia in _playerMap.Values)
            {
                wanjia.ClearEquipment();
            }
            _playerMap.Clear();
        }

        /// <summary>
        /// 清理装备资源
        /// </summary>
        public void ClearEquipment()
        {
            Weapon = null;
            Armor = null;
            Accessory = null;
        }

        /// <summary>
        /// 获取所有活跃的 Wanjia 实例
        /// </summary>
        public static System.Collections.Generic.IEnumerable<Wanjia> GetAll()
        {
            return _playerMap.Values;
        }

        /// <summary>
        /// 当玩家离开游戏时调用此方法
        /// </summary>
        public static void OnPlayerLeft(Player player)
        {
            Remove(player);
        }

        public static string DisplayName(Wanjia wj)
        {
            return wj.Owner.Nickname+ '|'+ (wj?.Weapon?.Name ?? "") + '|'+ (wj?.Armor?.Name ??  "") + '|'+(wj?.Accessory?.Name ??  "");
        }

        public static bool ComparePlayerFaction(Player player1, Player player2)
        {
            return player1.Faction == player2.Faction || (((Wanjia.Create(player1).Accessory is 人类之心) && player2.IsHuman) || ((Wanjia.Create(player2).Accessory is 人类之心 && player1.IsHuman)));
        }

        public void ReloadFireArm(PlayerReloadedWeaponEventArgs ev)
        {
            Weapon?.ReloadFireArmHander(ev);
            Armor?.ReloadFireArmHander(ev);
            Accessory?.ReloadFireArmHander(ev);
        }

        public void ToggledWeaponFlashlight(PlayerToggledWeaponFlashlightEventArgs ev)
        {
            Accessory?.ToggledWeaponFlashlightHander(ev);
        }

        public void ToggledFlashlight(PlayerToggledFlashlightEventArgs ev)
        {
            Accessory?.ToggledFlashlightHander(ev);
        }
    }
}