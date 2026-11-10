using System.Collections;
using System.Reflection;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace ConfigurableDischarge.Settings
{
    public class ConfigurableDischargeSettingsOwner : ModSettingsOwner
    {
        private readonly ModSetting<float> _waterFlowMultiplier = new(1f, ModSettingDescriptor
            .CreateLocalized("Souvy.ConfigurableDischarge.WaterFlowMultiplier")
            .SetLocalizedTooltip("Souvy.ConfigurableDischarge.WaterFlowMultiplierDesc"));

        private readonly ModSetting<float> _badwaterFlowMultiplier = new(1f, ModSettingDescriptor
            .CreateLocalized("Souvy.ConfigurableDischarge.BadwaterFlowMultiplier")
            .SetLocalizedTooltip("Souvy.ConfigurableDischarge.BadwaterFlowMultiplierDesc"));

        private readonly ModSetting<bool> _allowOverflow = new(false, ModSettingDescriptor
            .CreateLocalized("Souvy.ConfigurableDischarge.AllowOverflow")
            .SetLocalizedTooltip("Souvy.ConfigurableDischarge.AllowOverflowDesc"));

        public static float WaterFlowMultiplier { get; private set; } = 1f;
        public static float BadwaterFlowMultiplier { get; private set; } = 1f;
        public static bool AllowOverflow { get; private set; } = false;

        public static ConfigurableDischargeSettingsOwner Instance { get; private set; }

        public ConfigurableDischargeSettingsOwner(
            ISettings settings,
            ModSettingsOwnerRegistry modSettingsOwnerRegistry,
            ModRepository modRepository
        ) : base(settings, modSettingsOwnerRegistry, modRepository)
        {
            Instance = this;
            CleanupDuplicateRegistrations(modSettingsOwnerRegistry);
        }

        private void CleanupDuplicateRegistrations(ModSettingsOwnerRegistry registry)
        {
            var field = typeof(ModSettingsOwnerRegistry).GetField("_modSettingOwners", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null && field.GetValue(registry) is IDictionary dict)
            {
                foreach (IList list in dict.Values)
                {
                    if (list != null && list.Contains(this) && list.Count > 1)
                    {
                        list.Remove(this);
                        break;
                    }
                }
            }
        }

        public override string HeaderLocKey => "Souvy.ConfigurableDischarge.Setting.Header";
        public override ModSettingsContext ChangeableOn => ModSettingsContext.MainMenu | ModSettingsContext.Game;
        protected override string ModId => ConfigurableDischargePlugin.ModId;

        protected override void OnAfterLoad()
        {
            AddCustomModSetting(_waterFlowMultiplier, nameof(_waterFlowMultiplier));
            AddCustomModSetting(_badwaterFlowMultiplier, nameof(_badwaterFlowMultiplier));
            AddCustomModSetting(_allowOverflow, nameof(_allowOverflow));

            _waterFlowMultiplier.ValueChanged += (_, _) => UpdateValues();
            _badwaterFlowMultiplier.ValueChanged += (_, _) => UpdateValues();
            _allowOverflow.ValueChanged += (_, _) => UpdateValues();

            UpdateValues();
        }

        public static void UpdateValues()
        {
            if (Instance == null) return;

            WaterFlowMultiplier = Instance._waterFlowMultiplier.Value;
            BadwaterFlowMultiplier = Instance._badwaterFlowMultiplier.Value;
            AllowOverflow = Instance._allowOverflow.Value;

            if (WaterFlowMultiplier <= 0f)
            {
                WaterFlowMultiplier = 1f;
            }

            if (BadwaterFlowMultiplier <= 0f)
            {
                BadwaterFlowMultiplier = 1f;
            }
        }
    }
}

