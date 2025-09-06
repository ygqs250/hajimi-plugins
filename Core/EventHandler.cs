using LabApi.Events.CustomHandlers;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using CustomItems.API;
using UnityEngine;
using Mirror;
using PlayerStatsSystem;
using PlayerRoles.PlayableScps.Scp939;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.PlayableScps.Scp1507;
using LabApi.Events.Arguments.ServerEvents;
using System.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using RemoteAdmin.Communication;
using System.Threading.Tasks;
using LabApi.Events.Arguments.Scp173Events;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using System.Reflection;
using PlayerRoles.FirstPersonControl;

using PlayerRoles.Visibility;
using PlayerRoles.PlayableScps.Scp106;
using Respawning.Waves;
using static HarmonyLib.Code;
using CustomPlayerEffects;
using System.Linq.Expressions;
using PlayerRoles;
using InventorySystem.Items.Usables;
using System.Collections;
using MEC;
using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Arguments.WarheadEvents;
using System.Diagnostics;
using System.Threading;
using Timer = System.Timers.Timer;
using Respawning.Config;

namespace CustomItems.Core;

internal class EventHandler : CustomEventsHandler
{
    static Stopwatch stopwatch;

    // 创建定时器（间隔1000毫秒 = 1秒）
    Timer timer = new Timer(1000);
    private IEnumerator<float> latersetKinematic(Pickup pickup)
    {
        yield return Timing.WaitForSeconds(15f);
        if (pickup != null)
            pickup.Rigidbody.isKinematic = true;
    }

    public override void OnServerWaitingForPlayers()
    {
        API.CustomItems.CurrentItems.Clear();

        //生成抽奖机
        //九尾出生点
        API.CustomItems.TrySpawn("抽奖机", new Vector3(110.84f,295.69f,-34.67f), out var pickup);
        NetworkServer.UnSpawn(pickup.Base.gameObject);
        pickup.Base.gameObject.transform.localScale = Vector3.one * 8;
        Timing.RunCoroutine(latersetKinematic(pickup));
        //ev.Pickup.Rigidbody.detectCollisions = false;
        NetworkServer.Spawn(pickup.Base.gameObject);
        //混沌出生点
        API.CustomItems.TrySpawn("抽奖机", new Vector3(25.68f, 291.88f, -36.47f), out var pickup1);
        NetworkServer.UnSpawn(pickup1.Base.gameObject);
        pickup1.Base.gameObject.transform.localScale = Vector3.one * 8;
        Timing.RunCoroutine(latersetKinematic(pickup1));
        //ev.Pickup.Rigidbody.detectCollisions = false;
        NetworkServer.Spawn(pickup1.Base.gameObject);

        //其他位置
        foreach (var room in Room.List)
        {
            if (room.IsDestroyed) continue;
            if(room.Zone == MapGeneration.FacilityZone.LightContainment && room.Shape == MapGeneration.RoomShape.XShape)
            {
                Log.Info(room.Base.WorldspaceBounds.center.ToString());
                Vector3 spwanpoint = new(room.Base.WorldspaceBounds.center.x, room.Base.WorldspaceBounds.center.y-2, room.Base.WorldspaceBounds.center.z);
                API.CustomItems.TrySpawn("抽奖机", spwanpoint, out var pickuptest);
                NetworkServer.UnSpawn(pickuptest.Base.gameObject);
                pickuptest.Base.gameObject.transform.localScale = Vector3.one * 5;
                Timing.RunCoroutine(latersetKinematic(pickuptest));
                //ev.Pickup.Rigidbody.detectCollisions = false;
                NetworkServer.Spawn(pickuptest.Base.gameObject);
                break;
            }
        }


        Room heavyroom=null;
        foreach (var room in Room.List)
        {
            if (room.IsDestroyed) continue;
            if (room.Zone == MapGeneration.FacilityZone.HeavyContainment && room.Shape == MapGeneration.RoomShape.XShape)
            {
                heavyroom = room;
            }

        }
        if (heavyroom != null)
        {
            Log.Info("heavy" + heavyroom.Base.WorldspaceBounds.center.ToString());
            Vector3 spwanpoint = new(heavyroom.Base.WorldspaceBounds.center.x, heavyroom.Base.WorldspaceBounds.center.y - 2, heavyroom.Base.WorldspaceBounds.center.z);
            API.CustomItems.TrySpawn("抽奖机", spwanpoint, out var pickuptest);
            NetworkServer.UnSpawn(pickuptest.Base.gameObject);
            pickuptest.Base.gameObject.transform.localScale = Vector3.one * 5;
            Timing.RunCoroutine(latersetKinematic(pickuptest));
            //ev.Pickup.Rigidbody.detectCollisions = false;
            NetworkServer.Spawn(pickuptest.Base.gameObject);
        }

        foreach (var room in Room.List)
        {
            if (room.IsDestroyed) continue;
            if (room.Zone == MapGeneration.FacilityZone.Entrance && room.Shape == MapGeneration.RoomShape.XShape)
            {
                Log.Info(room.Base.WorldspaceBounds.center.ToString());
                Vector3 spwanpoint = new(room.Base.WorldspaceBounds.center.x, room.Base.WorldspaceBounds.center.y - 2, room.Base.WorldspaceBounds.center.z);
                API.CustomItems.TrySpawn("抽奖机", spwanpoint, out var pickuptest);
                NetworkServer.UnSpawn(pickuptest.Base.gameObject);
                pickuptest.Base.gameObject.transform.localScale = Vector3.one * 5;
                Timing.RunCoroutine(latersetKinematic(pickuptest));
                //ev.Pickup.Rigidbody.detectCollisions = false;
                NetworkServer.Spawn(pickuptest.Base.gameObject);
                break;
            }
        }

        if (!CustomItemsPlugin.Instance.Config.TestItemSpawning) return;
        if (API.CustomItems.AllItems.Count == 0)
        {
            Log.Error("No custom items registered, cannot spawn test items.");
            return;
        }

        foreach (var room in Room.List)
        {
            if (room.IsDestroyed) continue;
            API.CustomItems.TrySpawn(0, API.CustomItems.GetRandomPositionInRoom(room), out var pickuptest);
        }
    }

    public override void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev)
    {
        if (ev.Door is CheckpointDoor || ev.Door.DoorName == LabApi.Features.Enums.DoorName.EzGateB || ev.Door.DoorName == LabApi.Features.Enums.DoorName.EzGateA)
        {
            ev.CanOpen = true;
            return;
        }
        //Log.Info(ev.Door.ToString()+ev.CanOpen.ToString()+ev.Door.Permissions.ToString());
        //KeycardItem
        DoorPermissionFlags playerperm= DoorPermissionFlags.None;
        ev.Player.Items.ToList().ForEach(item =>
        {
            if (item is KeycardItem keycard) // 检测是否为钥匙卡
            {
                playerperm |= keycard.Permissions; // 合并权限
            }
        });

        if((playerperm | ev.Door.Permissions) == playerperm)
        {
            ev.CanOpen = true;
        }
    }


    public override void OnWarheadStarting(WarheadStartingEventArgs ev)
    {
        //Warhead.BaseController.InevitableTime = 0;
        //Warhead.BaseController.Info.ScenarioType = WarheadScenarioType.Start;
        Log.Info($"{nameof(OnWarheadStarting)} triggered by {ev.Player.UserId}|{ev.WarheadState.ScenarioType}|{ev.IsAllowed}");
    }

    public override void OnWarheadStarted(WarheadStartedEventArgs ev)
    {
        //Log.Info($"{nameof(OnWarheadStarted)} triggered by {ev.Player.UserId}{ev.WarheadState.StartTime}");
    }
    public override void OnWarheadStopping(WarheadStoppingEventArgs ev)
    {
        //Log.Info($"{nameof(OnWarheadStopping)} triggered by {ev.Player.UserId}|{ev.WarheadState.StartTime} ");
    }

    public override void OnWarheadStopped(WarheadStoppedEventArgs ev)
    {
        Warhead.BaseController.Info.ScenarioType = WarheadScenarioType.Start;
        Log.Info($"{nameof(OnWarheadStopped)} triggered by {ev.Player.UserId} |{ev.WarheadState.StartTime}");
    }
    public override void OnPlayerInteractedDoor(PlayerInteractedDoorEventArgs ev)
    {

    }
    public override void OnPlayerInteractingGenerator(PlayerInteractingGeneratorEventArgs ev)
    {
        //Log.Info(ev.Player.ToString() + ev.Generator.ToString() + ev.IsAllowed.ToString());
    }

    public override void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
    {
        //if (ev.Player.ReferenceHub.roleManager.CurrentRole is Scp173Role scp173Role)
        //{
        //    foreach (SubroutineBase sub in scp173Role.SubroutineModule.AllSubroutines)
        //    {
        //        if (sub is Scp173BlinkTimer blinkTimer)
        //        {
        //            //blinkTimer._initialStopTime = NetworkTime.time;
        //            //blinkTimer._totalCooldown = 1;
        //            //blinkTimer.ServerSendRpc(true);

        //            try
        //            {
        //                // 通过反射修改私有字段
        //                var type = typeof(Scp173BlinkTimer);

        //                // 修改 _initialStopTime
        //                var initialStopTimeField = type.GetField("_initialStopTime",
        //                    System.Reflection.BindingFlags.NonPublic |
        //                    System.Reflection.BindingFlags.Instance);
        //                initialStopTimeField?.SetValue(blinkTimer, NetworkTime.time);

        //                // 修改 _totalCooldown
        //                var totalCooldownField = type.GetField("_totalCooldown",
        //                    System.Reflection.BindingFlags.NonPublic |
        //                    System.Reflection.BindingFlags.Instance);
        //                totalCooldownField?.SetValue(blinkTimer, 1f);

        //                // 通过反射调用保护方法
        //                var serverSendRpcMethod = type.GetMethod(
        //                    "ServerSendRpc",
        //                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
        //                    null,
        //                    new[] { typeof(bool) }, // 参数类型
        //                    null
        //                );
        //                serverSendRpcMethod?.Invoke(blinkTimer, new object[] { true });
        //            }
        //            catch (Exception ex)
        //            {
        //                Log.Error($"反射调用失败: {ex}");
        //            }
        //        }
        //    }
                
        //}
    }
    //public override void OnScp173BreakneckSpeedChanging(Scp173BreakneckSpeedChangingEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173BreakneckSpeedChanging)} triggered by {ev.Player.UserId}");
    //}

    //public override void OnScp173BreakneckSpeedChanged(Scp173BreakneckSpeedChangedEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173BreakneckSpeedChanged)} triggered by {ev.Player.UserId}");
    //}

    //public override void OnScp173AddedObserver(Scp173AddedObserverEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173AddedObserver)} triggered by {ev.Player.UserId}");
    //}

    //public override void OnScp173RemovingObserver(Scp173RemovingObserverEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173RemovingObserver)} triggered by {ev.Player.UserId}");
    //}

    //public override void OnScp173RemovedObserver(Scp173RemovedObserverEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173RemovedObserver)} triggered by {ev.Player.UserId}");
    //}

    //public override void OnScp173CreatingTantrum(Scp173CreatingTantrumEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173CreatingTantrum)} triggered by {ev.Player.UserId}");
    //}

    //public override void OnScp173CreatedTantrum(Scp173CreatedTantrumEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173CreatedTantrum)} triggered by {ev.Player.UserId}");
    //}

    public override void OnScp173PlayingSound(Scp173PlayingSoundEventArgs ev)
    {
        if (ev.Player.ReferenceHub.roleManager.CurrentRole is Scp173Role scp173Role)
        {
            float blinkTime = 3.0f * (ev.Player.Health / ev.Player.MaxHealth);
            foreach (SubroutineBase sub in scp173Role.SubroutineModule.AllSubroutines)
            {
                if (sub is Scp173BlinkTimer blinkTimer)
                {
                    //blinkTimer._initialStopTime = NetworkTime.time;
                    //blinkTimer._totalCooldown = 1;
                    //blinkTimer.ServerSendRpc(true);

                    try
                    {
                        // 通过反射修改私有字段
                        var type = typeof(Scp173BlinkTimer);

                        // 修改 _initialStopTime
                        var initialStopTimeField = type.GetField("_initialStopTime",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance);
                        initialStopTimeField?.SetValue(blinkTimer, NetworkTime.time);

                        // 修改 _totalCooldown
                        var totalCooldownField = type.GetField("_totalCooldown",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance);
                        totalCooldownField?.SetValue(blinkTimer, blinkTime);

                        // 通过反射调用保护方法
                        var serverSendRpcMethod = type.GetMethod(
                            "ServerSendRpc",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                            null,
                            new[] { typeof(bool) }, // 参数类型
                            null
                        );
                        serverSendRpcMethod?.Invoke(blinkTimer, new object[] { true });
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"反射调用失败: {ex}");
                    }
                }
            }
        }
    }

    //public override void OnScp173PlayedSound(Scp173PlayedSoundEventArgs ev)
    //{
    //    Log.Info($"{nameof(OnScp173PlayedSound)} triggered by {ev.Player.UserId}");
    //}

    private bool Check(ushort serial)
    {
        return API.CustomItems.CurrentItems.ContainsKey(serial);
    }

    #region Item Using

    public override void OnPlayerFlippedCoin(PlayerFlippedCoinEventArgs ev)
    {
        if (!Check(ev.CoinItem.Serial)) return;
        API.CustomItems.CurrentItems[ev.CoinItem.Serial].OnFlippedCoin(ev);
    }
    public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
    {
        if (!Check(ev.UsableItem.Serial)) return;
        API.CustomItems.CurrentItems[ev.UsableItem.Serial].OnUsing(ev);

        if( ev.UsableItem.Category == ItemCategory.Medical && Wanjia.Create(ev.Player).CanUseMedical>0 )
        {
            ev.IsAllowed = false;
        }
    }
    public override void OnPlayerUsedItem(PlayerUsedItemEventArgs ev)
    {
        if (!Check(ev.UsableItem.Serial)) return;
        API.CustomItems.CurrentItems[ev.UsableItem.Serial].OnUsed(ev);
    }
    #endregion

    #region Item Dropping

    public override void OnPlayerDroppingAmmo(PlayerDroppingAmmoEventArgs ev)
    {
        ev.IsAllowed = false;
    }
    public override void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev)
    {
        if (!Check(ev.Item.Serial)) return;
        API.CustomItems.CurrentItems[ev.Item.Serial].OnDropping(ev);
        var customItem = API.CustomItems.CurrentItems[ev.Item.Serial];
        ev.Player.SendBroadcast($"你丢下了{customItem.Name}",3);
    }
    public override void OnPlayerDroppedItem(PlayerDroppedItemEventArgs ev)
    {
        try
        {
            if (!Check(ev.Pickup.Serial)) return;
            API.CustomItems.CurrentItems[ev.Pickup.Serial].OnDropped(ev);
        }
        catch (NullReferenceException ex)
        {

            Log.Error("OnPlayerDroppedItem:" + ex.Source.ToString());
        }

    }
    #endregion

    #region Item Picking Up
    public override void OnPlayerPickingUpItem(PlayerPickingUpItemEventArgs ev)
    {
        if (!Check(ev.Pickup.Serial)) return;
        API.CustomItems.CurrentItems[ev.Pickup.Serial].OnPickingUp(ev);
        //NetworkServer.UnSpawn(ev.Pickup.GameObject);
        //ev.Pickup.GameObject.transform.localScale = Vector3.one * 4;
        //NetworkServer.Spawn(ev.Pickup.GameObject);
    }
    public override void OnPlayerPickedUpItem(PlayerPickedUpItemEventArgs ev)
    {
        Wanjia wj = Wanjia.Create(ev.Player);
        wj.PickedUpedItemHander(ev);
        if (!Check(ev.Item.Serial)) return;
        var customItem = API.CustomItems.CurrentItems[ev.Item.Serial];
        customItem.OnPickedUp(ev);

        if (!customItem.ShowItemHints || !customItem.ShowPickupHints) return;
        //ev.Player.SendHint($"装备名称:{customItem.Name}\n效果:{customItem.Description}");
        //wj.hintUIManager.SetEquipmentPickup(customItem);
        //ev.Player.SendBroadcast($"你捡起来了{customItem.Name}",5);

        //NetworkServer.UnSpawn(ev.Item.GameObject);
        //ev.Item.GameObject.transform.localScale = Vector3.one * 200;
        //NetworkServer.Spawn(ev.Item.GameObject);
    }
    #endregion

    #region Item Selection
    public override void OnPlayerChangingItem(PlayerChangingItemEventArgs ev)
    {
        if (ev.OldItem != null && !ev.OldItem.IsDestroyed && ev.OldItem.GameObject != null && Check(ev.OldItem.Serial))
        {
            API.CustomItems.CurrentItems[ev.OldItem.Serial].OnUnselecting(ev);
            if (!ev.IsAllowed) return;
        }
        if (ev.NewItem == null || ev.NewItem.IsDestroyed || ev.NewItem.GameObject == null || !Check(ev.NewItem.Serial)) return;
        API.CustomItems.CurrentItems[ev.NewItem.Serial].OnSelecting(ev);
    }
    public override void OnPlayerChangedItem(PlayerChangedItemEventArgs ev)
    {
        Wanjia wj = Wanjia.Create(ev.Player);
        wj.SelectedItemHander(ev);
        if (ev.OldItem != null && !ev.OldItem.IsDestroyed && Check(ev.OldItem.Serial))
        {
            API.CustomItems.CurrentItems[ev.OldItem.Serial].OnUnselected(ev);
        }
        if (ev.NewItem == null || ev.NewItem.IsDestroyed || !Check(ev.NewItem.Serial)) return;
        var customItem = API.CustomItems.CurrentItems[ev.NewItem.Serial];
        customItem.OnSelected(ev);
        if (!customItem.ShowItemHints || !customItem.ShowSelectedHints) return;
        //ev.Player.SendHint($"你选中了{customItem.Type}: {customItem.Name}\n装备效果:{customItem.Description}",8);
        
        wj.hintUIManager.SetEquipmentPickup(customItem);
    }
    #endregion



    public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        Wanjia joiner = Wanjia.Create(ev.Player);
        Log.Info("玩家创建成功：" + ev.Player.Nickname);
        ev.Player.DisplayName = Wanjia.DisplayName(joiner);
        Log.Info("改名成功：" + ev.Player.Nickname);

        //StatusEffectBase[] allEffects = ev.Player.ReferenceHub.playerEffectsController.AllEffects;
        //foreach (var effect in allEffects)
        //{
        //    Log.Info(effect.ToString());
        //}
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        Wanjia.Remove(ev.Player);
        Log.Info("玩家移除成功：" + ev.Player.Nickname);
    }

    static ChaosSpawnWave chaosWave;

    public override void OnServerRoundRestarted()
    {
        timer.Elapsed -= OnTimerElapsed;
        抽奖机.clear();
    }


    public override void OnServerRoundStarting(RoundStartingEventArgs ev)
    {
        //stopwatch.Start();
        stopwatch = Stopwatch.StartNew();

        // 绑定定时器事件
        timer.Elapsed += OnTimerElapsed;

        // 设置定时器属性
        timer.AutoReset = true; // 设置为重复触发（默认为true）
        timer.Enabled = true;   // 启用定时器

        Log.Info("定时器已启动，每秒执行一次任务...");


        List<SpawnableWaveBase> waves = WaveSetting.getWaves();
        foreach (SpawnableWaveBase wave in waves)
        {
            if (wave is NtfSpawnWave timeBasedWave)
            {
                timeBasedWave.Timer.DefaultSpawnInterval = 120;
                timeBasedWave.Timer.SpawnIntervalSeconds = 120;
                timeBasedWave.RespawnTokens = 99;
                if (timeBasedWave.Configuration is PrimaryWaveConfig<NtfSpawnWave> primaryWaveConfig)
                {
                    primaryWaveConfig.SizePercentage = 1;
                }
               
            }
            else if (wave is ChaosSpawnWave chaosSpawnWave) {
                chaosWave = chaosSpawnWave;
                chaosSpawnWave.Timer.DefaultSpawnInterval = 120;
                chaosSpawnWave.Timer.SpawnIntervalSeconds = 120;
                chaosSpawnWave.RespawnTokens = 99;
                if (chaosSpawnWave.Configuration is PrimaryWaveConfig<NtfSpawnWave> primaryWaveConfig)
                {
                    primaryWaveConfig.SizePercentage = 1;
                }
            } else if(wave is TimeBasedWave timeBasedWave1)
            {
                timeBasedWave1.Timer.DefaultSpawnInterval = 9999;
                timeBasedWave1.Timer.SpawnIntervalSeconds = 9999;
            }
        }

        Server.CategoryLimits[ItemCategory.SpecialWeapon] = 8;
        Server.CategoryLimits[ItemCategory.Keycard] = 8;
        Server.CategoryLimits[ItemCategory.Firearm] = 8;
        Server.CategoryLimits[ItemCategory.Radio] = 8;
        Server.CategoryLimits[ItemCategory.Medical] = 8;
        Server.CategoryLimits[ItemCategory.Grenade] = 8;
        Server.CategoryLimits[ItemCategory.SCPItem] = 8;
    }


    //服务器插件的定时器，非常重要，1s执行一次
    private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        TimeSpan elapsed = stopwatch.Elapsed;
        Server.PlayerListName = $"<size=300%><align=\"center\"><color=red>哈基米服务器1服 QQ群:913477784</color> \\n <size=150%><align=\"center\"><color=blue>开局时间：{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}</color>";
        List<Player> players = Player.ReadyList.ToList<Player>();

        for (int i = 0; i < players.Count; i++)
        {
            Wanjia wj = Wanjia.Get(players[i]);
            wj.hintUIManager.UpdateHint();
            if (players[i].IsAlive)
            {
                if(wj.SpawnProtectedTime > 0)
                {
                    wj.SpawnProtectedTime--;
                }
                
                players[i].DisplayName = Wanjia.DisplayName(wj);
                wj.SufferTimer++;
                wj.DisengageTimer++;
                wj.AttactTimer++;
                wj?.Weapon?.EffectHander(wj);
                wj?.Armor?.EffectHander(wj);
                wj?.Accessory?.EffectHander(wj);

                Wanjia.SCPCount = 0;

                if (players[i].IsSCP)
                {
                    Wanjia.SCPCount++;
                    if (wj.lastPostion == players[i].Position)
                    {
                        wj.moveTimer++;
                    }
                    else
                    {
                        wj.moveTimer  = 0;
                    }
                    if (wj.moveTimer > 10 && wj.DisengageTimer > 10)
                    {
                        float healthstatus = players[i].Health / players[i].MaxHealth;
                        healthstatus *= 5;
                        switch ((int)healthstatus)
                        {
                            case 0:
                                players[i].Heal(70);
                                break;
                            case 1:
                                players[i].Heal(40);
                                break;
                            case 2:
                                players[i].Heal(30);
                                break;
                            case 3:
                                players[i].Heal(20);
                                break;
                            default:
                                players[i].Heal(20);
                                break;
                        }
                    }
                    wj.lastPostion = players[i].Position;

                    if (wj.scpKillHealCD > 0)
                        {
                        wj.scpKillHealCD--;
                        }
                }
            }

            if(Player.Count < 15)
            {
                if(chaosWave != null)
                chaosWave.RespawnTokens = 0;
            }else
            {
                if (chaosWave != null)
                    chaosWave.RespawnTokens = 99;
            }

        }


        if(elapsed.TotalSeconds % 360 == 0)
        {
            Timing.RunCoroutine(ClearPickup());
            foreach (var player in Player.ReadyList.ToList<Player>())
            {
                player.SendBroadcast("10秒后清理物品",10);
                Server.SendBroadcast("10秒后清理物品", 10, shouldClearPrevious: true);
            }
        }

        //List<WaveTimer> waves = WaveSetting.getRimeBasedWave();
        //foreach (WaveTimer wave in waves)
        //{
        //    Log.Info(wave.ToString()+":"+ wave.DefaultSpawnInterval+":" + wave.SpawnIntervalSeconds + ":"+wave.TimePassed);
        //}
        //Log.Info("循环执行了一次");
    }

    static private IEnumerator<float> ClearPickup()
    {
        yield return Timing.WaitForSeconds(10f);
        Maid.clean();
    }

    public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        if (ev.DamageHandler is Scp049DamageHandler scp049)
        {
            if(scp049.DamageSubType == Scp049DamageHandler.AttackType.CardiacArrest)
            {
                Wanjia.ClearEquipment(ev.Player);
                ev.Player.SetRole(PlayerRoles.RoleTypeId.Scp0492);
                return;
            }
        } 
        else if (ev.Attacker != null && (ev.Attacker.Role == PlayerRoles.RoleTypeId.Scp173 || ev.Attacker.Role == PlayerRoles.RoleTypeId.Scp096))
        {
            if (ev.DamageHandler is StandardDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
                st.Damage = 9527;
            }
        }
        else if (ev.DamageHandler is Scp939DamageHandler scp939)
        {
            if (scp939.Scp939DamageType == Scp939DamageType.Claw)
            {
                scp939.Damage = 50;
            }
        }
        else if (ev.Attacker != null &&  ev.Attacker.Role == PlayerRoles.RoleTypeId.Scp106)
        {
            if (!PocketDimension.IsPlayerInside(ev.Player))
                PocketDimension.ForceInside(ev.Player);
        }


        if (ev.Attacker != null)
        {
            Wanjia attacter = Wanjia.Create(ev?.Attacker);
            if (!Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
            {
                attacter.AttactTimer = 0;
                attacter.DisengageTimer = 0;
            }

            attacter.attack(ev);
        }
        if (ev.Player != null)
        {
            Wanjia suffer = Wanjia.Create(ev?.Player);
            //出生保护
            if(suffer.SpawnProtectedTime > 0)
            {
                if(ev.DamageHandler is StandardDamageHandler standardDamageHandler)
                {
                    standardDamageHandler.Damage = 0;
                }
            }

            if (!Wanjia.ComparePlayerFaction(ev.Player, ev.Attacker))
            {
                suffer.SufferTimer = 0;
                suffer.DisengageTimer = 0;
            }
            suffer.defend(ev);
        }
    }


    public override void OnPlayerDying(PlayerDyingEventArgs ev)
    {
        if (ev.Attacker != null)
        {
            Wanjia attacter = Wanjia.Create(ev?.Attacker);
            attacter.AttactTimer = 0;
            attacter.attackdying( ev);
        }
        if (ev.Player != null)
        {
            Wanjia suffer = Wanjia.Create(ev?.Player);
            suffer.SufferTimer = 0;
            suffer.defenddying( ev);
            //suffer.deathposition = ev.Player.Position;
        }
        //TimedGrenadeProjectile.SpawnActive(ev.Player.Position, ItemType.GrenadeHE, ev.Player);
        
    }

    public override void OnPlayerShootingWeapon(PlayerShootingWeaponEventArgs ev)
    {
        if (ev.Player != null)
        {
            Wanjia suffer = Wanjia.Create(ev?.Player);
            suffer.shottingHanlde(ev);
        }
    }

    public override void OnPlayerShotWeapon(PlayerShotWeaponEventArgs ev)
    {
        if (ev.Player != null)
        {
            Wanjia suffer = Wanjia.Create(ev?.Player);
            suffer.shottedHanlde(ev);
        }
    }

    public override void OnPlayerReloadingWeapon(PlayerReloadingWeaponEventArgs ev)
    {
        if(ev.FirearmItem is Scp127Firearm)
            return;
        ev.Player.SetAmmo(ev.FirearmItem.AmmoType, 1000);
    }

    public override void OnPlayerReloadedWeapon(PlayerReloadedWeaponEventArgs ev)
    {
        if (ev.FirearmItem is Scp127Firearm)
            return;
        Wanjia wj = Wanjia.Create(ev.Player);
        wj.ReloadFireArm(ev);
    }
    //public override void OnPlayerSpawning(PlayerSpawningEventArgs ev)
    //{

    //}
    public override void OnPlayerSpawned(PlayerSpawnedEventArgs ev)
    {
        
    }
    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
    {
        if (ev.NewRole == RoleTypeId.Scp079)
        {
            ev.NewRole = RoleTypeId.Scp049;
            int ran = Wanjia.rand.Next(0, 5);
            switch (ran)
            {
                case (0):
                    ev.NewRole = RoleTypeId.Scp049;
                    break;
                case (1):
                    ev.NewRole = RoleTypeId.Scp939;
                    break;
                case (2):
                    ev.NewRole = RoleTypeId.Scp173;
                    break;
                case (3):
                    ev.NewRole = RoleTypeId.Scp096;
                    break;
                case (4):
                    ev.NewRole = RoleTypeId.Scp106;
                    break;
                default:
                    break;
            }
        }

    }
    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
    {
        Wanjia wj = Wanjia.Create(ev.Player);

        if (ev.Player.Faction == Faction.FoundationStaff || ev.Player.Faction == Faction.FoundationEnemy)
        {
            ev.Player.ClearItems();

            if (ev.Player == null) return;

            //GiveItemsAfterDelay(ev.Player);
            //StartCoroutine();
            Timing.RunCoroutine(GiveItemsAfterDelay(ev.Player));
        }

        if (ev.ChangeReason == RoleChangeReason.RemoteAdmin || ev.ChangeReason == RoleChangeReason.RespawnMiniwave)
        {
            wj.ClearEquipment();
        }

        switch (ev.Player.Role)
        {
            case RoleTypeId.None:
                break;
            case RoleTypeId.Scp173:
                ev.Player.MaxHealth = 7173;
                ev.Player.Health = 7173;
                Wanjia.Create(ev.Player).Accessory = new API.Equipment.花生之怒();
                Wanjia.Create(ev.Player).Armor = new API.Equipment.皮糙肉厚();
                break;
            case RoleTypeId.ClassD:
                break;
            case RoleTypeId.Spectator:
                break;
            case RoleTypeId.Scp106:
                ev.Player.MaxHealth = 5106;
                ev.Player.Health = 5109;
                Wanjia.Create(ev.Player).Accessory = new API.Equipment.神奇口袋();
                Wanjia.Create(ev.Player).Armor = new API.Equipment.皮糙肉厚();
                break;
            case RoleTypeId.NtfSpecialist:
                break;
            case RoleTypeId.Scp049:
                ev.Player.MaxHealth = 4449;
                ev.Player.Health = 4449;
                Wanjia.Create(ev.Player).Accessory = new API.Equipment.感染手套();
                Wanjia.Create(ev.Player).Armor = new API.Equipment.皮糙肉厚();
                break;
            case RoleTypeId.Scientist:
                break;
            case RoleTypeId.Scp079:
                break;
            case RoleTypeId.ChaosConscript:
                break;
            case RoleTypeId.Scp096:
                ev.Player.MaxHealth = 5096;
                ev.Player.Health = 5096;
                Wanjia.Create(ev.Player).Accessory = new API.Equipment.痛肘();
                Wanjia.Create(ev.Player).Armor = new API.Equipment.皮糙肉厚();
                break;
            case RoleTypeId.Scp0492:
                Wanjia.Create(ev.Player).Accessory = new API.Equipment.隐形的翅膀();
                break;
            case RoleTypeId.NtfSergeant:
                break;
            case RoleTypeId.NtfCaptain:
                break;
            case RoleTypeId.NtfPrivate:
                break;
            case RoleTypeId.Tutorial:
                break;
            case RoleTypeId.FacilityGuard:
                break;
            case RoleTypeId.Scp939:
                ev.Player.MaxHealth = 4939;
                ev.Player.Health = 4939;
                Wanjia.Create(ev.Player).Armor = new API.Equipment.皮糙肉厚();
                break;
            case RoleTypeId.CustomRole:
                break;
            case RoleTypeId.ChaosRifleman:
                break;
            case RoleTypeId.ChaosMarauder:
                break;
            case RoleTypeId.ChaosRepressor:
                break;
            case RoleTypeId.Overwatch:
                break;
            case RoleTypeId.Filmmaker:
                break;
            case RoleTypeId.Scp3114:
                break;
            case RoleTypeId.Destroyed:
                break;
            case RoleTypeId.Flamingo:
                break;
            case RoleTypeId.AlphaFlamingo:
                break;
            case RoleTypeId.ZombieFlamingo:
                break;
            default:
                break;
        }



        ev.Player.SetAmmo(ItemType.Ammo12gauge, 1000);
            ev.Player.SetAmmo(ItemType.Ammo44cal, 1000);
            ev.Player.SetAmmo(ItemType.Ammo9x19, 1000);
            ev.Player.SetAmmo(ItemType.Ammo762x39, 1000);
            ev.Player.SetAmmo(ItemType.Ammo556x45, 1000);


    }

    // 定义协程方法
    private IEnumerator<float> GiveItemsAfterDelay(Player player)
    {
        yield return Timing.WaitForSeconds(0.5f);

        if (player != null)
        {
            switch (player.Role)
            {
                case RoleTypeId.NtfSergeant:
                    switch (Wanjia.rand.Next(0, 2))
                    {
                        case (0):
                            for (int i = 0; i < 8; i++)
                            {
                                player.AddItem(ItemType.GrenadeHE);
                            }
                            break;
                        case (1):
                            for (int i = 0; i < 7; i++)
                            {
                                player.AddItem(ItemType.GrenadeHE);
                            }
                            player.AddItem(ItemType.GunFRMG0);
                            break;
                        default:
                            for (int i = 0; i < 7; i++)
                            {
                                player.AddItem(ItemType.GrenadeHE);
                            }
                            player.AddItem(ItemType.GunCrossvec);
                            break;
                    }


                    break;
                case RoleTypeId.NtfCaptain:
                    switch (Wanjia.rand.Next(0, 3))
                    {
                        case (0):
                            for (int i = 0; i < 8; i++)
                            {
                                player.AddItem(ItemType.MicroHID);
                            }
                            break;
                        case (1):
                            for (int i = 0; i < 7; i++)
                            {
                                player.AddItem(ItemType.MicroHID);
                            }
                            player.AddItem(ItemType.GunCrossvec);
                            break;
                        default:
                            for (int i = 0; i < 7; i++)
                            {
                                player.AddItem(ItemType.MicroHID);
                            }
                            player.AddItem(ItemType.GunCrossvec);
                            break;
                    }
                    break;
                case RoleTypeId.Tutorial:
                    break;
                case RoleTypeId.ChaosMarauder:
                    for (int i = 0; i < 4; i++)
                    {
                        player.AddItem(ItemType.GrenadeFlash);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        player.AddItem(ItemRandomizer.GetRandomItem());
                    }
                    break;
                default:
                    for (int i = 0; i < 9; i++)
                    {
                        player.AddItem(ItemRandomizer.GetRandomItem());
                    }
                    break;
            }
        }
    }

    public override void OnPlayerDeath(PlayerDeathEventArgs ev)
    {
        Wanjia.DoDeadHandler(ev.Player,ev.Attacker);
        if (ev.Attacker != null)
        {
            Wanjia attacter = Wanjia.Create(ev?.Attacker);
            attacter.AttactTimer = 0;
            attacter.DisengageTimer = 0;
            attacter.attackdeath(ev);
            if (ev.Attacker.IsSCP && attacter.scpKillHealCD == 0)
            {
                ev.Attacker.Heal(500);
                attacter.scpKillHealCD = 12;
            }
        }
        if (ev.Player != null)
        {
            Wanjia suffer = Wanjia.Create(ev?.Player);
            suffer.SufferTimer = 0;
            suffer.DisengageTimer = 0;
            suffer.defenddeath(ev);
            suffer.ClearEquipment();
            抽奖机.Lotterysclear(ev.Player);
        }
        
    }




    public override void OnPlayerTogglingFlashlight(PlayerTogglingFlashlightEventArgs ev)
    {

    }

    public override void OnPlayerToggledFlashlight(PlayerToggledFlashlightEventArgs ev)
    {
        Wanjia wj = Wanjia.Create(ev.Player);
        wj.ToggledFlashlight(ev);
    }

    public override void OnPlayerTogglingWeaponFlashlight(PlayerTogglingWeaponFlashlightEventArgs ev)
    {

    }

    public override void OnPlayerToggledWeaponFlashlight(PlayerToggledWeaponFlashlightEventArgs ev)
    {

        Wanjia wj = Wanjia.Create(ev.Player);
        wj.ToggledWeaponFlashlight(ev);
    }


    //public override void OnServerWaveRespawning(WaveRespawningEventArgs ev)
    //{
    //    Logger.Info($"{nameof(OnServerWaveRespawning)} triggered");
    //}
    public override void OnServerWaveRespawned(WaveRespawnedEventArgs ev)
    {
        //ev.Wave.RespawnTokens = 99;
        //ev.Wave.Base.Timer.SpawnIntervalSeconds -= 20;

        foreach (Player player in ev.Players) 
        {
            //player.EnableEffect<CustomPlayerEffects.SpawnProtected>(1, 40f, false);
            Wanjia.Create(player).SpawnProtectedTime = 40;
        }
    }

    public override void OnPlayerHurt(PlayerHurtEventArgs ev)
    {
        if (ev.Attacker != null && ev.Player.Faction == ev.Attacker.Faction)
        {
            return;
        }
            
        string reason;
        if(ev.DamageHandler is DisruptorDamageHandler)
        {
            reason = "3x分裂者";
        }
        else if (ev.DamageHandler is ExplosionDamageHandler)
        {
            reason = "爆炸";
        }
        else if (ev.DamageHandler is FirearmDamageHandler)
        {
            FirearmDamageHandler fire = ev.DamageHandler as FirearmDamageHandler;
            reason = fire.WeaponType.ToString();
        }
        else if (ev.DamageHandler is JailbirdDamageHandler)
        {
            reason = "囚鸟";
        }
        else if (ev.DamageHandler is MicroHidDamageHandler)
        {
            reason = "电炮";
        }
        else if (ev.DamageHandler is Scp018DamageHandler)
        {
            reason = "SCP018-跳跳球";
        }
        else if (ev.DamageHandler is ScpDamageHandler)
        {
            ScpDamageHandler scp = ev.DamageHandler as ScpDamageHandler;
            reason = scp.Attacker.Role.ToString();
        }
        else if (ev.DamageHandler is ScpDamageHandler)
        {
            ScpDamageHandler scp = ev.DamageHandler as ScpDamageHandler;
            reason = scp.Attacker.Role.ToString();
        }
        else if (ev.DamageHandler is Scp939DamageHandler)
        {
            reason = "SCP939";
        }
        else if (ev.DamageHandler is Scp3114DamageHandler)
        {
            Scp3114DamageHandler scp3114 = ev.DamageHandler as Scp3114DamageHandler;
            reason = "SCP3114-"+ scp3114.Subtype.ToString();
        }
        else if (ev.DamageHandler is Scp1507DamageHandler)
        {
            Scp1507DamageHandler scp1507 = ev.DamageHandler as Scp1507DamageHandler;
            reason = "SCP1507-火烈鸟";
        }
        else if (ev.DamageHandler is CustomReasonDamageHandler)
        {
            CustomReasonDamageHandler cus = ev.DamageHandler as CustomReasonDamageHandler;
            reason = cus.DeathScreenText;
        }
        else
        {
            reason = "未知:"+ev.DamageHandler.ToString();
        }
        if (ev.DamageHandler is StandardDamageHandler)
        {
            StandardDamageHandler st = ev.DamageHandler as StandardDamageHandler;
            ev?.Attacker?.SendConsoleMessage("你对" + ev?.Player.Nickname + "造成了"+ st?.Damage+ "点伤害，伤害原因："+reason);
            ev?.Player?.SendConsoleMessage("你受到了" + ev?.Attacker?.Nickname + "造成的" + st?.Damage + "点伤害，伤害原因：" + reason);
        }
        else
        {
            ev.Player?.SendConsoleMessage("未知伤害原因");
        }
        //if(ev.Player.RoleBase is IFpcRole ipc)
        //{
        //    PlayerRoles.In
        //}
    }


    public override void OnPlayerValidatedVisibility(PlayerValidatedVisibilityEventArgs ev)
    {

        if (ev.Target.Role == RoleTypeId.Scp0492 && ev.Player.IsHuman)
        {
            if (Wanjia.Create(ev.Target).DisengageTimer > 10)
            {
                if(Vector3.Distance(ev.Target.Position, ev.Player.Position) > 3)
                {
                    ev.IsVisible = false;
                }
            }
            else
            {
                if (Vector3.Distance(ev.Target.Position, ev.Player.Position) > 3)
                {
                    ev.IsVisible = false;
                }
            }

           
        }
    }

    public override void OnPlayerLeftPocketDimension(PlayerLeftPocketDimensionEventArgs ev)
    {
        int ran = Wanjia.rand.Next(0, 100);
        if (ran < 20)
        {
            ev.Player.SendBroadcast("老头口袋随机效果，你没了装备", 15);
            Wanjia.Create(ev.Player).ClearEquipment();
            return;
        }
        if(ran<40)
        {
            ev.Player.SendBroadcast("老头口袋随机效果，你得了近视", 15);
            ev.Player.EnableEffect<FogControl>(5);
            return;
        }
        if (ran < 60)
        {
            ev.Player.SetRole(RoleTypeId.Scp0492);
            ev.Player.SendBroadcast("老头口袋随机效果，所以你变成了小僵尸", 15);
            return;
        }
        if(ran < 80)
        {
            ev.Player.SendBroadcast("老头口袋随机效果，你变成了敌对阵营", 15);
            if (ev.Player.IsChaos)
            {
                ev.Player.SetRole(RoleTypeId.NtfSpecialist);
            }
            else
            {
                ev.Player.SetRole(RoleTypeId.ChaosRifleman);
            }
            return;
        }
        if(ran < 99)
        {
            ev.Player.SendBroadcast("老头口袋随机效果，你没了物品", 15);
            ev.Player.ClearItems();
            return;
        }
        if (ran < 100)
        {
            ev.Player.SendBroadcast("老头口袋随机效果，你变成了SCP106", 15);
            ev.Player.SetRole(RoleTypeId.Scp106);
            return;
        }

    }
}