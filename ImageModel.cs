using System;

namespace Shopify2021Summer
{
    public class ImageModel
    {
        public string[] ImageData { get; set; }

        public bool IsPrivate {get ; set; }

        public string Password {get; set;}
        public ImageModel(){
            this.IsPrivate = false;
        }
    }
}
