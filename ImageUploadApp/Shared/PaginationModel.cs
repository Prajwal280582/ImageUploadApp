using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploadApp.Shared
{
    public class PaginationModel
    {
        //Properties required for pagination

        #region Pagination properties
        public int totalRecords { set; get; }
        public int curPage = 1;       
        public int pageSize = 6;
        public int startPage { set; get; }
        public int endPage { set; get; }
        public int totalPages
        {
            get
            {
                return (int)Math.Ceiling(totalRecords / (decimal)pageSize);
            }
        }
        public int pagerSize;  //Required for file deletion functionality. Initialized equal to total pages
        
        //Properties for sorting in pagination
        public string sortColumnName = "ID";       
        public string sortDir = "DESC";
        #endregion

        // Methods required for pagination
        #region Pagination Methods
        //Used set the start and end pages based on the pager size when added or deleted
        public void SetPagerSize(string direction)
        {
            pagerSize = totalPages;
            if (direction == "forward" && endPage < totalPages)  //Forward Navigation
            {
                startPage = endPage + 1;
                if (endPage + pagerSize < totalPages)
                {
                    endPage = startPage + pagerSize - 1;
                }
                else
                {
                    endPage = totalPages;
                }
            }
            else if (direction == "back" && startPage > 1)  //Backward Navigation
            {
                endPage = startPage - 1;
                startPage = startPage - pagerSize;
            }
        }

        //Called when user click on Previous and Next in the browser pagination control
        public int NavigateToPage(string direction)
        {
            if (direction == "next")   //Activated when next is clicked
            {
                if (curPage < totalPages)
                {
                    if (curPage == endPage)
                    {
                        SetPagerSize("forward");
                    }
                    curPage += 1;
                }
            }
            else if (direction == "previous")   //Activated when previous is clicked
            {
                if (curPage > 1)
                {
                    if (curPage == startPage)
                    {
                        SetPagerSize("back");
                    }
                    curPage -= 1;
                }
            }
            return curPage;
        }
        #endregion
    }
}
