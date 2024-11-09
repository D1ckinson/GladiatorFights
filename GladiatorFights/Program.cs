using System;
using System.Collections.Generic;
using System.Threading;

namespace GladiatorFights
{
    public class Program
    {
        static void Main()
        {
            Arena arena = new Arena();

            arena.Work();
        }
    }

    public class Arena
    {
        private List<Warrior> _warriors;

        public Arena()
        {
            _warriors = new List<Warrior>
            {
                new Rogue("Разбойник",90,15,7),
                new Barbarian("Варвар",110,16,4),
                new Fanatic("Фанатик", 130, 12, 9),
                new Mage("Маг", 95, 18, 1),
                new Thief("Вор", 80,17,12)
            };
        }

        public void Work()
        {
            const string FightCommand = "1";
            const string ExitCommand = "2";

            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("Добро пожаловать на Арену\n" +
                    $"{FightCommand} - Выбрать бойцов и начать бой\n" +
                    $"{ExitCommand} - Выйти");

                switch (UserUtils.ReadString())
                {
                    case FightCommand:
                        Fight();
                        break;

                    case ExitCommand:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Такой команды нет");
                        break;
                }
            }
        }

        public void Fight()
        {
            int millisecondsTimeout = 1400;
            Warrior warrior1 = ChoseWarrior();
            Warrior warrior2 = ChoseWarrior();

            while (warrior1.IsAlive && warrior2.IsAlive)
            {
                Thread.Sleep(millisecondsTimeout);

                warrior1.Attack(warrior2);
                warrior2.Attack(warrior1);
                Console.WriteLine();
            }

            WriteWiner(warrior1, warrior2);
        }

        private void WriteWiner(Warrior warrior1, Warrior warrior2)
        {
            if (warrior1.IsAlive)
            {
                Console.WriteLine($"Победил {warrior1.Name}");

                return;
            }

            if (warrior2.IsAlive)
            {
                Console.WriteLine($"Победил {warrior2.Name}");

                return;
            }

            Console.WriteLine($"{warrior1.Name} и {warrior2.Name} мертвы");

        }

        private Warrior ChoseWarrior()
        {
            int index;

            do
            {
                for (int i = 0; i < _warriors.Count; i++)
                {
                    Warrior warrior = _warriors[i];
                    Console.WriteLine($"{i + 1} - {warrior.Name}. Здоровье: {warrior.Health}. Урон: {warrior.Damage}. Броня: {warrior.Armor}");
                }

                index = UserUtils.ReadInt("Выберите воина: ") - 1;

            } while (IsIndexInRange(index, "Воина с таким номером нет") == false);

            return _warriors[index].Clone();
        }

        private bool IsIndexInRange(int index, string errorText)
        {
            if (index < 0 || index >= _warriors.Count)
            {
                Console.WriteLine(errorText);

                return false;
            }

            return true;
        }
    }

    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

    public abstract class Warrior : IDamageable
    {
        private float _minDamageMultiplier = 0.25f;
        private int _maxHealth;

        protected Warrior(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
            _maxHealth = health;
        }

        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public int Armor { get; private set; }

        public bool IsAlive => Health > 0;

        public virtual void Attack(IDamageable warrior)
        {
            Console.WriteLine($"{Name} атакует на {Damage} урона");
            warrior.TakeDamage(Damage);
        }

        public virtual void TakeDamage(int damage)
        {
            if (damage < 0)
                return;

            int resultDamage = Math.Max(damage - Armor, (int)(damage * _minDamageMultiplier));
            Health -= resultDamage;
            int blockedDamage = damage - resultDamage;

            Console.WriteLine($"{Name} получил {damage} урона. {Armor} защиты блокирует {blockedDamage} урона. оставшееся здоровье: {Health}");
        }

        protected void Heal(int healValue)
        {
            if (healValue < 0)
                return;

            Health = Math.Min(Health + healValue, _maxHealth);
        }

        public abstract Warrior Clone();
    }

    public class Rogue : Warrior
    {
        private int _doubleDamageChance = 25;
        private int _damageMultiplier = 2;
        private int _maxChance = 100;

        public Rogue(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public override void Attack(IDamageable warrior)
        {
            int chance = UserUtils.GenerateRandomValue(0, _maxChance + 1);

            if (chance <= _doubleDamageChance)
            {
                Console.WriteLine($"{Name} наносит двойной урон");
                warrior.TakeDamage(Damage * _damageMultiplier);

                return;
            }

            base.Attack(warrior);
        }

        public override Warrior Clone() =>
            new Rogue(Name, Health, Damage, Armor);
    }

    public class Barbarian : Warrior
    {
        private int _attackNumber = 0;
        private int _triggerAttackNumber = 3;

        public Barbarian(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public override void Attack(IDamageable warrior)
        {
            _attackNumber++;

            if (_attackNumber == _triggerAttackNumber)
            {
                Console.WriteLine($"{Name} бьет два раза");
                _attackNumber = 0;

                base.Attack(warrior);
            }

            base.Attack(warrior);
        }

        public override Warrior Clone() =>
            new Barbarian(Name, Health, Damage, Armor);
    }

    public class Fanatic : Warrior
    {
        private float _rageQuantity = 0f;
        private float _ragePerDamage = 1.3f;
        private float _triggerRageQuantity = 100;
        private int _heal = 26;
        private int _healPenalty = 5;
        private int _maxHealth;

        public Fanatic(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {
            _maxHealth = health;
        }

        public override void TakeDamage(int damage)
        {
            _rageQuantity += damage * _ragePerDamage;

            if (_rageQuantity >= _triggerRageQuantity)
            {
                Console.WriteLine($"{Name} исцеляет себя на {_heal} здоровья.");
                Heal(_heal);

                _rageQuantity -= _triggerRageQuantity;
                _heal -= _healPenalty;
            }

            base.TakeDamage(damage);
        }

        public override Warrior Clone() =>
            new Fanatic(Name, Health, Damage, Armor);
    }

    public class Mage : Warrior
    {
        private int _mana = 100;
        private int _fireballManaCost = 21;
        private int _fireBallDamage;
        private float _fireBallDamageMultiplier = 1.5f;

        public Mage(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {
            _fireBallDamage = (int)(damage * _fireBallDamageMultiplier);
        }

        public override void Attack(IDamageable warrior)
        {
            if (_mana >= _fireballManaCost)
            {
                Console.WriteLine($"{Name} Использует огненный шар");
                _mana -= _fireballManaCost;
                warrior.TakeDamage(_fireBallDamage);

                return;
            }

            base.Attack(warrior);
        }

        public override Warrior Clone() =>
            new Mage(Name, Health, Damage, Armor);
    }

    public class Thief : Warrior
    {
        private int _dodgeChance = 23;
        private int _maxChance = 100;

        public Thief(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public override void TakeDamage(int damage)
        {
            int chance = UserUtils.GenerateRandomValue(0, _maxChance + 1);

            if (chance <= _dodgeChance)
            {
                Console.WriteLine($"{Name} Увернулся от атаки");

                return;
            }

            base.TakeDamage(damage);
        }

        public override Warrior Clone() =>
            new Thief(Name, Health, Damage, Armor);
    }

    public static class UserUtils
    {
        private static Random s_random = new Random();

        public static int ReadInt(string text)
        {
            int number;

            while (int.TryParse(ReadString(text), out number) == false)
                Console.WriteLine("Некорректный ввод. Введите число.");

            return number;
        }

        public static string ReadString(string text = "")
        {
            Console.Write(text);

            return Console.ReadLine();
        }

        public static int GenerateRandomValue(int min, int max) =>
            s_random.Next(min, max);
    }
}
