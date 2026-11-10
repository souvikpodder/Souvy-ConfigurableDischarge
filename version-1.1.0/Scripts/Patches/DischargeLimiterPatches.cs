using System.Reflection;
using ConfigurableDischarge.Settings;
using HarmonyLib;

namespace ConfigurableDischarge.Patches
{
    /// <summary>
    /// When AllowOverflow is enabled in settings, bypasses the water depth limiter so
    /// fluid dumps can continue dumping even when the spout is submerged.
    /// </summary>
    [HarmonyPatch]
    public static class DischargeLimiterPatches
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("Timberborn.WaterWorkshops.WaterOutputManufactoryLimiter");
            return AccessTools.Method(type, "ProductionEfficiency");
        }

        [HarmonyPrefix]
        public static bool Prefix(ref float __result)
        {
            if (ConfigurableDischargeSettingsOwner.AllowOverflow)
            {
                __result = 1f;
                return false; // Skip original check
            }

            return true; // Run original check
        }
    }
}
