using System.ComponentModel.DataAnnotations;

namespace SheetCutting.Models.ViewModels
{
    public class DetailInfoViewModel : ICloneable
    {
        [Range(50, double.PositiveInfinity, ErrorMessage = "Width must be not less than 50")]
        public int Width { get; set; } = 50;

        [Range(50, double.PositiveInfinity, ErrorMessage = "Height must be not less than 50")]
        public int Height { get; set; } = 50;
        public BackgroundColor BackgroundColor { get; set; }// = BackgroundColor.blue;

        public int Count { get; set; }

        // public List<DetailInfoViewModel>? Details { get; set; } 

        public object Clone()
        {
            //return MemberwiseClone();
            return new DetailInfoViewModel()
            {
                Width = Width,
                Height = Height,
                BackgroundColor = BackgroundColor,
                Count = Count,
            };
        }
    }
}
