using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Enemy
{
    public EnemyEnum EnemyEnum { get; }
    public RarityEnum RarityEnum { get; }

    public string Description { get; }

    public int Health { get; }

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
        Moves = moves;
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