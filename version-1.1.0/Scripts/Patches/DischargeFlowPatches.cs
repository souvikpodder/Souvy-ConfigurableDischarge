using System;
using System.Reflection;
using ConfigurableDischarge.Settings;
using HarmonyLib;
using Timberborn.WaterBuildings;
using Timberborn.Workshops;

namespace ConfigurableDischarge.Patches
{
    [HarmonyPatch]
    public static class CleanWaterDischargeFlowPatch
    {
        private static FieldInfo _waterOutputField;
        private static FieldInfo _producedWaterField;
        private static bool _fieldsInitialized;

        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("Timberborn.WaterWorkshops.ManufactoryWaterProducer");
            return AccessTools.Method(type, "OnProductionProgressed");
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance, ProductionProgressedEventArgs e)
        {
            float multiplier = ConfigurableDischargeSettingsOwner.WaterFlowMultiplier;
            if (multiplier == 1f)
            {
                return true; // Execute original method
            }

            if (!_fieldsInitialized)
            {
                var type = __instance.GetType();
                _waterOutputField = AccessTools.Field(type, "_waterOutput");
                _producedWaterField = AccessTools.Field(type, "_producedWater");
                _fieldsInitialized = true;
            }

            var waterOutput = (WaterOutput)_waterOutputField.GetValue(__instance);
            float producedWater = (float)_producedWaterField.GetValue(__instance);

            waterOutput.AddCleanWater(producedWater * multiplier * e.ProductionProgressChange);
            return false; // Skip original method
        }
    }

    [HarmonyPatch]
    public static class BadwaterDischargeFlowPatch
    {
        private static FieldInfo _waterOutputField;
        private static FieldInfo _producedWaterContaminationField;
        private static bool _fieldsInitialized;

        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("Timberborn.WaterWorkshops.ManufactoryWaterContaminationProducer");
            return AccessTools.Method(type, "OnProductionProgressed");
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance, ProductionProgressedEventArgs e)
        {
            float multiplier = ConfigurableDischargeSettingsOwner.BadwaterFlowMultiplier;
            if (multiplier == 1f)
            {
                return true; // Execute original method
            }

            if (!_fieldsInitialized)
            {
                var type = __instance.GetType();
                _waterOutputField = AccessTools.Field(type, "_waterOutput");
                _producedWaterContaminationField = AccessTools.Field(type, "_producedWaterContamination");
                _fieldsInitialized = true;
            }

            var waterOutput = (WaterOutput)_waterOutputField.GetValue(__instance);
            float producedWaterContamination = (float)_producedWaterContaminationField.GetValue(__instance);

            waterOutput.AddContaminatedWater(producedWaterContamination * multiplier * e.ProductionProgressChange);
            return false; // Skip original method
        }
    }
}

