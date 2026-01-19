public interface IPlayerManager
{
    public int MaxHealth { get; }
    public int CurrentHealth { get; }
    public int TileCount { get; }
    public void Init();
    public void UpdateHealthBy(int value);
    public bool IsDead();
    public void FullHeath();
    public void HandleReward(Reward reward);
}