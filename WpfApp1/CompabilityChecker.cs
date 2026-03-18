using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class CompatibilityChecker
    {
        public static List<string> Check(List<basepart_> selectedParts)
        {
            List<string> errors = new List<string>();

            
            var cpu = selectedParts.FirstOrDefault(p => p.cpu_ != null)?.cpu_;
            var mb = selectedParts.FirstOrDefault(p => p.motherboard_ != null)?.motherboard_;
            var ram = selectedParts.FirstOrDefault(p => p.ram_ != null)?.ram_;
            var gpu = selectedParts.FirstOrDefault(p => p.gpu_ != null)?.gpu_;
            var ps = selectedParts.FirstOrDefault(p => p.powersupply_ != null)?.powersupply_;
            var pcCase = selectedParts.FirstOrDefault(p => p.case_ != null)?.case_;
            var cooler = selectedParts.FirstOrDefault(p => p.processorcooler_ != null)?.processorcooler_;

            
            if (cpu != null && mb != null)
            {
                if (cpu.socketid != mb.socketid)
                    errors.Add($"❌ Сокеты не совпадают: CPU ({cpu.socket_?.name}) и MB ({mb.socket_?.name})");
            }

            
            if (mb != null && ram != null)
            {
                if (mb.memorytypeid != ram.memorytypeid)
                    errors.Add($"❌ Несовместимый тип памяти: MB требует {mb.memorytype_?.name}, выбрана {ram.memorytype_?.name}");
            }

            
            if (gpu != null && ps != null)
            {
                if (ps.power < gpu.recommendpower)
                    errors.Add($"❌ Недостаточная мощность БП: нужно {gpu.recommendpower}W, есть {ps.power}W");
            }

            
            if (mb != null && pcCase != null)
            {
                
                bool isCompatible = pcCase.boardformfactorcase_.Any(bf => bf.formfactorid == mb.formfactorid);
                if (!isCompatible)
                    errors.Add("❌ Материнская плата не подходит по размеру к корпусу!");
            }

            
            if (cpu != null && cooler != null)
            {
                
                bool isSocketSupported = cooler.socketprocessorcooler_.Any(s => s.socketid == cpu.socketid);
                if (!isSocketSupported)
                    errors.Add("❌ Кулер не поддерживает данный сокет!");
            }

            return errors;
        }
    }
}
