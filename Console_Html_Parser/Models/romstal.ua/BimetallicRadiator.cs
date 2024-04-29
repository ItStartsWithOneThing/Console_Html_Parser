
using Console_Html_Parser.StaticTools;

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
        public int DefaultTemperatureInCelsium { get; set; } = 70;
        public string Description { get; set; }
        private bool IsAvailable { get; set; } = true;

        public BimetallicRadiator(string resultHtml)
        {
            Parse(resultHtml);
        }

        private void Parse(string html)
        {
            Brand = Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-brand=\"", "\"");
            Code = Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-article=\"", "\"");
            ManufacturerCode = Tools.FindValue(html, "<p class=\"item__info__side-title\">Код виробника:</p>", "<p class=\"item__info__side-sub\">", "<");
            Name = Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-name=\"", "\"");
            Price = decimal.Parse(Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-price=\"", "\"").Replace('.', ','));
            Weight = double.Parse(Tools.FindValue(html, "class=\"spec__title\">Вага", "<span class=\"spec__sub\">", "<").Replace('.', ','));
            TestPressureInBar = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Випробний тиск, бар</span>", "<span class=\"spec__sub\">", "<"));
            SectionHeightInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Висота секції, мм</span>", "<span class=\"spec__sub\">", "<"));
            HeightInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Висота, мм</span>", "class=\"link\">", "<"));
            GuaranteeInYears = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Гарантія, років</span>", "<span class=\"spec__sub\">", "<"));
            SectionDepthInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Глибина секції, мм</span>", "<span class=\"spec__sub\">", "<"));
            Color = Tools.FindValue(html, "<span class=\"spec__title\">Колір</span>", "<span class=\"spec__sub\">", "<");
            CenterDistanceInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Міжосьова відстань, мм</span>", "<span class=\"spec__sub\">", "<"));
            MaxCoolantTemperatureInCelsium = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Макс. температура теплоносія,", "<span class=\"spec__sub\">", "<"));
            WorkingPressureInBar = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Робочий тиск, бар</span>", "<span class=\"spec__sub\">", "<"));
            HeatOutputWithDefaultTemperatureInW = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Тепловіддача", "<span class=\"spec__sub\">", "<"));
            Description = Tools.FindValue(html, "<h4>Опис</h4>", "<p>", "<");
        }
    }
}
