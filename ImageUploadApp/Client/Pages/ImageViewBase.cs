using ImageUploadApp.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploadApp.Client.Pages
{
    public class ImageViewBase : ComponentBase
    {
        protected ImageModel model = new ImageModel();
        protected PaginationModel pageModel = new PaginationModel();
     
        [Inject]
        protected HttpClient http { set; get; }

        protected IEnumerable<Uri> lstUriRow1, lstUriRow2;

 

        protected override async Task OnInitializedAsync()
        {
            var response = await http.GetAsync("/getall");

            if (response.IsSuccessStatusCode)
            {
                model.listUri = await http.GetFromJsonAsync<List<Uri>>("/getall");
                
                pageModel.totalRecords = model.listUri.Count();
                
                await Task.Run(() =>
                {
                    paginateRecords(pageModel.curPage);
                });
                pageModel.SetPagerSize("forward");

            }
        }
        public async Task navigatePage(string direction)
        {
            int curPage = pageModel.NavigateToPage(direction);
            await refreshRecords(curPage);
        }

        protected async Task refreshRecords(int currentPage)
        {
            await Task.Run(() =>
            {
                paginateRecords(currentPage);
            });
            pageModel.curPage = currentPage;

        }

        protected void paginateRecords(int currentPage)
        {
            lstUriRow1 = model.listUri.Skip((currentPage - 1) * pageModel.pageSize).Take(pageModel.pageSize/2);

            lstUriRow2 = model.listUri.Skip(((currentPage - 1) * pageModel.pageSize) + (pageModel.pageSize/2)).Take(pageModel.pageSize/2);

        }




    }
}
