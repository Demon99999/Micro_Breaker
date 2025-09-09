namespace Assets.Scripts.Infrastructure.BootstrapperS
{
    public class BootstrapperStartScene : Bootstrapper
    {
        protected override void OnInit()
        {
            Wallet.Init(SaveService.Coins);
        }
    }
}
