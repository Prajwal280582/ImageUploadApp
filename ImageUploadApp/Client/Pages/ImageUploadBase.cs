using ImageUploadApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace ImageUploadApp.Client.Pages
{
    public class ImageUploadBase : ComponentBase
    {
        protected ImageModel model = new ImageModel();

        [Inject]
        ILogger<ImageUpload> logger { set; get; }

        [Inject]
        protected HttpClient http { set; get; }



        protected void OnInputFileChange(InputFileChangeEventArgs e)
        {
            try
            {
                model.selectedFiles = e.GetMultipleFiles();
                model.message = $"{model.selectedFiles.Count} file(s) selected";
                

            }
            catch (Exception ex)
            {
                model.message = $"{ex.Message}";
            }

        }
        protected async Task OnSubmit()
        {

            logger.LogInformation("Upload is initiated");
            try
            {

                foreach (IBrowserFile file in model.selectedFiles)
                {
                    bool validate = ImageValidate(file);

                    if (validate)
                    {

                        var browserFile = await file.RequestImageFileAsync(model.imgFormat, 800, 600);

                        Stream fileStream = browserFile.OpenReadStream(model.maxFileSize);
                        MemoryStream ms = new();

                        await fileStream.CopyToAsync(ms);

                        model.imageUri = $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
                        model.imageUriList.Add(browserFile.Name, model.imageUri);

                        using var form = new MultipartFormDataContent();
                        using var fileContent = new ByteArrayContent(ms.ToArray());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                        form.Add(fileContent, nameof(model.imageFile), browserFile.Name);

                        var response = await http.PostAsync("/upload", form);

                        if (response.IsSuccessStatusCode)
                        {
                            model.message = $"Below {model.selectedFiles.Count} file(s) uploaded on server";

                        }
                        else
                        {
                            model.message = $"Below {model.selectedFiles.Count} file(s) not uploaded on server";
                        }
                    }


                }

            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                model.message = $"Image Upload was unsuccessful";
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex.Message);
                model.message = $"Image Upload was unsuccessful";
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                model.message = $"Image Upload was unsuccessful";
            }

        }

        protected bool ImageValidate(IBrowserFile file)
        {
            string[] ext = file.Name.Split(".");
            if (!model.supportedTypes.Contains(ext[1].ToLower()))
            {
                model.message = $"Choose image file extension. Application does not allow {ext[1].ToString()} extension";
                return false;
            }

            if (model.selectedFiles.Count > model.maxNumberofFiles)
            {
                model.message = $"Application allows {model.maxNumberofFiles} images to upload in each instance ";
                return false;
            }

            if (file.Size > model.maxFileSize)
            {

                model.message = $"One or more selected images size exceeds the {model.maxFileSize} allowed max file size by the application";
                return false;
            }

            return true;

        }

    }
}
