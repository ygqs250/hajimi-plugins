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

namespace CustomItems.Core;

internal class EventHandler : CustomEventsHandler
{
    static Stopwatch stopwatch = new Stopwatch();

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

        if(!Wanjia.Create(ev.Player).CanUseMedical && ev.UsableItem.Category== ItemCategory.Medical)
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
        if (!Check(ev.Item.Serial)) return;
        var customItem = API.CustomItems.CurrentItems[ev.Item.Serial];
        customItem.OnPickedUp(ev);

        if (!customItem.ShowItemHints || !customItem.ShowPickupHints) return;
        ev.Player.SendHint($"装备名称:{customItem.Name}\n效果:{customItem.Description}");
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
        if (ev.OldItem != null && !ev.OldItem.IsDestroyed && Check(ev.OldItem.Serial))
        {
            API.CustomItems.CurrentItems[ev.OldItem.Serial].OnUnselected(ev);
        }
        if (ev.NewItem == null || ev.NewItem.IsDestroyed || !Check(ev.NewItem.Serial)) return;
        var customItem = API.CustomItems.CurrentItems[ev.NewItem.Serial];
        customItem.OnSelected(ev);
        if (!customItem.ShowItemHints || !customItem.ShowSelectedHints) return;
        ev.Player.SendHint($"你选中了 {customItem.Name}\n装备效果:{customItem.Description}");
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

    public override void OnServerRoundStarting(RoundStartingEventArgs ev)
    {
        stopwatch.Start();
        // 创建定时器（间隔1000毫秒 = 1秒）
        Timer timer = new Timer(1000);

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
            }
            else if(wave is ChaosSpawnWave chaosSpawnWave)
            {
                chaosSpawnWave.Timer.DefaultSpawnInterval = 120;
                chaosSpawnWave.Timer.SpawnIntervalSeconds = 120;
            } else if(wave is TimeBasedWave timeBasedWave1)
            {
                timeBasedWave1.Timer.DefaultSpawnInterval = 999;
                timeBasedWave1.Timer.SpawnIntervalSeconds = 999;
            }
        }
    }


    //服务器插件的定时器，非常重要，1s执行一次
    private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        TimeSpan elapsed = stopwatch.Elapsed;
        Server.PlayerListName = $"<size=300%><align=\"center\"><color=red>哈基米服务器1服 QQ群:913477784</color> \\n <size=150%><align=\"center\"><color=blue>开局时间：{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}</color>";
        List<Player> players = Player.ReadyList.ToList<Player>();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].IsAlive)
            {
                Wanjia wj = Wanjia.Get(players[i]);
                players[i].DisplayName = Wanjia.DisplayName(wj);
                wj.SufferTimer++;
                wj.DisengageTimer++;
                wj.AttactTimer++;
                wj?.Weapon?.EffectHander(wj);
                wj?.Armor?.EffectHander(wj);
                wj?.Accessory?.EffectHander(wj);

                if (players[i].IsSCP)
                {
                    if(wj.lastPostion == players[i].Position)
                    {
                        wj.moveTimer++;
                    }
                    else
                    {
                        wj.moveTimer  = 0;
                    }
                    if (wj.moveTimer > 10)
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

        }

        List<WaveTimer> waves = WaveSetting.getRimeBasedWave();
        //foreach (WaveTimer wave in waves)
        //{
        //    Log.Info(wave.ToString()+":"+ wave.DefaultSpawnInterval+":" + wave.SpawnIntervalSeconds + ":"+wave.TimePassed);
        //}
        //Log.Info("循环执行了一次");
    }

    public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        if (ev.DamageHandler is Scp049DamageHandler scp049)
        {
            if(scp049.DamageSubType == Scp049DamageHandler.AttackType.CardiacArrest)
            {
                Wanjia.ClearEquip(ev.Player);
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
            attacter.AttactTimer = 0;
            attacter.DisengageTimer = 0;
            attacter.attack(ev);
        }
        if (ev.Player != null)
        {
            Wanjia suffer = Wanjia.Create(ev?.Player);
            suffer.SufferTimer = 0;
            suffer.DisengageTimer = 0;
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
        }
    }

    public override void OnPlayerReloadingWeapon(PlayerReloadingWeaponEventArgs ev)
    {
        ev.Player.SetAmmo(ev.FirearmItem.AmmoType, 1000);
    }

    public override void OnPlayerReloadedWeapon(PlayerReloadedWeaponEventArgs ev)
    {
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
        if (ev.Player.IsSCP)
        {
            switch (ev.Player.Role)
            {
                case (RoleTypeId.Scp173):
                    ev.Player.MaxHealth = 7173;
                    ev.Player.Health = 7173;
                    break;
                case (RoleTypeId.Scp049):
                    ev.Player.MaxHealth = 4449;
                    ev.Player.Health = 4449;
                    break;
                case (RoleTypeId.Scp939):
                    ev.Player.MaxHealth = 4939;
                    ev.Player.Health = 4939;
                    break;
                case (RoleTypeId.Scp106):
                    ev.Player.MaxHealth = 5106;
                    ev.Player.Health = 5109;
                    break;
                case (RoleTypeId.Scp096):
                    ev.Player.MaxHealth = 5096;
                    ev.Player.Health = 5096;
                    break;
                //case (RoleTypeId.Scp079):
                //    ev.Player.SetRole(RoleTypeId.Scp106);
                //int ran = Wanjia.rand.Next(0, 5);
                //switch (ran)
                //{
                //    case (0):
                //        ev.Player.SetRole(RoleTypeId.Scp049);
                //        break;
                //    case (1):
                //        ev.Player.SetRole(RoleTypeId.Scp939);
                //        break;
                //    case (2):
                //        ev.Player.SetRole(RoleTypeId.Scp096);
                //        break;
                //    case (3):
                //        ev.Player.SetRole(RoleTypeId.Scp049);
                //        break;
                //    case (4):
                //        ev.Player.SetRole(RoleTypeId.Scp106);
                //        break;
                //    default:
                //        break;
                //}

                //break;
                default:
                    break;
            }
        }

        ev.Player.SetAmmo(ItemType.Ammo12gauge, 1000);
            ev.Player.SetAmmo(ItemType.Ammo44cal, 1000);
            ev.Player.SetAmmo(ItemType.Ammo9x19, 1000);
            ev.Player.SetAmmo(ItemType.Ammo762x39, 1000);
            ev.Player.SetAmmo(ItemType.Ammo556x45, 1000);

        if (ev.Player.Faction == Faction.FoundationStaff || ev.Player.Faction == Faction.FoundationEnemy)
        {
            ev.Player.ClearItems();

            if (ev.Player == null) return;

            //GiveItemsAfterDelay(ev.Player);
            //StartCoroutine();
            Timing.RunCoroutine(GiveItemsAfterDelay(ev.Player));
        }
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
                                player.AddItem(ItemType.GrenadeHE);
                            }
                            player.AddItem(ItemType.GunCrossvec);
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
            suffer.ClearEquip();
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
        ev.Wave.RespawnTokens = 99;
        ev.Wave.Base.Timer.SpawnIntervalSeconds -= 20;

        foreach (Player player in ev.Players) 
        {
            player.EnableEffect<CustomPlayerEffects.SpawnProtected>(1, 40f, false);
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
}