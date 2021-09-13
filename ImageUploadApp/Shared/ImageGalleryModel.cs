using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploadApp.Shared
{
    public class ImageGalleryModel
    {
        //Properties for Image Display
        public string ImageName { set; get; }
        public Uri ImageUri { set; get; }
        public Uri ImageThumbnailUri { set; get; }
        public string ImageLength { set; get; }
        public string ImageType { set; get; }
        public DateTime ImageUploadDate { set; get; }


    }
}
