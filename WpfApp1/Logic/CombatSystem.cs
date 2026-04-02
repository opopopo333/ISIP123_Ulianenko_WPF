using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Logic
{
    public static class CombatSystem
    {
        // Высчитывает урон и возвращает текст для журнала событий
        public static string ApplyDamage(Character attacker, Character target, int rawDamage, bool ignoreArmor, bool targetDefending, string specialEffect = "")
        {
            int defense = ignoreArmor ? 0 : (target is Player p ? p.TotalDefense : target.BaseDefense);

            if (targetDefending && target is Player)
            {
                if (RNG.RollChance(40)) return $"{attacker.Name} промахивается! Игрок уклонился.";

                // Блок 70-100% брони
                double blockMultiplier = RNG.NextDouble() * 0.3 + 0.7;
                defense = (int)(defense * blockMultiplier);
            }

            int finalDamage = Math.Max(1, rawDamage - defense);
            target.TakeDamage(finalDamage);

            string log = $"{attacker.Name} бьет {target.Name} на {finalDamage} урона.";
            if (!string.IsNullOrEmpty(specialEffect)) log += $" ({specialEffect})";
            return log;
        }
    }
}
