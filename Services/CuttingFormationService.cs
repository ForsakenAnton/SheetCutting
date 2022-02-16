using SheetCutting.Models.ViewModels;

namespace SheetCutting.Services
{
    public class CuttingFormationService : ICuttingFormationService


    {
        private readonly ILogger<CuttingFormationService> _logger;

        public CuttingFormationService(ILogger<CuttingFormationService> logger)
        {
            _logger = logger;
        }

        public List<DetailViewModel> Cut(SheetViewModel sheet, List<DetailInfoViewModel> details)
        {
            if (details.Any(d => d.Width > sheet.Width || d.Height > sheet.Height))
            {
                _logger.LogInformation("Size of details are more than size of Sheet");
                return null!;
            }

            List<DetailViewModel> result = new List<DetailViewModel>(); // (!) list for return

            int remainingWidth = sheet.Width;
            int remainingHeight = sheet.Height;
            int maxHeightOfDetail = details.Max(d => d.Height);

            List<DetailInfoViewModel> sortedDetails = details
                .Select(d => d.Clone() as DetailInfoViewModel)
                .OrderByDescending(d => d?.Height) // (!) Сортируем по высоте детали
                .ThenByDescending(d => (d?.Height * d?.Width)) // (!) далее сортируем по площади
                .ToList()!;


            while (!sortedDetails.All(d => d.Count == 0))
            {
                if (remainingHeight < maxHeightOfDetail)
                {
                    _logger.LogInformation("Lack of space for details");
                    return null!;
                }

                bool isResizedMaxHeight = false;
                for (int i = 0; i < sortedDetails.Count;)
                {
                    if (remainingWidth >= sortedDetails[i].Width && sortedDetails[i].Count != 0)
                    {
                        result.Add(new DetailViewModel { Width = sortedDetails[i].Width, Height = sortedDetails[i].Height, BackgroundColor = (BackgroundColor) i } /*sortedDetails[i]*/);
                        sortedDetails[i].Count--;
                        remainingWidth -= sortedDetails[i].Width;
                        // _logger.LogInformation(remainingWidth);
                        if (!isResizedMaxHeight)
                        {
                            maxHeightOfDetail = sortedDetails[i].Height;
                            isResizedMaxHeight = true;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }

                remainingWidth = sheet.Width;
                remainingHeight -= maxHeightOfDetail; //sortedDetails.Max(d => d.Height);
                _logger.LogInformation("remainingHeight: " + remainingHeight);
            }

            return result;
        }
    }
}
