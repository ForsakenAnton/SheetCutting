using System.ComponentModel.DataAnnotations;

namespace SheetCutting.Models.ViewModels
{
    public class SheetViewModel
    {
        [Range(500, 1500, ErrorMessage = "Width must be not less than 0 and most than 1500")]
        public int Width { get; set; } = 1000;

        [Range(500, 1500, ErrorMessage = "Height must be not less than 0 and most than 1500")]
        public int Height { get; set; } = 500;
    }
}
