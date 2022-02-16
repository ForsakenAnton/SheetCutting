namespace SheetCutting.Models.ViewModels
{
    public class DetailInfoViewModel : ICloneable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public BackgroundColor BackgroundColor { get; set; }

        public int Count { get; set; }

        // public List<DetailInfoViewModel>? Details { get; set; } 

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
