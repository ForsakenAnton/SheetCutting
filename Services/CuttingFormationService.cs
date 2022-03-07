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
                throw new Exception("Size of details are more than size of Sheet");
            }

            //for (int i = 0, j = 0; i < details.Count;)
            //{
            //    if ((int)details[i].BackgroundColor == j)
            //    {
            //        //details[i].BackgroundColor = (BackgroundColor)i;
            //        j++;
            //    }
            //    else
            //    {
            //        details[i].BackgroundColor = (BackgroundColor)j;
            //        j++;
            //        i++;
            //    }
            //}

            List<DetailViewModel> result = new List<DetailViewModel>(); // (!) list for return

            int remainingWidth = sheet.Width;
            int remainingHeight = sheet.Height;
            int maxHeightOfDetail = details.Max(d => d.Height);

            List<DetailInfoViewModel> sortedDetails = details
                .Select(d => d.Clone() as DetailInfoViewModel)
                //.OrderByDescending(d => d?.Height) // (!) Сортируем по высоте детали
                .OrderByDescending(d => (d?.Height * d?.Width)) // (!) сортируем по площади
                .ToList()!;

            foreach (var detail in sortedDetails)
            {
                while (detail.Count != 0)
                {
                    result.Add(new DetailViewModel
                    {
                        Width = detail.Width,
                        Height = detail.Height,
                        BackgroundColor = detail.BackgroundColor,
                    });
                    detail.Count--;
                }
            }
            return result;


            //while (!sortedDetails.All(d => d.Count == 0))
            //{
            //    bool isResizedMaxHeight = false;
            //    bool isLastDetailAdded = false; // (!!!)

            //    int firstMaxHeightInRow = sortedDetails
            //        .Where(d => d.Count != 0)
            //        .Max(d => d.Height);

            //    int moreOfOneDetailsInRow = 0; // счетчик двух и более деталей в ряду коллекции result
            //    for (int i = 0; i < sortedDetails.Count;)
            //    {
            //        // (!!!)
            //        //if (isLastDetailAdded 
            //        //    && moreOfOneDetailsInRow >= 2 
            //        //    && result[^1].Details.Count == 0)
            //        //{
            //        //    OptimizeFreeSpaceUnderDetail(
            //        //        result,
            //        //        sortedDetails,
            //        //        result[^1].Width,
            //        //        (firstMaxHeightInRow - result[^1].Height),
            //        //        i) ;
            //        //}
            //        // (!!!)


            //        if (remainingWidth >= sortedDetails[i].Width 
            //            && sortedDetails[i].Count > 0)
            //        {
            //            if (!isResizedMaxHeight)
            //            {
            //                maxHeightOfDetail = sortedDetails[i].Height;
            //                isResizedMaxHeight = true;
            //            }

            //            if (remainingHeight < maxHeightOfDetail)
            //            {
            //                _logger.LogInformation("Lack of space for details");
            //                throw new Exception("Lack of space for details");
            //            }

            //            if (!Validator.TryValidateObject(sortedDetails[i], new ValidationContext(sortedDetails[i]), null, true))
            //            {
            //                sortedDetails[i].Count = 0;
            //                break;
            //            }
            //            result.Add(new DetailViewModel 
            //            {
            //                Width = sortedDetails[i].Width, 
            //                Height = sortedDetails[i].Height,
            //                BackgroundColor = sortedDetails[i].BackgroundColor,
            //            });

            //            sortedDetails[i].Count--;
            //            remainingWidth -= sortedDetails[i].Width;

            //            moreOfOneDetailsInRow++;
            //            isLastDetailAdded = true; // (!!!)
            //            // _logger.LogInformation(remainingWidth);
            //        }
            //        else
            //        {
            //            i++;
            //        }
            //    }

            //    remainingWidth = sheet.Width;
            //    remainingHeight -= maxHeightOfDetail;

            //    moreOfOneDetailsInRow = 0;
            //    _logger.LogInformation("remainingHeight: " + remainingHeight);
            //}

            //return result;
        }


        //private void OptimizeFreeSpaceUnderDetail(
        //    List<DetailViewModel> result,
        //    List<DetailInfoViewModel>sortedDetails,
        //    int remainingWidth, 
        //    int remainingHeight, 
        //    int index)
        //{
        //    for (int i = index; i < sortedDetails.Count;)
        //    {
        //        if(sortedDetails[i].Count <= 0)
        //        {
        //            i++;
        //            continue;
        //        }

        //        if (remainingWidth >= sortedDetails[i].Width
        //            && remainingHeight > 0 /*sortedDetails[i].Height*/) // (!)
        //        {
        //            result[^1].Details.Add(new DetailViewModel()
        //            {
        //                Width = sortedDetails[i].Width,
        //                Height = sortedDetails[i].Height,
        //                BackgroundColor = sortedDetails[i].BackgroundColor, // (BackgroundColor)i
        //            });

        //            remainingWidth -= sortedDetails[i].Width;
        //           // remainingHeight -= sortedDetails[i].Height;
        //            sortedDetails[i].Count--;

        //            OptimizeFreeSpaceUnderDetail(result[^1].Details, sortedDetails, sortedDetails[i].Width, (remainingHeight - sortedDetails[i].Height), index); // recursion
        //        }
        //        else
        //        {
        //            i++;
        //        }
        //    }
        //}
    }
}
