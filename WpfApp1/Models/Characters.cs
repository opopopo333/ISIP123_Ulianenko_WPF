using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int MaxHp { get; protected set; }
        public int CurrentHp { get; set; }
        public int BaseAttack { get; protected set; }
        public int BaseDefense { get; protected set; }
        public string ImagePath { get; set; }

        public bool IsAlive => CurrentHp > 0;

        public virtual void TakeDamage(int damage)
        {
            CurrentHp = Math.Max(0, CurrentHp - damage);
        }
    }

    public class Player : Character
    {
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsFrozen { get; set; }

        public Player()
        {
            Name = "Герой";
            MaxHp = 1000;
            CurrentHp = 1000;
            BaseAttack = 5;
            BaseDefense = 2;
            EquippedWeapon = new Weapon("Деревянный меч", 2);
            EquippedArmor = new Armor("Рваная туника", 1);
        }

        public int TotalAttack => BaseAttack + (EquippedWeapon?.AttackBonus ?? 0);
        public int TotalDefense => BaseDefense + (EquippedArmor?.DefenseBonus ?? 0);

        public void HealFull() => CurrentHp = MaxHp;
    }
}
