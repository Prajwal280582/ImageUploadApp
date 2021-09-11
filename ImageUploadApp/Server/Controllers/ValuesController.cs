using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImageUploadApp.Shared;

namespace ImageUploadApp.Server.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public ValuesController(IHttpClientFactory clientFactory)
        {

            _clientFactory = clientFactory;
        }

        [HttpPost]
        
        public async Task<IActionResult> PostFile([FromForm] IFormFile uploadfile)
        {
            var client = _clientFactory.CreateClient();

            ImageModel model = new();

                model.imageFile = uploadfile;
                using (var memoryStream = new MemoryStream())
                {
                    await model.imageFile.CopyToAsync(memoryStream);
                    using var form = new MultipartFormDataContent();
                    using var fileContent = new ByteArrayContent(memoryStream.ToArray());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    //Get the file steam from the multiform data uploaded from the browser

                    form.Add(fileContent, nameof(model.imageFile), model.imageFile.FileName);
                    var response = await client.PostAsync($"https://imageloadupload.azurewebsites.net/upload", form);
                    if (response.IsSuccessStatusCode)
                    {
                    }
                


            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return new OkResult();
        }
    }
}
