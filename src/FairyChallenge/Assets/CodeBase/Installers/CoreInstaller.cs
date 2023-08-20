using Savidiy.Utils;
using Zenject;

namespace Fairy
{
    public class CoreInstaller : MonoInstaller
    {
        public HeroLibrary HeroLibrary;
        public FightTestLibrary FightTestLibrary;
        public ItemsLibrary ItemsLibrary;
        public FightSettings FightSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TickInvoker>().AsSingle();
            
            Container.BindInstance(HeroLibrary);
            Container.BindInstance(FightTestLibrary);
            Container.BindInstance(ItemsLibrary);
            Container.BindInstance(FightSettings);
            
            Container.Bind<HeroFactory>().AsSingle();
            Container.Bind<ActionFactory>().AsSingle();
            Container.Bind<ItemFactory>().AsSingle();
            Container.Bind<FightCalculator>().AsSingle();
        }
    }
}