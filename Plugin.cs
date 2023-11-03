using Assets.Scripts.Objects.Clothing;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Pipes;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ExtraLogical
{
    [BepInPlugin("ExtraLogical", "Extra Logical", "0.1")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource sLogger;

        private void Awake()
        {
            sLogger = Logger;
            new Harmony("ExtraLogical").PatchAll(typeof(Patches));
        }
    }

    public class Patches
    {
        [HarmonyPatch(typeof(AdvancedTablet), nameof(AdvancedTablet.GetLogicable))]
        [HarmonyPatch(typeof(AdvancedSuit),   nameof(AdvancedSuit.GetLogicable))]
        [HarmonyPostfix]
        public static void GetLogicableWithCustomDevices (int deviceIndex, ref ILogicable __result, AdvancedTablet __instance)
        {
            if (__result != null) {
                return;
            }
            Plugin.sLogger.LogInfo("Logic called. Index: [" + deviceIndex + "]");
            Human H = __instance.RootParentHuman;
            switch (deviceIndex) {
                case 104: {
                        __result = (H?.GlassesSlot.Occupant) as ILogicable;
                        break;
                    }
                case 109: {
                        __result = (H?.LeftHandSlot.Occupant) as ILogicable;
                        break;
                    }
                case 110: {
                        __result = (H?.RightHandSlot.Occupant) as ILogicable;
                        break;
                    }
                default: {
                        break;
                    }
            }
        }

        [HarmonyPatch(typeof(AdvancedTablet), nameof(AdvancedTablet.IsValidIndex))]
        [HarmonyPatch(typeof(AdvancedSuit),   nameof(AdvancedSuit.IsValidIndex))]
        [HarmonyPostfix]
        public static void IsValidIndexWithCustomDevices(int index, ref bool __result)
        {
            __result = __result || index == 104;
        }

        // TODO Override System.Double Assets.Scripts.Objects.DynamicThing::GetLogicValue(Assets.Scripts.Objects.Motherboards.LogicType)

        // TODO Override System.Boolean Assets.Scripts.Objects.DynamicThing::CanLogicRead(Assets.Scripts.Objects.Motherboards.LogicType)
    }
}
