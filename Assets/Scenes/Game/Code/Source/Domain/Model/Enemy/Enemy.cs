using System.Collections.Generic;
using System.Linq.Expressions;

public class Enemy
{
    public EnemyEnum EnemyEnum { get; }
    public RarityEnum RarityEnum { get; }

    public virtual string Title { get; }

    public virtual string Description { get; }

    public virtual int MaxHealth { get; }

    public virtual int CurrentHealth { get; private set; }

    public virtual int TurnsRemaining { get ; set; }

    public virtual List<Move> Moves { get; }

    public virtual Move CurrentMove { get; private set; }

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

    public virtual void UpdateHealthBy(int damage)
    {
        if (!IsDead())
            CurrentHealth += damage;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public virtual bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    public virtual void SetCurrentMove(Enemy.Move move)
    {
        CurrentMove = move;
        TurnsRemaining = move.Wait;
    }

    public virtual string ShortLabel()
    {
        return $"{RarityEnum} {Title}";
    }

    public override string ToString()
    {
        return $"{ShortLabel()}:{CurrentHealth}hp:intending {CurrentMove} with {TurnsRemaining}tr";
    }

    public class Move
    {
        public virtual string Title { get; }

        public virtual string Description { get; }

        public virtual int Value { get; }

        public virtual int Wait { get; }

        public virtual int Weight { get; }

        public virtual MoveEnum MoveEnum { get; }

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