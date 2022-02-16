namespace SheetCutting.Models.ViewModels
{
    public class IndexViewModel
    {
#nullable disable
        public SheetViewModel Sheet { get; set; }
        public List<DetailInfoViewModel> DetailsInfo { get; set; }
        public List<DetailViewModel> CuttedDetails { get; set; }
#nullable restore
    }
}
