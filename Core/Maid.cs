using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.Core
{
    public class Maid
    {
        public static void clean()
        {
            foreach (Ragdoll ragdoll in Ragdoll.List)
            {
                ragdoll.Destroy();
            }

            foreach (Pickup item in Pickup.List)
            {
                if (ShouldDestroyItem(item.Type))
                {
                    item.Destroy();
                }
            }
        }
        public static bool ShouldDestroyItem(ItemType itemType)
        {
            // 定义需要保留的物品白名单
            ItemType[] excludedItems =
            {
                // 钥匙卡系列
                ItemType.KeycardMTFOperative,
                ItemType.KeycardMTFCaptain,
                ItemType.KeycardFacilityManager,
                ItemType.KeycardChaosInsurgency,
                ItemType.KeycardO5,
                
                // 医疗用品
                ItemType.Medkit,
                ItemType.Adrenaline,
                ItemType.Painkillers,
                ItemType.SCP500,

                
                
                // 通讯设备
                ItemType.Flashlight,
                
                // 特殊物品
                ItemType.SCP1344,
                ItemType.SCP207,
                ItemType.Coin,
                ItemType.SCP018,
                ItemType.SCP1576,
                ItemType.SCP2176,
                ItemType.SCP330,
                ItemType.AntiSCP207,
                ItemType.SCP1344,
                
                //武器
                ItemType.MicroHID,
                ItemType.ParticleDisruptor,
                ItemType.GunSCP127,
                ItemType.GunLogicer,
                ItemType.GunFSP9
            };

            // 如果物品在白名单中则保留，否则销毁
            return !excludedItems.Contains(itemType);

        }

    }


  }
