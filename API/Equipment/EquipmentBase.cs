using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MEC;
using Mirror;
using PlayerStatsSystem;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace CustomItems.API;

public class EquipmentBase: CustomItem
{
    public enum EquipmentType
    {
        Weapon,     // 武器
        Armor,      // 防具
        Accessory   // 饰品
    }

    public virtual EquipmentType EquipType => EquipmentType.Weapon;

    public override string Name => "测试装备";

    public override string Description => "没有用，没有任何效果";

    public override ItemType Type => ItemType.Coin;

    public virtual  void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
    {

    }



    public virtual void DefendHander(PlayerHurtingEventArgs ev, System.Object extra = null)
    {

    }

    public virtual void AttactDyingHander(PlayerDyingEventArgs ev)
    {

    }



    public virtual void DefendDyingHander(PlayerDyingEventArgs ev)
    {

    }

    public virtual void EffectHander(Wanjia wj, System.Object extra = null)
    {

    }

    public virtual void EquipHander(Wanjia wj, System.Object extra = null)
    {

    }

    public virtual void RemoveHander(Wanjia wj, System.Object extra = null)
    {

    }

    public virtual void KillHander(PlayerDeathEventArgs ev)
    {

    }
    public virtual void KilledHander(PlayerDeathEventArgs ev)
    {
        
    }

    public virtual void ReloadFireArmHander(PlayerReloadedWeaponEventArgs ev)
    {

    }


    public virtual void ToggledWeaponFlashlightHander(PlayerToggledWeaponFlashlightEventArgs ev)
    {

    }

    public virtual void ToggledFlashlightHander(PlayerToggledFlashlightEventArgs ev)
    {

    }



    public override void OnRegistered()
    {
        抽奖机.AddLottery(new LotteryItem(this.Id, 0));
    }

    public override void OnUnregistered()
    {

    }

    public override  void OnFlippedCoin(PlayerFlippedCoinEventArgs ev)
    {
        Wanjia wj = Wanjia.Create(ev.Player);
        ev.Player.SendConsoleMessage(this.Name + "yqgs");
        switch (EquipType)
        {
            case EquipmentType.Weapon:
                if(wj.Weapon == null)
                {
                    wj.Weapon = this;
                    ev.Player.SendBroadcast("你装备了武器：" + wj.Weapon.Name, 5, Broadcast.BroadcastFlags.Normal, true);
                    ev.Player.RemoveItem(ev.CoinItem);
                }
                else
                {
                    ev.Player.SendBroadcast("你已经装备了武器：" + wj.Weapon.Name+",不能再装备武器了", 5, Broadcast.BroadcastFlags.Normal, true);
                }
                break;
            case EquipmentType.Armor:
                if (wj.Armor == null)
                {
                    wj.Armor = this;
                    ev.Player.SendBroadcast("你装备了防具：" + wj.Armor.Name, 5, Broadcast.BroadcastFlags.Normal, true);
                    ev.Player.RemoveItem(ev.CoinItem);
                }
                else
                {
                    ev.Player.SendBroadcast("你已经装备了防具：" + wj.Armor.Name + ",不能再装备防具了", 5, Broadcast.BroadcastFlags.Normal, true);
                }
                break;
            case EquipmentType.Accessory:
                if (wj.Accessory == null)
                {
                    wj.Accessory = this;
                    ev.Player.SendBroadcast("你装备了饰品：" + wj.Accessory.Name, 5, Broadcast.BroadcastFlags.Normal, true);
                    ev.Player.RemoveItem(ev.CoinItem);
                }
                else
                {
                    ev.Player.SendBroadcast("你已经装备了饰品：" + wj.Accessory.Name + ",不能再装备饰品了", 5, Broadcast.BroadcastFlags.Normal, true);
                }
                break;
            default:
                ev.Player.SendBroadcast("错误的装备", 5, Broadcast.BroadcastFlags.Normal, true);
                break;
        }
    }

    public override void OnPickedUp(PlayerPickedUpItemEventArgs ev)
    {
        //ev.Player.SendConsoleMessage(ev.Player.Position.ToString());
        //Pickup PIC;
        //CustomItems.TrySpawn(0, ev.Player.Position,out PIC);
        Wanjia wj = Wanjia.Create(ev.Player);
        switch (EquipType)
        {
            case EquipmentType.Weapon:
                    ev.Player.SendBroadcast("你捡起了武器：" + CustomItems.CurrentItems[ev.Item.Serial].Name  ,5, Broadcast.BroadcastFlags.Normal, true);
                break;
            case EquipmentType.Armor:
                ev.Player.SendBroadcast("你捡起了防具：" + CustomItems.CurrentItems[ev.Item.Serial].Name, 5, Broadcast.BroadcastFlags.Normal, true);
                break;
            case EquipmentType.Accessory:
                ev.Player.SendBroadcast("你捡起了饰品：" + CustomItems.CurrentItems[ev.Item.Serial].Name, 5, Broadcast.BroadcastFlags.Normal, true);
                break;
            default:
                ev.Player.SendBroadcast("错误的装备", 5, Broadcast.BroadcastFlags.Normal, true);
                break;
        }
    }

    public override void OnDropped(PlayerDroppedItemEventArgs ev)
    {
        NetworkServer.UnSpawn(ev.Pickup.Base.gameObject);
        ev.Pickup.Base.gameObject.transform.localScale = Vector3.one * 10;
        NetworkServer.Spawn(ev.Pickup.Base.gameObject);

        Wanjia wj = Wanjia.Create(ev.Player);
        switch (EquipType)
        {
            case EquipmentType.Weapon:
                ev.Player.SendBroadcast("你丢下了武器：" + CustomItems.CurrentItems[ev.Pickup.Serial].Name, 5, Broadcast.BroadcastFlags.Normal, true);
                break;
            case EquipmentType.Armor:
                ev.Player.SendBroadcast("你丢下了防具：" + CustomItems.CurrentItems[ev.Pickup.Serial].Name, 5, Broadcast.BroadcastFlags.Normal, true);
                break;
            case EquipmentType.Accessory:
                ev.Player.SendBroadcast("你丢下了饰品：" + CustomItems.CurrentItems[ev.Pickup.Serial].Name, 5, Broadcast.BroadcastFlags.Normal, true);
                break;
            default:
                ev.Player.SendBroadcast("你丢下了错误的装备", 5, Broadcast.BroadcastFlags.Normal, true);
                break;
        }

    }


}