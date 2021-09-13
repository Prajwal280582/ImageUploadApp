using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageUploadApp.Shared
{
    public class ImageModel
    {
        //Properties for Image uploads

        #region Image Properties
        //Readonly property. COnfiguration can be changed based on necessity
        public readonly List<string> supportedTypes = new List<string> { "jpg", "jpeg", "png" };
        public readonly int maxNumberofFiles = 5;
        public readonly long maxFileSize = 1024 * 1024 * 10;
        
        //Readonly property
        public IReadOnlyList<IBrowserFile> selectedFiles { set; get; }
        public readonly string imgFormat = "image/jpeg";

        public string imageUri;
        public Dictionary<string, string> imageUriList = new Dictionary<string, string>();
        public IFormFile imageFile { set; get; }

        //Property for dispaying messages on the browser screen
        public string message = "No file(s) selected";
        public List<Uri> listUri { set; get; }

        #endregion

    }
}
