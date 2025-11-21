using System.Collections.Generic;

public class Enemy
{
    public EnemyEnum EnemyEnum { get; }
    public RarityEnum RarityEnum { get; }

    public string Title { get; }

    public string Description { get; }

    public int MaxHealth { get; }

    public int CurrentHealth { get; private set; }

    public int TurnsRemaining { get; set; }

    public List<Move> Moves { get; }

    public Move CurrentMove { get; private set; }

    public Enemy(
            EnemyEnum enemyEnum,
            RarityEnum rarityEnum,
            string title,
            string description,
            int health,
            List<Move> moves
        )
    {
        EnemyEnum = enemyEnum;
        RarityEnum = rarityEnum;
        Title = title;
        Description = description;
        MaxHealth = health;
        CurrentHealth = health;
        TurnsRemaining = 0;
        Moves = moves;
    }

    public void TakeDamage(int damage)
    {
        if (!IsDead())
            CurrentHealth -= damage;
    }

    public void Heal(int value)
    {
        if (!IsDead())
            CurrentHealth += value;
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    public void SetCurrentMove(Enemy.Move move)
    {
        CurrentMove = move;
        TurnsRemaining = move.Wait;
    }

    public string ShortLabel()
    {
        return $"{RarityEnum} {Title}";
    }

    public override string ToString()
    {
        return $"{ShortLabel()}:{CurrentHealth}hp:intending {CurrentMove} with {TurnsRemaining}tr";
    }

    public class Move
    {
        public string Title { get; }

        public string Description { get; }

        public int Value { get; }

        public int Wait { get; }

        public int Weight { get; }

        public MoveEnum MoveEnum { get; }

        public Move(
            string title,
            string description,
            int value,
            int wait,
            int weight,
            MoveEnum moveEnum
            )
        {
            Title = title;
            Description = description;
            Value = value;
            Wait = wait;
            Weight = weight;
            MoveEnum = moveEnum;
        }

        public override string ToString()
        {
            return $"{Title}({Value})";
        }
    }
}