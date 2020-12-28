using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MobileApp.Core.ViewModels.Home;

namespace MobileApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<NextViewModel>();

        }
    }
}
