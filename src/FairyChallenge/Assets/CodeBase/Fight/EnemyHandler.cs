namespace Fairy
{
    public sealed class EnemyHandler
    {
        public Hero Enemy { get; private set; }

        public void SetEnemy(Hero enemy)
        {
            Enemy = enemy;
        }
    }
}