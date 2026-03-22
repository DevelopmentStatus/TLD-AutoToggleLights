using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;

[assembly: MelonInfo(typeof(AutoLightToggle.MyMod), "Auto Light Extinguish", "1.0.0", "Krusty")]
[assembly: MelonGame("Hinterland", "TheLongDark")]

namespace AutoLightToggle
{
    public class MyMod : MelonMod
    {
        [HarmonyPatch(typeof(PassTime), nameof(PassTime.Begin))]
        internal class Patch_PassTime_Begin
        {
            private static void Postfix() => ExtinguishHeldLight();
        }

        [HarmonyPatch(typeof(Panel_Rest), nameof(Panel_Rest.OnRest))]
        internal class Patch_Panel_Rest_OnRest
        {
            private static void Postfix() => ExtinguishHeldLight();
        }

        private static void ExtinguishHeldLight()
        {
            var pm = GameManager.GetPlayerManagerComponent();
            if (pm == null || pm.m_ItemInHands == null) return;

            GearItem heldItem = pm.m_ItemInHands;

            var lantern = heldItem.GetComponent<KeroseneLampItem>();
            if (lantern != null && lantern.IsOn())
            {
                lantern.TurnOff();
                MelonLogger.Msg("Extinguished Lantern.");
            }

            var torch = heldItem.GetComponent<TorchItem>();
            if (torch != null && torch.IsBurning())
            {
                torch.Extinguish(TorchState.Extinguished);
                MelonLogger.Msg("Extinguished Torch.");
            }

            var flashlight = heldItem.GetComponent<FlashlightItem>();
            if (flashlight != null && flashlight.IsOn())
            {
                flashlight.TurnOff();
                MelonLogger.Msg("Extinguished Flashlight.");
            }
        }
    }
}