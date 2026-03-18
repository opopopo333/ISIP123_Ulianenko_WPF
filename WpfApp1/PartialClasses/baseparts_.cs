using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class basepart_
    {
        
        public string FullName => $"{this.manufacturer_?.name} {this.name}";

        
        public string DisplayInfo
        {
            get
            {
                if (this.cpu_ != null)
                    return $"Сокет: {this.cpu_.socket_?.name}, Ядер: {this.cpu_.numberofcores}, TDP: {this.cpu_.thermalpower}W, Базовая частота: {this.cpu_.basecorefrequency}Mhz, Макс. частота: {this.cpu_.maxcorefrequency}Mhz, L3: {this.cpu_.cachel3}mb";

                if (this.motherboard_ != null)
                    return $"Сокет: {this.motherboard_.socket_?.name}, RAM: {this.motherboard_.memorytype_?.name}, Слоты Ram: {this.motherboard_.memoryslots}, PCOslots: {this.motherboard_.pcislots}, SATAports: {this.motherboard_.sataports},USBports: {this.motherboard_.usbports}, FormFactor: {this.motherboard_.formfactor_.name}" ;

                if (this.gpu_ != null)
                    return $"Память: {this.gpu_.videomemory}GB, Реком. БП: {this.gpu_.recommendpower}W, Частота: {this.gpu_.chipfrequency}mhz, PCI: {this.gpu_.gpuinterface_.name}, Шина: {this.gpu_.memorybus}bit";

                if (this.ram_ != null)
                    return $"{this.ram_.capacity}GB, {this.ram_.memorytype_?.name}, {this.ram_.ghz}MHz, Тайминги:{this.ram_.timings}, Плашек: {this.ram_.count}";

                if (this.powersupply_ != null)
                    return $"Мощность: {this.powersupply_.power}W, Сертификат: {this.powersupply_.certificate_.name}, {this.powersupply_.fandimension_.name}";

                if (this.case_ != null)
                    return $"Размер: {this.case_.casesize_?.name}, Вентиляторы: {this.case_.fans}, Вентиляторы: {this.case_.fans}";

                
                if (this.storagedevice_ != null)
                {
                    
                    string typeName = this.storagedevice_.storagedevicetype_?.name ?? "Накопитель";

                    
                    string interfaceName = this.storagedevice_.storagedeviceinterface_?.name ?? "Интерфейс не указан";
              
                    return $"{typeName}: {this.storagedevice_.capacity}GB, {interfaceName},";
                }

                if (this.processorcooler_ != null)
                    return $"Уровень шума: {this.processorcooler_.noiselevel}, Трубок: {this.processorcooler_.heatpipes}, Макс: {this.processorcooler_.maxspeed}, Мин: {this.processorcooler_.minspeed}, {this.processorcooler_.fandimension_.name}  ";

                return "Комплектующее";
            }
        }
    }
}
