using CustomItems.API;
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
using System.Xml.Linq;
using UnityEngine;
using static InventorySystem.Items.Usables.Scp330.Scp330Translations;

namespace CustomItems.API
{
    public class Wanjia
    {
        public HintUIManager hintUIManager; 
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

        public uint CanUseMedical = 0;

        public uint scpKillHealCD = 0;

        //出生保护时间，大于0是无敌状态
        public uint SpawnProtectedTime = 0;


        public static uint SCPCount = 0;

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


        public static void ClearEquipment(Player player)
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
            hintUIManager = new HintUIManager(this);
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
            if(player1 == null || player2 == null) return false;
            return player1.Faction == player2.Faction || (((Wanjia.Create(player1).Accessory is 人类之心) && player2.IsHuman) || ((Wanjia.Create(player2).Accessory is 人类之心 && player1.IsHuman)));
        }

        public void ReloadFireArm(PlayerReloadedWeaponEventArgs ev)
        {
            Weapon?.ReloadFireArmHander(ev);
            Armor?.ReloadFireArmHander(ev);
            Accessory?.ReloadFireArmHander(ev);
        }

        public void PickedUpedItemHander(PlayerPickedUpItemEventArgs ev)
        {
            Weapon?.PickupeditemHander(ev);
            Armor?.PickupeditemHander(ev);
            Accessory?.PickupeditemHander(ev);
        }



        public void ToggledWeaponFlashlight(PlayerToggledWeaponFlashlightEventArgs ev)
        {
            Accessory?.ToggledWeaponFlashlightHander(ev);
        }

        public void ToggledFlashlight(PlayerToggledFlashlightEventArgs ev)
        {
            Accessory?.ToggledFlashlightHander(ev);
        }

        public void shottingHanlde(PlayerShootingWeaponEventArgs ev)
        {
            Weapon?.shottingHanlde(ev);
            Armor?.shottingHanlde(ev);
            Accessory?.shottingHanlde(ev);
        }

        public void shottedHanlde(PlayerShotWeaponEventArgs ev)
        {
            Weapon?.shottedHanlde(ev);
            Armor?.shottedHanlde(ev);
            Accessory?.shottedHanlde(ev);
        }

        public void SelectedItemHander(PlayerChangedItemEventArgs ev)
        {
            Weapon?.SelectedItemHander(ev);
            Armor?.SelectedItemHander(ev);
            Accessory?.SelectedItemHander(ev);
        }
    }


    public class HintUIManager
    {
        private string currenthint="";
        // 聊天框配置
        private class ChatMessage
        {
            public string PlayerName { get; set; }
            public string Content { get; set; }
            public Faction Faction { get; set; }  // 新增阵营字段
        }
        private List<ChatMessage> chatMessages = new List<ChatMessage>();
        private const int MaxChatLines = 5;
        private const int MaxPlayerNameLength = 15;
        private const int MaxChatContentLength = 50;

        public uint chatshowtimer = 0;
        public uint pickupshowtimer = 0;

        // 装备拾取介绍
        private class EquipmentPickup
        {
            public string Type { get; set; } // "武器"/"防具"/"饰品"
            public string Name { get; set; }
            public string Description { get; set; }
        }
        private EquipmentPickup currentPickup;

        // 已装备武器介绍
        private const int MaxEquippedDescriptionLength = 60;

        public string WeaponDisplay
        {
            get
            {
                if (Wanjia.Weapon != null)
                {
                    return "武器:" + Wanjia.Weapon.Name + "——" + Wanjia.Weapon.Description;
                }
                else
                {
                    return "武器:未装备";
                }

            }
        }
        public string ArmorDisplay
        {
            get
            {
                if (Wanjia.Armor != null)
                {
                    return "防具:" + Wanjia.Armor.Name + '-' + Wanjia.Armor.Description;
                }
                else
                {
                    return "防具:未装备";
                }

            }
        }
        public string AccessoryDisplay
        {
            get
            {
                if (Wanjia.Accessory != null)
                {
                    return "饰品:" + Wanjia.Accessory.Name + '-' + Wanjia.Accessory.Description;
                }
                else
                {
                    return "饰品:未装备";
                }

            }
        }
        public HintUIManager(Wanjia wanjia)
        {
            Wanjia = wanjia;
        }

        public Wanjia Wanjia { get; }

        public void ShowPickupItem(CustomItem item)
        {
            string type;
            if (item is EquipmentBase equip)
            {

                switch (equip.EquipType)
                {
                    case EquipmentBase.EquipmentType.Weapon:
                        type = "武器";
                        break;
                    case EquipmentBase.EquipmentType.Armor:
                        type = "防具";
                        break;
                    case EquipmentBase.EquipmentType.Accessory:
                        type = "饰品";
                        break;
                    default:
                        type = "其他";
                        break;
                }
                currentPickup = new EquipmentPickup
                {
                    Type = type,
                    Name = item.Name,
                    Description = item.Description
                };
            }

        }

        // 添加聊天消息
        public bool AddChatMessageTeam(string playerName, string content, Faction senderFaction)
        {
            // 阵营验证（如果接收者阵营存在且不匹配则拒绝）
            if (senderFaction != Wanjia.Owner.Faction)          // 发送者阵营不匹配
            {
                return false; // 添加失败
            }

            // 新增：获取阵营后缀
            string GetFactionSuffix()
            {
                return senderFaction switch
                {
                    Faction.SCP => "[SCP]",
                    Faction.FoundationStaff => "[九尾]",
                    Faction.FoundationEnemy => "[混沌]",
                    Faction.Unclassified => "[观察者]",
                    Faction.Flamingos => "[火烈鸟]",
                    _ => ""
                };
            }

            // 处理玩家名（截断超长部分 + 添加阵营标识）
            string suffix = GetFactionSuffix();
            int nameLengthLimit = MaxPlayerNameLength - suffix.Length; // 预留阵营标识空间

            string rawName = playerName.Length > nameLengthLimit
                ? playerName.Substring(0, nameLengthLimit)
                : playerName;

            string displayName = $"{rawName}{suffix}";

            // 移除内容中的所有换行
            content = content.Replace("\\r", "").Replace("\\n", "");


            // 截断超长内容
            if (content.Length > MaxChatContentLength)
            {
                content = content.Substring(0, MaxChatContentLength);
            }

            // 添加到消息列表
            chatMessages.Add(new ChatMessage
            {
                PlayerName = displayName,
                Content = content,
                Faction = senderFaction
            });

            // 保持最多MaxChatLines条消息
            if (chatMessages.Count > MaxChatLines)
            {
                chatMessages.RemoveAt(0);
            }
            chatshowtimer = 10;
            return true;
        }

        public bool AddChatMessageAll(string playerName, string content, Faction senderFaction)
        {

            string suffix = "[全体]";
            int nameLengthLimit = MaxPlayerNameLength - suffix.Length; // 预留阵营标识空间

            string rawName = playerName.Length > nameLengthLimit
                ? playerName.Substring(0, nameLengthLimit)
                : playerName;

            string displayName = $"{rawName}{suffix}";

            // 移除内容中的所有换行
            content = content.Replace("\\r", "").Replace("\\n", "");


            // 截断超长内容
            if (content.Length > MaxChatContentLength)
            {
                content = content.Substring(0, MaxChatContentLength);
            }

            // 添加到消息列表
            chatMessages.Add(new ChatMessage
            {
                PlayerName = displayName,
                Content = content,
                Faction = senderFaction
            });

            // 保持最多MaxChatLines条消息
            if (chatMessages.Count > MaxChatLines)
            {
                chatMessages.RemoveAt(0);
            }
            chatshowtimer = 10;
            return true;
        }

        // 设置装备拾取信息
        public void SetEquipmentPickup(CustomItem item)
        {
            if (item is EquipmentBase equip)
            {
                string type;
                switch (equip.EquipType)
                {
                    case EquipmentBase.EquipmentType.Weapon:
                        type = " 武器";
                        break;
                    case EquipmentBase.EquipmentType.Armor:
                        type = "防具";
                        break;
                    case EquipmentBase.EquipmentType.Accessory:
                        type = "饰品";
                        break;
                    default:
                        type = "其他";
                        break;
                }
                currentPickup = new EquipmentPickup
                {
                    Type = type,
                    Name = item.Name,
                    Description = item.Description,
                };
                pickupshowtimer = 15;
            }

        }


        // 生成富文本Hint
        public string GenerateHintText()
        {
            StringBuilder sb = new StringBuilder();

            // 使用换行符维持结构（替代原来的行）
            sb.Append("\n\n\n"); // 顶部空白

            //
            int linecount = 5;

            if(chatshowtimer>0)
            {
                sb.Append("<align=\"left\"><size=47.5%>");
                foreach (var msg in chatMessages)
                {
                    string color;
                    switch (msg.Faction)
                    {
                        case Faction.SCP:
                            color = "red";
                            break;
                        case Faction.FoundationStaff:
                            color = "blue";
                            break;
                        case Faction.FoundationEnemy:
                            color = "green";
                            break;
                        case Faction.Flamingos:
                            color = "#FFB6C1";
                            break;
                        default:
                            color = "white";
                            break;
                    }
                    sb.Append($"<color=\"{color}\">{msg.PlayerName}：{msg.Content}</color>\n");
                    linecount--;
                }
            }
            

            sb.Append("</size></align>");
            // 补足空行确保5行高度
            sb.Append("<align=\"left\"><size=60%>");
            for (int i = 0; i < linecount; i++)
            {
                sb.Append("\n");
            }
            sb.Append("</size></align>");

            // 中间空白区域
            sb.Append("\n\n\n\n\n\n\n\n\n\n"); // 10行空白

            linecount = 5;
            // 装备拾取介绍部分
            if (currentPickup != null && pickupshowtimer > 0)
            {
                sb.Append("<size=78.5%>");

                // 拾取提示行
                sb.Append($"你选中了{currentPickup.Type}：{currentPickup.Name}\n");
                linecount--;
                // 装备描述（每行最多40字）
                if (!string.IsNullOrEmpty(currentPickup.Description))
                {
                    const int maxLineLength = 40;
                    string desc = currentPickup.Description;

                    for (int i = 0; i < desc.Length; i += maxLineLength)
                    {
                        int length = Math.Min(maxLineLength, desc.Length - i);
                        sb.AppendLine(desc.Substring(i, length));
                        linecount--;
                    }
                }

                sb.Append("</size>");
            }
            for(int i=0;i< linecount;i++)
            {
                sb.AppendLine("");
            }

            // 底部空白区域
            sb.Append("\n\n\n"); // 3行空白

            // 已装备物品部分
            sb.Append("<align=\"left\"><size=60%>");

            sb.AppendLine($"\t{WeaponDisplay}");
            sb.AppendLine($"\t{ArmorDisplay}");
            sb.AppendLine($"\t{AccessoryDisplay}");

            sb.Append("</size></align>");

            // 末尾空白（确保UI底部留白）
            sb.Append("\n\n\n\n\n\n\n"); // 7行空白

            return sb.ToString();
        }

        // 更新玩家UI
        public void UpdateHint()
        {
            if (pickupshowtimer > 0)
            {
                --pickupshowtimer;
            }
            if (chatshowtimer > 0)
            {
                --chatshowtimer;
            }
            string newhint = GenerateHintText();
            if(newhint != currenthint)
            {
                currenthint = newhint;
                Wanjia.Owner.SendHint(newhint, 99999);
            }
        }
    }
    // 添加聊天消息
}


