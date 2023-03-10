namespace UwpSample.Models
{
    public class Theme
    {
        public static Theme Red => new Theme { Name = "Red" };

        public static Theme Blue => new Theme { Name = "Blue" };

        public static Theme Default => Theme.Red;

        public string Name { get; set; }
    }
}
