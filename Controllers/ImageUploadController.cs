using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using LiteDB;

namespace Shopify2021Summer.Controllers
{
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly ILogger<ImageUploadController> _logger;

        public ImageUploadController(ILogger<ImageUploadController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{bucketId}")]
        public IEnumerable<string> Get(string bucketId)
        {
            using(var db = new LiteDatabase(@"data.db"))
            {
                var col = db.GetCollection<BucketModel>("buckets");

                var results = col.Query()
                        .Where(x => x.BucketName.Equals(bucketId))
                        .ToList();

                return results.Select(x => x.Url).ToList();
            }
        }

        [HttpPost]
        [Route("{bucketId}/add")]
        public ActionResult UploadImage(string bucketId, [FromBody] ImageModel model){

            using(var db = new LiteDatabase(@"data.db"))
            {
                var col = db.GetCollection<BucketModel>("buckets");

                var guidList = new List<string>();
                
                byte[] hashedBytes = new byte[0];

                if (model.IsPrivate == true && model.Password != null){
                    using (SHA256 sha256 = SHA256.Create()){
                        hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));  
                    }   
                }

                foreach (var x in model.ImageData){
                    var imageDataByteArray = Convert.FromBase64String(x);

                    var imageGuid = Guid.NewGuid().ToString("n").Substring(0, 8);

                    guidList.Add(imageGuid);

                    var bucket = new BucketModel{
                        BucketName = bucketId,
                        Image = imageDataByteArray,
                        Url = imageGuid
                    };

                    if (model.IsPrivate == true && model.Password != null){
                        bucket.IsPrivate = true;
                        bucket.Hash = hashedBytes;
                    } 

                    col.Insert(bucket);
                }

                return Ok(guidList);
            }
        }

        [HttpGet]
        [Route("{bucketId}/{url}")]
        public ActionResult GetImage(string bucketId, string url, [FromQuery] string access){
            try {
                using(var db = new LiteDatabase(@"data.db"))
                {
                    var col = db.GetCollection<BucketModel>("buckets");

                    var results = col.Query()
                        .Where(x => x.BucketName.Equals(bucketId))
                        .Where(x => x.Url == url)
                        .FirstOrDefault();

                    if (results != null){
                        
                        Debug.WriteLine(access);
                        if (results.IsPrivate){
                            using (SHA256 sha256 = SHA256.Create()){
                                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(access)); 
                                if (!(hashedBytes.SequenceEqual(results.Hash))){
                                    throw new Exception();
                                }
                            }
                        }
                        return new FileContentResult(results.Image,"image/png");
                    }
                    else {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
                return new NotFoundResult();
            }
        }
    }
}
