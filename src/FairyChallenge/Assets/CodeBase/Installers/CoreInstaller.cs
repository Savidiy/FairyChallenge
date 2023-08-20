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
        public BackgroundLibrary BackgroundLibrary;
        public NodesLibrary NodesLibrary;
        public StorySettings StorySettings;
        public PersonLibrary PersonLibrary;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TickInvoker>().AsSingle();
            
            Container.BindInstance(HeroLibrary);
            Container.BindInstance(FightTestLibrary);
            Container.BindInstance(ItemsLibrary);
            Container.BindInstance(FightSettings);
            Container.BindInstance(BackgroundLibrary);
            Container.BindInstance(NodesLibrary);
            Container.BindInstance(StorySettings);
            Container.BindInstance(PersonLibrary);
            
            Container.Bind<HeroFactory>().AsSingle();
            Container.Bind<ActionFactory>().AsSingle();
            Container.Bind<ItemFactory>().AsSingle();
            Container.Bind<FightCalculator>().AsSingle();
        }
    }
}