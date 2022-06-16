using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazonWebServiceDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;

        public BucketsController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (bucketExists) return BadRequest($"Bucket {bucketName} already exists");
            await _s3Client.PutBucketAsync(bucketName);
            return Ok($"Bucket {bucketName} created");
        }

        [HttpGet("getBuckets")]
        public async Task<IActionResult> GetAllAvaliableBuckets()
        {
            var data = await _s3Client.ListBucketsAsync();
            var buckets = data.Buckets.Select( b=> { return b.BucketName; });
            return Ok(buckets);
        }

        [HttpDelete("deleteBucket")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            await _s3Client.DeleteBucketAsync(bucketName);
            return NoContent();
        }


    
    }
}
