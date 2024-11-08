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
        private List<WarriorFabric> _warriorFabrics;

        public Arena()
        {
            _warriorFabrics = new List<WarriorFabric>()
            {
                new RogueFabric(),
                new BarbarianFabric(),
                new FanaticFabric(),
                new MageFabric(),
                new ThiefFabric()
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
                for (int i = 0; i < _warriorFabrics.Count; i++)
                    Console.WriteLine($"{i + 1} - {_warriorFabrics[i].Description}");

                index = UserUtils.ReadInt("Выберите воина: ") - 1;

            } while (IsIndexInRange(index, "Воина с таким номером нет") == false);

            return _warriorFabrics[index].Create();
        }

        private bool IsIndexInRange(int index, string errorText)
        {
            if (index < 0 || index >= _warriorFabrics.Count)
            {
                Console.WriteLine(errorText);

                return false;
            }

            return true;
        }
    }

    public abstract class WarriorFabric
    {
        protected string Name;

        public abstract string Description { get; protected set; }
        protected abstract int[] HealthStats { get; set; }
        protected abstract int[] DamageStats { get; set; }
        protected abstract int[] ArmorStats { get; set; }

        public abstract Warrior Create();

        protected int GenerateHealth() =>
            UserUtils.GenerateRandomValue(HealthStats);

        protected int GenerateDamage() =>
            UserUtils.GenerateRandomValue(DamageStats);

        protected int GenerateArmor() =>
            UserUtils.GenerateRandomValue(ArmorStats);
    }

    public class RogueFabric : WarriorFabric
    {
        public RogueFabric()
        {
            Name = "Разбойник";
            HealthStats = new int[] { 80, 100 };
            DamageStats = new int[] { 10, 20 };
            ArmorStats = new int[] { 5, 10 };
            Description = $"Выбрать Разбойника. Имеет шанс нанести двойной урон" +
                $" Здоровье: {HealthStats[0]}-{HealthStats[1]}." +
                $" Урон: {DamageStats[0]}-{DamageStats[1]}." +
                $" Защита: {ArmorStats[0]}-{ArmorStats[1]}.";
        }

        public override string Description { get; protected set; }
        protected override int[] HealthStats { get; set; }
        protected override int[] DamageStats { get; set; }
        protected override int[] ArmorStats { get; set; }

        public override Warrior Create() =>
            new Rogue(Name, GenerateHealth(), GenerateDamage(), GenerateArmor());
    }

    public class BarbarianFabric : WarriorFabric
    {
        public BarbarianFabric()
        {
            Name = "Варвар";
            HealthStats = new int[] { 100, 120 };
            DamageStats = new int[] { 15, 18 };
            ArmorStats = new int[] { 3, 6 };
            Description = "Выбрать Варвара. Каждую третью свою атаку наносит дважды урон врагу" +
                $" Здоровье: {HealthStats[0]}-{HealthStats[1]}." +
                $" Урон: {DamageStats[0]}-{DamageStats[1]}." +
                $" Защита: {ArmorStats[0]}-{ArmorStats[1]}.";
        }

        public override string Description { get; protected set; }
        protected override int[] HealthStats { get; set; }
        protected override int[] DamageStats { get; set; }
        protected override int[] ArmorStats { get; set; }

        public override Warrior Create() =>
            new Barbarian(Name, GenerateHealth(), GenerateDamage(), GenerateArmor());
    }

    public class FanaticFabric : WarriorFabric
    {
        public FanaticFabric()
        {
            Name = "Фанатик";
            HealthStats = new int[] { 120, 150 };
            DamageStats = new int[] { 6, 12 };
            ArmorStats = new int[] { 8, 10 };
            Description = "Выбрать Фанатика. Накапливая ярость лечит себя" +
                $" Здоровье: {HealthStats[0]}-{HealthStats[1]}." +
                $" Урон: {DamageStats[0]}-{DamageStats[1]}." +
                $" Защита: {ArmorStats[0]}-{ArmorStats[1]}.";
        }

        public override string Description { get; protected set; }
        protected override int[] HealthStats { get; set; }
        protected override int[] DamageStats { get; set; }
        protected override int[] ArmorStats { get; set; }

        public override Warrior Create() =>
            new Fanatic(Name, GenerateHealth(), GenerateDamage(), GenerateArmor());
    }

    public class MageFabric : WarriorFabric
    {
        public MageFabric()
        {
            Name = "Маг";
            HealthStats = new int[] { 90, 100 };
            DamageStats = new int[] { 17, 20 };
            ArmorStats = new int[] { 1, 1 };
            Description = "Выбрать Мага. Использует огненный шар" +
                $" Здоровье: {HealthStats[0]}-{HealthStats[1]}." +
                $" Урон: {DamageStats[0]}-{DamageStats[1]}." +
                $" Защита: {ArmorStats[0]}-{ArmorStats[1]}.";
        }

        public override string Description { get; protected set; }
        protected override int[] HealthStats { get; set; }
        protected override int[] DamageStats { get; set; }
        protected override int[] ArmorStats { get; set; }

        public override Warrior Create() =>
            new Mage(Name, GenerateHealth(), GenerateDamage(), GenerateArmor());
    }

    public class ThiefFabric : WarriorFabric
    {
        public ThiefFabric()
        {
            Name = "Вор";
            HealthStats = new int[] { 70, 90 };
            DamageStats = new int[] { 15, 18 };
            ArmorStats = new int[] { 10, 14 };
            Description = "Выбрать Вора. Имеет шанс уклониться от атаки" +
                $" Здоровье: {HealthStats[0]}-{HealthStats[1]}." +
                $" Урон: {DamageStats[0]}-{DamageStats[1]}." +
                $" Защита: {ArmorStats[0]}-{ArmorStats[1]}.";
        }

        public override string Description { get; protected set; }
        protected override int[] HealthStats { get; set; }
        protected override int[] DamageStats { get; set; }
        protected override int[] ArmorStats { get; set; }

        public override Warrior Create() =>
            new Thief(Name, GenerateHealth(), GenerateDamage(), GenerateArmor());
    }

    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

    public abstract class Warrior : IDamageable
    {
        private float _minDamageMultiplier = 0.25f;

        protected Warrior(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public string Name { get; private set; }
        protected int Health { get; set; }
        protected int Damage { get; private set; }
        protected int Armor { get; private set; }

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
    }

    public class Fanatic : Warrior
    {
        private float _rageQuantity = 0f;
        private float _ragePerDamage = 1.3f;
        private float _triggerRageQuantity = 100;
        private int _heal = 30;
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
                Console.WriteLine($"{Name} исцеляет себя");
                _rageQuantity -= _triggerRageQuantity;
                Heal();
            }

            base.TakeDamage(damage);
        }

        private void Heal()
        {
            int health = Health + _heal;
            Health = Math.Min(health, _maxHealth);
        }
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

        public static int GenerateRandomValue(int[] values) =>
            s_random.Next(values[0], values[1]);
    }
}
