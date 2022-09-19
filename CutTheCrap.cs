namespace Carbon.Plugins
{
    [Info("CutTheCrap", "C MHawke", "1.0.0")]
    [Description("Fertilizer straight from the horses ass")]
    public class CutTheCrap : CarbonPlugin
    {
        object OnHorseDung(BaseRidableAnimal horse)
        { return ItemManager.CreateByName("fertilizer"); }
    }
}