using System.ComponentModel.DataAnnotations;

namespace SheetCutting.Models.ViewModels
{
    public class DetailViewModel
    {
        public int Width { get; set; } = 50;

        public int Height { get; set; } = 50;
        public BackgroundColor BackgroundColor { get; set; }
    }
}
