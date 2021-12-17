using Sangheli.Event;
using Zenject;

namespace Sangheli.Installer
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EventController>().AsSingle();
        }
    }
}