
namespace Console_Html_Parser.Models.romstal.ua
{
    public class BimetallicRadiator
    {
        public string Brand { get; set; }
        public string Code { get; set; }
        public string ManufacturerCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Weight { get; set; }
        public int TestPressureInBar { get; set; }
        public int SectionHeightInMM { get; set; }
        public int HeightInMM { get; set; }
        public int GuaranteeInYears { get; set; }
        public int SectionDepthInMM { get; set; }
        public string Color { get; set; }
        public int CenterDistanceInMM { get; set; }
        public int MaxCoolantTemperatureInCelsium { get; set; }
        public int WorkingPressureInBar { get; set; }
        public int HeatOutputWithDefaultTemperatureInW { get; set; }
        private static int DefaultTemperatureInCelsium { get; } = 70;
        public string Description { get; set; }

        public static IEnumerable<string> PropertiesNames => _propertiesNames;

        private static List<string> _propertiesNames = new List<string>()
        {
                "Бренд",
                "Артикул",
                "Артикул производителя",
                "Название",
                "Цена",
                "Вес",
                "Тестовое давление, бар",
                "Высота секции, мм",
                "Высота, мм",
                "Гарантия, лет",
                "Глубина секции, мм",
                "Цвет",
                "Сежосевое расстояние, мм",
                "Макс. температура теплоносителя, С",
                "Рабочее давление, бар",
                $"Теплоотдача dt={DefaultTemperatureInCelsium}, Вт",
                "Описание"
        };

        public override string ToString()
        {
            return Code;
        }
    }
}
