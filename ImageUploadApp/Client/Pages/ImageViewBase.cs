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
        // Class object declaration
      
        protected PaginationModel pageModel = new PaginationModel();

        protected List<ImageGalleryModel> imageGalleryModel = new List<ImageGalleryModel>();

        //Http CLient for calling web api
        [Inject]
        protected HttpClient http { set; get; }

        //Used to display images on the browser screen
        //protected IEnumerable<Uri> lstUriRow1, lstUriRow2;

        protected IEnumerable<ImageGalleryModel> lstUriRow1, lstUriRow2;


        //Loading all the images when loading
        protected override async Task OnInitializedAsync()
        {
            var response = await http.GetAsync("/Mongo");

            if (response.IsSuccessStatusCode)
            {
                imageGalleryModel = await http.GetFromJsonAsync<List<ImageGalleryModel>>("/Mongo");
                pageModel.totalRecords = imageGalleryModel.Count();

                await Task.Run(() =>
                {
                    paginateRecords(pageModel.curPage);
                });
                pageModel.SetPagerSize("forward");
            }
        }

        //Click event of pagination calls the below event
        public async Task navigatePage(string direction)
        {
            int curPage = pageModel.NavigateToPage(direction);
            await refreshRecords(curPage);
        }

        //Records are fetched and page number is highligted after paginatiion button click
        protected async Task refreshRecords(int currentPage)
        {
            await Task.Run(() =>
            {
                paginateRecords(currentPage);
            });
            pageModel.curPage = currentPage;

        }

        //Fetching the list of records, storing it in object and displaying on the screen
        protected void paginateRecords(int currentPage)
        {

            lstUriRow1 = imageGalleryModel.Skip((currentPage - 1) * pageModel.pageSize).Take(pageModel.pageSize/2);
            lstUriRow2 = imageGalleryModel.Skip(((currentPage - 1) * pageModel.pageSize) + (pageModel.pageSize/2)).Take(pageModel.pageSize/2);

           
        }




    }
}
