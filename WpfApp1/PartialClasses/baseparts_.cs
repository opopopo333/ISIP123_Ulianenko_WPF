using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class basepart_
    {
        // 1. Полное имя (Производитель + Название)
        // Используем manufacturer_ (свойство) и name
        public string FullName => $"{this.manufacturer_?.name} {this.name}";

        // 2. Свойство для отображения характеристик в списке
        // Так как это одиночные объекты, FirstOrDefault() НЕ НУЖЕН
        public string DisplayInfo
        {
            get
            {
                if (this.cpu_ != null)
                    return $"Сокет: {this.cpu_.socket_?.name}, Ядер: {this.cpu_.numberofcores}, TDP: {this.cpu_.thermalpower}W";

                if (this.motherboard_ != null)
                    return $"Сокет: {this.motherboard_.socket_?.name}, RAM: {this.motherboard_.memorytype_?.name}";

                if (this.gpu_ != null)
                    return $"Память: {this.gpu_.videomemory}GB, Реком. БП: {this.gpu_.recommendpower}W";

                if (this.ram_ != null)
                    return $"{this.ram_.capacity}GB, {this.ram_.memorytype_?.name}, {this.ram_.ghz}MHz";

                if (this.powersupply_ != null)
                    return $"Мощность: {this.powersupply_.power}W";

                if (this.case_ != null)
                    return $"Размер: {this.case_.casesize_?.name}";

                // ОБНОВЛЕННЫЙ БЛОК ДЛЯ SSD / HDD
                if (this.storagedevice_ != null)
                {
                    // Берем название типа (SSD/HDD) из таблицы storagedevicetype_
                    string typeName = this.storagedevice_.storagedevicetype_?.name ?? "Накопитель";

                    // Берем название интерфейса (SATA/M.2/NVMe) из таблицы storagedeviceinterface_
                    string interfaceName = this.storagedevice_.storagedeviceinterface_?.name ?? "Интерфейс не указан";

                    return $"{typeName}: {this.storagedevice_.capacity}GB, {interfaceName}";
                }

                if (this.processorcooler_ != null)
                    return $"Уровень шума: {this.processorcooler_.noiselevel}";

                return "Комплектующее";
            }
        }
    }
}
