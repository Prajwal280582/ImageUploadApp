using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploadApp.Shared
{
    public class PaginationModel
    {

        public int totalRecords { set; get; }
        public int curPage = 1;
        
        public int pageSize = 10;
        public int startPage { set; get; }
        public int endPage { set; get; }

        public int totalPages
        {
            get
            {
                return (int)Math.Ceiling(totalRecords / (decimal)pageSize);
            }
        }

        public int pagerSize;

        public string sortColumnName = "ID";
        
        public string sortDir = "DESC";

        public void SetPagerSize(string direction)
        {
            pagerSize = totalPages;

            if (direction == "forward" && endPage < totalPages)
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
            else if (direction == "back" && startPage > 1)
            {
                endPage = startPage - 1;
                startPage = startPage - pagerSize;
            }
        }

        public int NavigateToPage(string direction)
        {
            if (direction == "next")
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
            else if (direction == "previous")
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
    }
}
