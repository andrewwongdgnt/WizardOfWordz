using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Enemy
{
    public EnemyEnum EnemyEnum { get; }
    public RarityEnum RarityEnum { get; }

    public string Title { get; }

    public string Description { get; }

    public int Health { get; }

    public int CurrentHealth { get; private set; }

    public int TurnsRemaining { get; set; }

    public List<Move> Moves { get; }

    public Enemy(
            EnemyEnum enemyEnum,
            RarityEnum rarityEnum,
            string title,
            string description,
            int health,
            int delay,
            List<Move> moves
        )
    {
        EnemyEnum = enemyEnum;
        RarityEnum = rarityEnum;
        Title = title;
        Description = description;
        Health = health;
        CurrentHealth = health;
        TurnsRemaining = delay;
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

    public string ShortLabel()
    {
        return $"{RarityEnum} {Title}";
    }

    public override string ToString()
    {
        return $"{ShortLabel()}:{CurrentHealth}hp:{TurnsRemaining}tr";
    }

    public abstract class Move
    {
        public string Title { get; protected set; }

        public int Wait { get; protected set; }

        public int Weight { get; protected set; }

        public Move(
            string title,
            int wait,
            int weight
            )
        {
            Title = title;
            Wait = wait;
            Weight = weight;
        }

        public class Attack : Move
        {
            public int Damage { get; }

            public Attack(
                string title,
                int wait,
                int weight,
                int damage
                ) : base(
                    title,
                    wait,
                    weight
                    )
            {
                Damage = damage;
            }

            public override string ToString()
            {
                return $"{Title}({Damage}dmg)";
            }
        }
    }
}