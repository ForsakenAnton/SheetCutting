using System.ComponentModel.DataAnnotations;

namespace SheetCutting.Models.ViewModels
{
    public class DetailViewModel
    {
        public int Width { get; set; } = 50;

        public int Height { get; set; } = 50;

        public int RemainingWidth { get; set; }
        public int RemainingHeight { get; set; }
        public List<DetailViewModel> Details { get; set; } = new List<DetailViewModel>();
        public BackgroundColor BackgroundColor { get; set; }
    }
}
