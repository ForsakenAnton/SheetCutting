using SheetCutting.Models.ViewModels;

namespace SheetCutting.Services
{
    public interface ICuttingFormationService
    {
        public List<DetailViewModel> Cut(SheetViewModel sheet, List<DetailInfoViewModel> details);
    }
}
