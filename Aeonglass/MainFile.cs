using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves.Runs;

[ModInitializer(nameof(Initialize))]
public class MainFile
{
    public const string ModId = "Aeonglass";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        ConfigFile.LoadConfig();

        harmony.CreateClassProcessor(typeof(AeonglassPatch)).Patch();
        harmony.CreateClassProcessor(typeof(CardPoolPatch)).Patch();
        harmony.CreateClassProcessor(typeof(LocalizationPatch)).Patch();
        harmony.CreateClassProcessor(typeof(WitherTurnEndPatch)).Patch();

        if ((bool)ConfigFile._aeonglassV106)
        {
            harmony.CreateClassProcessor(typeof(AeonglassIconPatch)).Patch();
            harmony.CreateClassProcessor(typeof(WitheringPresencePowerIconPatch)).Patch();
            harmony.CreateClassProcessor(typeof(WitherV106TitlePatch)).Patch();
        }

        if ((bool)ConfigFile._shouldActivateNewRelics)
        {
            harmony.CreateClassProcessor(typeof(RelicPoolPatch)).Patch();
            harmony.CreateClassProcessor(typeof(NeowOptionPatch)).Patch();
            SavedPropertiesTypeCache.InjectTypeIntoCache(typeof(FishingRod));
            SavedPropertiesTypeCache.InjectTypeIntoCache(typeof(SilkenTress));
        }
    }
}
