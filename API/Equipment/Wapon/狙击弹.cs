using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static CustomItems.API.EquipmentBase;

namespace CustomItems.API.Equipment.Armor
{
    internal class 狙击弹 : EquipmentBase
    {

        public override EquipmentType EquipType => EquipmentType.Weapon;

        public override string Name => "狙击弹";

        public override string Description => "你的武器只能有一发子弹，但是造成5倍伤害";

        public override void OnRegistered()
        {
            抽奖机.AddLottery(new LotteryItem(this.Id, 1000));
        }
        public override void AttactHander(PlayerHurtingEventArgs ev, System.Object extra = null)
        {
            if (ev.Attacker == null) return;
            if (ev.DamageHandler is FirearmDamageHandler)
            {
                StandardDamageHandler st = ev.DamageHandler as FirearmDamageHandler;
                if(ev.Attacker.CurrentItem.Type == ItemType.GunShotgun)
                {
                    st.Damage *= 2;
                }else
                {
                    st.Damage *= 5;
                }
                
            }
        }

        public override void ReloadFireArmHander(PlayerReloadedWeaponEventArgs ev)
        {
            ev.FirearmItem.StoredAmmo = 0;
            
            foreach (Item item in ev.Player.Items)
            {
                if (item is FirearmItem firearmItem) {
                    firearmItem.StoredAmmo = 0;
                }

            }
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

        public override void OnPickedUp(PlayerPickedUpItemEventArgs ev) {
            if (ev.Item is FirearmItem)
            {
                FirearmItem st = ev.Item as FirearmItem;
                st.StoredAmmo = 0;
            }
        }


        public override void EffectHander(Wanjia wj, System.Object extra = null)
        {

        }
    }
}

