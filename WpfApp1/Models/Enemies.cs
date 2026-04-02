using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Logic;

namespace WpfApp1.Models
{
    public abstract class Enemy : Character
    {
        public abstract string Attack(Player target, bool targetIsDefending);
    }

    public class Goblin : Enemy
    {
        public Goblin() { Name = "Гоблин"; MaxHp = CurrentHp = 30; BaseAttack = 12; BaseDefense = 3; ImagePath = "Images\\goblin.jpg"; }

        public override string Attack(Player target, bool targetIsDefending)
        {
            int damage = BaseAttack;
            bool isCrit = RNG.RollChance(20);
            if (isCrit) damage *= 2;
            return CombatSystem.ApplyDamage(this, target, damage, false, targetIsDefending, isCrit ? "КРИТ!" : "");
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton() { Name = "Скелет"; MaxHp = CurrentHp = 40; BaseAttack = 10; BaseDefense = 5; ImagePath = "Images\\skeleton.jpg"; }

        public override string Attack(Player target, bool targetIsDefending)
        {
            return CombatSystem.ApplyDamage(this, target, BaseAttack, true, targetIsDefending, "Игнор брони!");
        }
    }

    public class Mage : Enemy
    {
        public Mage() { Name = "Маг"; MaxHp = CurrentHp = 25; BaseAttack = 15; BaseDefense = 2; ImagePath = "Images\\mage.jpg"; }

        public override string Attack(Player target, bool targetIsDefending)
        {
            if (RNG.RollChance(15)) target.IsFrozen = true;
            string special = target.IsFrozen ? "ЗАМОРОЗКА!" : "";

            return CombatSystem.ApplyDamage(this, target, BaseAttack, false, targetIsDefending, special);
        }
    }

    public class BossVVG : Goblin
    {
        public BossVVG()
        {
            Name = "Босс: ВВГ";
            MaxHp = CurrentHp = (int)(30 * 2.0);
            BaseAttack = (int)(12 * 1.5);
            BaseDefense = (int)(3 * 1.2);
            ImagePath = "Images/boss_vvg.png";
        }
        public override string Attack(Player target, bool targetIsDefending)
        {
            int damage = BaseAttack;
            bool isCrit = RNG.RollChance(30);
            if (isCrit) damage *= 2;

            return CombatSystem.ApplyDamage(this, target, damage, false, targetIsDefending, isCrit ? "КРИТ!" : "");
        }
    }

    public class BossKowalski : Skeleton
    {
        public BossKowalski()
        {
            Name = "Босс: Ковальский";
            MaxHp = CurrentHp = (int)(40 * 2.5);
            BaseAttack = (int)(10 * 1.3);
            BaseDefense = (int)(5 * 1.4);
            ImagePath = "Images/boss_kowalski.png";
        }
       
    }

    public class BossArchmageCPP : Mage
    {
        public BossArchmageCPP()
        {
            Name = "Босс: Архимаг C++";
            MaxHp = CurrentHp = (int)(25 * 1.8);
            BaseAttack = (int)(15 * 1.6);
            BaseDefense = (int)(2 * 1.1);
            ImagePath = "Images/boss_cpp.png";
        }
        public override string Attack(Player target, bool targetIsDefending)
        {
            if (RNG.RollChance(15 + 10)) target.IsFrozen = true; 
            string special = target.IsFrozen ? "ЗАМОРОЗКА!" : "";
            return CombatSystem.ApplyDamage(this, target, BaseAttack, false, targetIsDefending, special);
        }
    }

    public class BossPestovCMM : Skeleton 
    {
        public BossPestovCMM()
        {
            Name = "Босс: Пестов C--";
            MaxHp = CurrentHp = (int)(40 * 1.3);
            BaseAttack = (int)(10 * 1.8);
            BaseDefense = (int)(5 * 0.6);
            ImagePath = "Images/boss_pestov.png";
        }
        public override string Attack(Player target, bool targetIsDefending)
        {
            if (RNG.RollChance(15)) target.IsFrozen = true;
            string special = "Игнор брони" + (target.IsFrozen ? " + ЗАМОРОЗКА!" : "");
            return CombatSystem.ApplyDamage(this, target, BaseAttack, true, targetIsDefending, special);
        }
    }
}
