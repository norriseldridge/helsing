namespace Helsing.Client.UI.Api
{
    public readonly struct FadeData
    {
        public readonly bool fade;

        public FadeData(bool fade) => this.fade = fade;
    }

    public readonly struct FadeCompleteMessage { }
}
