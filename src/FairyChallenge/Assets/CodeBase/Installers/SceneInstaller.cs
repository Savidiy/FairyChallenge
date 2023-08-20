using Zenject;

namespace Fairy
{
    public class SceneInstaller : MonoInstaller
    {
        public StoryWindow StoryWindow;
        public IntroWindow IntroWindow;
        public FightWindow FightWindow;

        public override void InstallBindings()
        {
            Container.BindInstance(StoryWindow);
            Container.BindInstance(IntroWindow);
            Container.BindInstance(FightWindow);

            Container.Bind<ApplicationStateMachine>().AsSingle();
            Container.BindInterfacesTo<IntroApplicationState>().AsSingle();
            Container.BindInterfacesTo<SelectContinueApplicationState>().AsSingle();
            Container.BindInterfacesTo<NewGameApplicationState>().AsSingle();
            Container.BindInterfacesTo<StoryApplicationState>().AsSingle();
            Container.BindInterfacesTo<FightApplicationState>().AsSingle();
            
            Container.Bind<FightStateMachine>().AsSingle();
            Container.BindInterfacesTo<ActionFightState>().AsSingle();
            Container.BindInterfacesTo<IntroFightState>().AsSingle();
            Container.BindInterfacesTo<LoadingFightState>().AsSingle();
            Container.BindInterfacesTo<SelectActionFightState>().AsSingle();
            Container.BindInterfacesTo<LoseFightState>().AsSingle();
            Container.BindInterfacesTo<WinFightState>().AsSingle();

            Container.Bind<StoryTeller>().AsSingle();
            Container.Bind<FightLoop>().AsSingle();

            Container.Bind<StepFactory>().AsSingle();
            Container.BindInterfacesTo<BackgroundStepFactory>().AsSingle();
            Container.BindInterfacesTo<DialogStepFactory>().AsSingle();
            Container.BindInterfacesTo<ButtonStepFactory>().AsSingle();
            Container.BindInterfacesTo<EncounterStepFactory>().AsSingle();
            Container.BindInterfacesTo<GotoStepFactory>().AsSingle();
        }
    }
}