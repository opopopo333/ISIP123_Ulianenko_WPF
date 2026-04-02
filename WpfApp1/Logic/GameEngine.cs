using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Logic
{
    public class GameEngine
    {
        public Player CurrentPlayer { get; private set; }
        public int Floor { get; private set; }
        public ObservableCollection<string> EventLog { get; private set; }
        public List<Enemy> CurrentEnemies { get; private set; }
        public Item DroppedItem { get; private set; }

        public enum GameState { MainMenu, Exploring, InCombat, Looting, GameOver }
        public GameState State { get; private set; }

        public GameEngine()
        {
            EventLog = new ObservableCollection<string>();
            State = GameState.MainMenu;
        }

        public void StartNewGame()
        {
            CurrentPlayer = new Player();
            Floor = 0;
            EventLog.Clear();
            Log("Игра началась!");
            NextFloor();
        }

        public void NextFloor()
        {
            Floor++;
            CurrentPlayer.IsFrozen = false;

            if (Floor % 10 == 0)
            {
                GenerateBoss();
            }
            else
            {
                if (RNG.RollChance(50)) GenerateEnemies();
                else GenerateChest();
            }
        }

        private void GenerateEnemies()
        {
            State = GameState.InCombat;
            CurrentEnemies = new List<Enemy>();
            int enemyCount = 1; // 1-3 врага

            for (int i = 0; i < enemyCount; i++)
            {
                int type = RNG.Next(0, 3);
                if (type == 0) CurrentEnemies.Add(new Goblin());
                else if (type == 1) CurrentEnemies.Add(new Skeleton());
                else CurrentEnemies.Add(new Mage());
            }
            Log($"Этаж {Floor}: Враги появились! (Количество: {enemyCount})");
        }

        private void GenerateBoss()
        {
            State = GameState.InCombat;
            int bossType = RNG.Next(0, 4);
            Enemy boss;

            switch (bossType)
            {
                case 0: boss = new BossVVG(); break;
                case 1: boss = new BossKowalski(); break;
                case 2: boss = new BossArchmageCPP(); break;
                default: boss = new BossPestovCMM(); break;
            }

            CurrentEnemies = new List<Enemy> { boss };
            Log($"ВНИМАНИЕ! На этаже {Floor} появился {boss.Name}!");
        }

        private void GenerateChest()
        {
            State = GameState.Looting;
            int dropType = RNG.Next(0, 3);
            if (dropType == 0) DroppedItem = new Potion();
            else if (dropType == 1) DroppedItem = new Weapon("Меч героя", RNG.Next(3, 10));
            else DroppedItem = new Armor("Стальная броня", RNG.Next(3, 8));
            Log($"Этаж {Floor}: Найден сундук! Внутри: {DroppedItem.Name}.");
        }

        public void PlayerTurn(bool defend, Enemy target = null)
        {
            if (CurrentPlayer.IsFrozen)
            {
                Log("Вы заморожены и пропускаете ход!");
                CurrentPlayer.IsFrozen = false;
            }
            else if (!defend && target != null)
            {
                string log = CombatSystem.ApplyDamage(CurrentPlayer, target, CurrentPlayer.TotalAttack, false, false);
                Log(log);
                if (!target.IsAlive)
                {
                    Log($"{target.Name} убит!");
                    CurrentEnemies.Remove(target);
                }
            }
            else
            {
                Log("Игрок уходит в глухую оборону.");
            }

            if (CurrentEnemies.Count == 0)
            {
                Log("Комната зачищена!");
                NextFloor();
                return;
            }

            EnemyTurn(defend);
        }

        private void EnemyTurn(bool playerDefended)
        {
            foreach (var enemy in CurrentEnemies.ToList())
            {
                Log(enemy.Attack(CurrentPlayer, playerDefended));
            }

            if (!CurrentPlayer.IsAlive)
            {
                Log("ВЫ ПОГИБЛИ!");
                State = GameState.GameOver;
            }
        }

        public void HandleLoot(bool take)
        {
            if (take)
            {
                if (DroppedItem is Potion)
                {
                    CurrentPlayer.HealFull();
                    Log("Вы выпили зелье. Здоровье восстановлено.");
                }
                else if (DroppedItem is Weapon w) CurrentPlayer.EquippedWeapon = w;
                else if (DroppedItem is Armor a) CurrentPlayer.EquippedArmor = a;
                Log($"Вы взяли {DroppedItem.Name}.");
            }
            else
            {
                Log("Вы прошли мимо предмета.");
            }
            DroppedItem = null;
            NextFloor();
        }

        private void Log(string message)
        {
            EventLog.Insert(0, message); 
        }
        public string GetLootComparison()
        {
            if (DroppedItem is Weapon newWep)
            {
                int currentAtk = CurrentPlayer.EquippedWeapon?.AttackBonus ?? 0;
                return $"Новое оружие: {newWep.Name}\nАтака: {newWep.AttackBonus} (Ваша: {currentAtk})";
            }
            if (DroppedItem is Armor newArm)
            {
                int currentDef = CurrentPlayer.EquippedArmor?.DefenseBonus ?? 0;
                return $" Новая броня: {newArm.Name}\nЗащита: {newArm.DefenseBonus} (Ваша: {currentDef})";
            }
            if (DroppedItem is Potion)
            {
                return "Зелье здоровья\n(Полностью восстановит ваше HP)";
            }
            return "";
        }
    }
}
