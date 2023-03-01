using Microsoft.AspNetCore.Mvc.Rendering;

namespace CivilCalc.Areas.CAL_Category.Models
{
    public class PagedListPagerModel
    {
        /*******************************************************************
         *	Pageinng
         *******************************************************************/
        public int? TotalItems { get; set; }
        public int? ItemFrom { get; set; }
        public int? ItemTo { get; set; }

        public int PageSize { get; set; }
        public int CurrentPageNo { get; set; }
        public int TotalPages { get; set; }

        public int StartPageNo { get; set; }
        public int EndPageNo { get; set; }

        public string PageInfo { get; set; }
        public IList<SelectListItem> PageSizeList { get; set; }

        public string PageNoTargetInput { get; set; }

        public PagedListPagerModel(int? totalItems, int? pageNo, int? pageSize = 10)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = pageNo != null ? (int)pageNo : 1;

            var startPage = currentPage - 3;
            var endPage = currentPage + 2;

            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 5;
                }
            }

            int? vLastPageRecords = totalItems % pageSize;

            int? itemFrom = 0;
            int? itemTo = 0;
            if (totalItems > 0)
            {
                itemFrom = (pageNo - 1) * pageSize + 1;
                itemTo = (pageNo - 1) * pageSize + (pageNo == totalPages ? vLastPageRecords : pageSize);
            }

            TotalItems = totalItems;
            ItemFrom = itemFrom;
            ItemTo = itemTo;

            PageSize = (int)pageSize;
            CurrentPageNo = currentPage;
            TotalPages = totalPages;

            StartPageNo = startPage;
            EndPageNo = endPage;

            if (currentPage == totalPages)
            {
                ItemTo = itemFrom + totalItems % pageSize - 1;
            }
        }
    }
}
