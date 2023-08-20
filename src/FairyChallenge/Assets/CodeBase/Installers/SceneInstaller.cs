using Zenject;

namespace Fairy
{
    public class SceneInstaller : MonoInstaller
    {
        public StoryWindow StoryWindow;
        public IntroWindow IntroWindow;

        public override void InstallBindings()
        {
            Container.BindInstance(StoryWindow);
            Container.BindInstance(IntroWindow);

            Container.Bind<ApplicationStateMachine>().AsSingle();
            Container.BindInterfacesTo<IntroApplicationState>().AsSingle();
            Container.BindInterfacesTo<SelectContinueApplicationState>().AsSingle();
            Container.BindInterfacesTo<NewGameApplicationState>().AsSingle();
            Container.BindInterfacesTo<StoryApplicationState>().AsSingle();
        }
    }
}