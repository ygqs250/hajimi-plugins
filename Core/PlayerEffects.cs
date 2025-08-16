using CustomPlayerEffects;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItems.Core
{
    public class PlayerEffects
    {
        static bool isinit = false;
        public void intilize(Player player)
        {
            if (!isinit)
            {
                allEffects = player.ReferenceHub.playerEffectsController.AllEffects;
            }


        }
        static StatusEffectBase[] allEffects;



    }
}

/*
[2025-08-12 22:21:51.796 +08:00] [INFO] [CustomItems] Scp1853 (CustomPlayerEffects.Scp1853)
[2025-08-12 22:21:51.812 +08:00] [INFO] [CustomItems] AmnesiaVision (CustomPlayerEffects.AmnesiaVision)
[2025-08-12 22:21:51.828 +08:00] [INFO] [CustomItems] AmnesiaItems (CustomPlayerEffects.AmnesiaItems)
[2025-08-12 22:21:51.843 +08:00] [INFO] [CustomItems] Asphyxiated (CustomPlayerEffects.Asphyxiated)
[2025-08-12 22:21:51.858 +08:00] [INFO] [CustomItems] Scp1344 (CustomPlayerEffects.Scp1344)
[2025-08-12 22:21:51.874 +08:00] [INFO] [CustomItems] Scp1344Detected (CustomPlayerEffects.Scp1344Detected)
[2025-08-12 22:21:51.890 +08:00] [INFO] [CustomItems] Bleeding (CustomPlayerEffects.Bleeding) 流血效果，会自动愈合
[2025-08-12 22:21:51.906 +08:00] [INFO] [CustomItems] Blurred (CustomPlayerEffects.Blurred) 视野很模糊
[2025-08-12 22:21:51.921 +08:00] [INFO] [CustomItems] Burned (CustomPlayerEffects.Burned) 视野轻度模糊
[2025-08-12 22:21:51.936 +08:00] [INFO] [CustomItems] Concussed (CustomPlayerEffects.Concussed)
[2025-08-12 22:21:51.951 +08:00] [INFO] [CustomItems] Corroding (CustomPlayerEffects.Corroding)
[2025-08-12 22:21:51.967 +08:00] [INFO] [CustomItems] PocketCorroding (CustomPlayerEffects.PocketCorroding)
[2025-08-12 22:21:51.982 +08:00] [INFO] [CustomItems] Deafened (CustomPlayerEffects.Deafened) 耳聋
[2025-08-12 22:21:51.997 +08:00] [INFO] [CustomItems] Decontaminating (CustomPlayerEffects.Decontaminating)
[2025-08-12 22:21:52.011 +08:00] [INFO] [CustomItems] Disabled (CustomPlayerEffects.Disabled)
[2025-08-12 22:21:52.026 +08:00] [INFO] [CustomItems] Ensnared (CustomPlayerEffects.Ensnared) 定身
[2025-08-12 22:21:52.042 +08:00] [INFO] [CustomItems] Exhausted (CustomPlayerEffects.Exhausted)
[2025-08-12 22:21:52.058 +08:00] [INFO] [CustomItems] Flashed (CustomPlayerEffects.Flashed)
[2025-08-12 22:21:52.072 +08:00] [INFO] [CustomItems] Hemorrhage (CustomPlayerEffects.Hemorrhage) 移动出血
[2025-08-12 22:21:52.088 +08:00] [INFO] [CustomItems] Hypothermia (InventorySystem.Items.Usables.Scp244.Hypothermia.Hypothermia)
[2025-08-12 22:21:52.104 +08:00] [INFO] [CustomItems] Invigorated (CustomPlayerEffects.Invigorated)
[2025-08-12 22:21:52.119 +08:00] [INFO] [CustomItems] Poisoned (CustomPlayerEffects.Poisoned) 可乐和洗手液的中毒
[2025-08-12 22:21:52.135 +08:00] [INFO] [CustomItems] Scp207 (CustomPlayerEffects.Scp207)
[2025-08-12 22:21:52.150 +08:00] [INFO] [CustomItems] AntiScp207 (CustomPlayerEffects.AntiScp207)
[2025-08-12 22:21:52.166 +08:00] [INFO] [CustomItems] Invisible (CustomPlayerEffects.Invisible)
[2025-08-12 22:21:52.182 +08:00] [INFO] [CustomItems] Sinkhole (CustomPlayerEffects.Sinkhole)
[2025-08-12 22:21:52.197 +08:00] [INFO] [CustomItems] Stained (CustomPlayerEffects.Stained) 花生减速
[2025-08-12 22:21:52.214 +08:00] [INFO] [CustomItems] SeveredHands (CustomPlayerEffects.SeveredHands)
[2025-08-12 22:21:52.229 +08:00] [INFO] [CustomItems] RainbowTaste (CustomPlayerEffects.RainbowTaste)
[2025-08-12 22:21:52.246 +08:00] [INFO] [CustomItems] BodyshotReduction (CustomPlayerEffects.BodyshotReduction)
[2025-08-12 22:21:52.262 +08:00] [INFO] [CustomItems] DamageReduction (CustomPlayerEffects.DamageReduction)
[2025-08-12 22:21:52.278 +08:00] [INFO] [CustomItems] MovementBoost (CustomPlayerEffects.MovementBoost)
[2025-08-12 22:21:52.293 +08:00] [INFO] [CustomItems] Vitality (CustomPlayerEffects.Vitality)
[2025-08-12 22:21:52.309 +08:00] [INFO] [CustomItems] Traumatized (CustomPlayerEffects.Traumatized)
[2025-08-12 22:21:52.325 +08:00] [INFO] [CustomItems] CardiacArrest (CustomPlayerEffects.CardiacArrest)
[2025-08-12 22:21:52.341 +08:00] [INFO] [CustomItems] MuteSoundtrack (CustomPlayerEffects.SoundtrackMute)
[2025-08-12 22:21:52.357 +08:00] [INFO] [CustomItems] SpawnProtected (CustomPlayerEffects.SpawnProtected)
[2025-08-12 22:21:52.372 +08:00] [INFO] [CustomItems] InsufficientLighting (CustomPlayerEffects.InsufficientLighting)
[2025-08-12 22:21:52.387 +08:00] [INFO] [CustomItems] Scanned (CustomPlayerEffects.Scanned)
[2025-08-12 22:21:52.403 +08:00] [INFO] [CustomItems] Strangled (CustomPlayerEffects.Strangled)
[2025-08-12 22:21:52.417 +08:00] [INFO] [CustomItems] Ghostly (CustomPlayerEffects.Ghostly)
[2025-08-12 22:21:52.432 +08:00] [INFO] [CustomItems] SilentWalk (CustomPlayerEffects.SilentWalk) 静bu
[2025-08-12 22:21:52.449 +08:00] [INFO] [CustomItems] FogControl (CustomPlayerEffects.FogControl) 设置烟雾的类型，0默认，1关闭烟雾，其他是更看不清的烟雾
[2025-08-12 22:21:52.463 +08:00] [INFO] [CustomItems] Slowness (CustomPlayerEffects.Slowness) 减速
[2025-08-12 22:21:52.479 +08:00] [INFO] [CustomItems] Blindness (CustomPlayerEffects.Blindness)
[2025-08-12 22:21:52.495 +08:00] [INFO] [CustomItems] SeveredEyes (CustomPlayerEffects.SeveredEyes)
[2025-08-12 22:21:52.510 +08:00] [INFO] [CustomItems] PitDeath (CustomPlayerEffects.PitDeath)
[2025-08-12 22:21:52.526 +08:00] [INFO] [CustomItems] Scp1576 (CustomPlayerEffects.Scp1576)
[2025-08-12 22:21:52.541 +08:00] [INFO] [CustomItems] Fade (CustomPlayerEffects.Fade)
[2025-08-12 22:21:52.557 +08:00] [INFO] [CustomItems] Lightweight (CustomPlayerEffects.Lightweight)
[2025-08-12 22:21:52.573 +08:00] [INFO] [CustomItems] HeavyFooted (CustomPlayerEffects.HeavyFooted)
*/