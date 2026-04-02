using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }

    public class Weapon : Item
    {
        public int AttackBonus { get; set; }
        public Weapon(string name, int atk) { Name = name; AttackBonus = atk; ImagePath = "Images/weapon.png"; }
    }

    public class Armor : Item
    {
        public int DefenseBonus { get; set; }
        public Armor(string name, int def) { Name = name; DefenseBonus = def; ImagePath = "Images/armor.png"; }
    }

    public class Potion : Item
    {
        public Potion() { Name = "Зелье здоровья"; ImagePath = "Images/potion.png"; }
    }
}
