using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using static UnityEngine.Rendering.DebugUI;

public class Enemy
{
    public EnemyEnum EnemyEnum { get; }
    public RarityEnum RarityEnum { get; }

    public string Description { get; }

    public int Health { get; }

    public int CurrentHealth { get; private set; }

    public List<Move> Moves { get; }

    public Enemy (
            EnemyEnum enemyEnum,
            RarityEnum rarityEnum,
            string description,
            int health,
            List<Move> moves
        )
    {
        EnemyEnum = enemyEnum;
        RarityEnum = rarityEnum;
        Description = description;
        Health = health;
        CurrentHealth = health;
        Moves = moves;
    }

    public void TakeDamage(int damage)
    {
        if (!IsDead())
            CurrentHealth -= damage;
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    public override string ToString()
    {
        return $"{RarityEnum} {EnemyEnum} at {CurrentHealth}hp";
    }

    public abstract class Move
    {
        public string Title { get; protected set; }

        public int Wait { get; protected set; }

        public int Weight { get; protected set; }

        public class Attack : Move
        {
            public int Damage { get; }

            public Attack(
                string title,
                int wait,
                int weight,
                int damage
                ) {
                Title = title;
                Wait = wait; 
                Weight = weight;
                Damage = damage;
            }
        }
    }
}