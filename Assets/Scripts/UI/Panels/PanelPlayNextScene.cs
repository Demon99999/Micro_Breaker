namespace Assets.Scripts.UI.Panels
{
    public class PanelPlayNextScene : Panel
    {
        public override async void OnMove(bool isActive)
        {
            base.OnMove(isActive);
            await MovePanel(isActive);
        }
    }
}