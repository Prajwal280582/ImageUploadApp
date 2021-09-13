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
        #region Class and Properties declaration
        protected ImageModel model = new ImageModel();

        //Logger is injected for logging informations, errors, warning
        [Inject]
        ILogger<ImageUpload> logger { set; get; }

        //Httpclient is injected for Web API
        [Inject]
        protected HttpClient http { set; get; }
        #endregion

        //Event captures once the browser picks all the image files
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

        //Onsubmit button Click event
        protected async Task OnSubmit()
        {
            try
            {
                logger.LogInformation("Upload is initiated");
                foreach (IBrowserFile file in model.selectedFiles)
                {
                    //Validation function
                    bool validate = ImageValidate(file);

                    //Validate check
                    if (validate)
                    {
                        //Setting up image format and resizing the image file
                        var browserFile = await file.RequestImageFileAsync(model.imgFormat, 800, 600);

                        //Filestream and  memmory stream is used to get from browser and place in form object
                        Stream fileStream = browserFile.OpenReadStream(model.maxFileSize);
                        MemoryStream ms = new();

                        await fileStream.CopyToAsync(ms);

                        //Used to display the Filename and image on the screen
                        model.imageUri = $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
                        model.imageUriList.Add(browserFile.Name, model.imageUri);

                        //Form object is used to pass the details to Web APi
                        using var form = new MultipartFormDataContent();
                        using var fileContent = new ByteArrayContent(ms.ToArray());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                        form.Add(fileContent, nameof(model.imageFile), browserFile.Name);

                        //Connecting to Web API to upload file
                        var response = await http.PostAsync("/Upload", form);

                        //Checking for Web API response
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
            catch (InvalidOperationException ex)  // Catching Specific Exceptions related to Web API
            {
                logger.LogError(ex.Message);
                model.message = $"Image Upload was unsuccessful";
            }
            catch (HttpRequestException ex) // Catching Specific Exceptions related to Web API
            {
                logger.LogError(ex.Message);
                model.message = $"Image Upload was unsuccessful";
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                model.message = $"Image Upload was unsuccessful";
            }
            logger.LogInformation("Upload Completed");
        }

        //Validations on file extension, max number of image upload and file size
        protected bool ImageValidate(IBrowserFile file)
        {           
            try
            {
                logger.LogInformation("Validation started");
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
                logger.LogInformation("Validation Completed");
                return true;
            }
            catch(Exception ex)  //Catching Exceptions
            {
                logger.LogError(ex.Message);
                model.message = $"Image Upload was unsuccessful";
                return false;
            }

        }

    }
}
