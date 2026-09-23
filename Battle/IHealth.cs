namespace GameEngine
{
    public interface IHealth
    {
        int BaseHealth { get; }
        int CurrentHealth { get; }

        void TakeDamage(int amount);
        void Heal(int amount);
    }
}