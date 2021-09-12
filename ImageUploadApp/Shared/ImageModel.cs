using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageUploadApp.Shared
{
    public class ImageModel
    {
        public readonly List<string> supportedTypes = new List<string> { "jpg", "jpeg", "png" };
        public readonly int maxNumberofFiles = 5;
        public readonly long maxFileSize = 1024 * 1024 * 10;

        public IReadOnlyList<IBrowserFile> selectedFiles { set; get; }

        public readonly string imgFormat = "image/jpeg";

        public string imageUri;

        public Dictionary<string, string> imageUriList = new Dictionary<string, string>();
        public IFormFile imageFile { set; get; }

        public string message = "No file(s) selected";

        public List<Uri> listUri { set; get; }

    }
}
