using SheetCutting.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

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
            var errors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(sheet, new ValidationContext(sheet), errors, true))
            {
                string? message = "";
                foreach (var error in errors)
                {
                    message += error + "\n";
                }
                throw new Exception(message);
            }

            if (details == null)
            {
                throw new Exception("No single detail added");
            }

            if (details.Any(d => d.Width > sheet.Width || d.Height > sheet.Height))
            {
                _logger.LogInformation("Size of details are more than size of Sheet");
                //return null!;
                throw new Exception("Size of details are more than size of Sheet");
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
                bool isResizedMaxHeight = false;
                for (int i = 0; i < sortedDetails.Count;)
                {
                    if (remainingWidth >= sortedDetails[i].Width && sortedDetails[i].Count > 0)
                    {
                        if (!isResizedMaxHeight)
                        {
                            maxHeightOfDetail = sortedDetails[i].Height;
                            isResizedMaxHeight = true;
                        }

                        if (remainingHeight < maxHeightOfDetail)
                        {
                            _logger.LogInformation("Lack of space for details");
                            //return null!;
                            throw new Exception("Lack of space for details");
                        }

                        //if (sortedDetails[i].Width < 50 || sortedDetails[i].Height < 50)
                        if (!Validator.TryValidateObject(sortedDetails[i], new ValidationContext(sortedDetails[i]), null, true))
                        {
                            sortedDetails[i].Count = 0;
                            break;
                        }
                        result.Add(new DetailViewModel { Width = sortedDetails[i].Width, Height = sortedDetails[i].Height, BackgroundColor = (BackgroundColor)i } /*sortedDetails[i]*/);
                        sortedDetails[i].Count--;
                        remainingWidth -= sortedDetails[i].Width;
                        // _logger.LogInformation(remainingWidth);
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
