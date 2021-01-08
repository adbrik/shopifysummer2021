using System;
using System.Collections.Generic;

namespace Shopify2021Summer
{
    public class BucketModel
    {
        public string BucketName { get; set; }
        public string Url { get ; set; }
        public byte[] Image {get ; set; }
        public bool IsPrivate {get; set; }
        public byte[] Hash {get ; set; }

        public BucketModel(){
            this.IsPrivate = false;
        }
    }
}
