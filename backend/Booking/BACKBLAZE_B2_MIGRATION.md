# Backblaze B2 Migration Guide

## Overview

This project has been migrated from **AWS S3 + CloudFront** to **Backblaze B2** with its S3-compatible API. This provides:

- ✅ **Cost Savings**: Backblaze B2 is significantly cheaper than AWS S3
- ✅ **Simpler Setup**: No need for complex CloudFront signed URLs
- ✅ **Public URLs**: Native support for public, cacheable URLs
- ✅ **S3 Compatibility**: Works with existing AWS SDK libraries
- ✅ **Built-in CDN**: Cloudflare CDN integration included

---

## Configuration Changes

### Old AWS Configuration (Removed)

```json
{
  "AWS": {
    "Region": "us-east-1",
    "S3": {
      "BucketName": "your-bucket"
    },
    "CloudFront": {
      "Url": "https://d123456.cloudfront.net",
      "KeyPairId": "APKAXXXXXXXX",
      "PrivateKey": "-----BEGIN RSA PRIVATE KEY-----\n..."
    }
  }
}
```

### New Backblaze B2 Configuration

Update your `appsettings.json` or environment variables:

```json
{
  "Storage": {
    "BucketName": "your-bucket-name",
    "PublicUrlBase": "https://f004.backblazeb2.com/file/your-bucket-name"
  },
  "AWS": {
    "Profile": "backblaze",
    "Region": "us-west-004"
  }
}
```

**Or using custom domain (recommended for production):**

```json
{
  "Storage": {
    "BucketName": "your-bucket-name",
    "PublicUrlBase": "https://cdn.yourdomain.com"
  },
  "AWS": {
    "Profile": "backblaze",
    "Region": "us-west-004"
  }
}
```

---

## Setup Instructions

### 1. Create Backblaze B2 Account

1. Sign up at [backblaze.com](https://www.backblaze.com/b2/sign-up.html)
2. Navigate to **B2 Cloud Storage** in the dashboard

### 2. Create a Bucket

1. Go to **Buckets** → **Create a Bucket**
2. Choose a unique bucket name (e.g., `my-app-images`)
3. **Important**: Set **Files in Bucket are** to **Public** for public URLs
4. Select a region (e.g., `us-west-004`)
5. Enable **Object Lock** if needed (optional)

### 3. Create Application Key

1. Go to **App Keys** → **Add a New Application Key**
2. Name: `your-app-name`
3. Allow access to: Select your bucket
4. Permissions: Read and Write
5. **Save the credentials** (shown only once):
   - `keyID` → This is your Access Key
   - `applicationKey` → This is your Secret Key

### 4. Configure AWS SDK to Use Backblaze B2

#### Option A: Using AWS Profile (Recommended)

Create or update `~/.aws/credentials`:

```ini
[backblaze]
aws_access_key_id = YOUR_BACKBLAZE_KEY_ID
aws_secret_access_key = YOUR_BACKBLAZE_APPLICATION_KEY
```

Create or update `~/.aws/config`:

```ini
[profile backblaze]
region = us-west-004
s3 =
    endpoint_url = https://s3.us-west-004.backblazeb2.com
```

#### Option B: Using Environment Variables

```bash
export AWS_ACCESS_KEY_ID="YOUR_BACKBLAZE_KEY_ID"
export AWS_SECRET_ACCESS_KEY="YOUR_BACKBLAZE_APPLICATION_KEY"
export AWS_REGION="us-west-004"
export AWS_ENDPOINT_URL="https://s3.us-west-004.backblazeb2.com"
```

#### Option C: Using appsettings.json

```json
{
  "AWS": {
    "Profile": "backblaze",
    "Region": "us-west-004",
    "ServiceURL": "https://s3.us-west-004.backblazeb2.com"
  }
}
```

### 5. Update Dependency Injection

In your `Program.cs` or `Startup.cs`, configure the S3 client:

```csharp
// Add AWS S3 client configured for Backblaze B2
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var s3Config = new AmazonS3Config
    {
        // Backblaze B2 S3-compatible endpoint
        ServiceURL = "https://s3.us-west-004.backblazeb2.com",

        // Use your Backblaze region
        AuthenticationRegion = "us-west-004",

        // Force path style for Backblaze compatibility
        ForcePathStyle = true,

        // Disable payload signing for better performance
        SignatureVersion = "4"
    };

    return new AmazonS3Client(
        config["AWS:AccessKeyId"],      // Your Backblaze keyID
        config["AWS:SecretAccessKey"],   // Your Backblaze applicationKey
        s3Config
    );
});

// Register the image processing service
builder.Services.AddScoped<S3ImageProcessingService>();
```

---

## Public URLs

### Backblaze B2 Public URL Format

When you set a bucket to **Public**, files are accessible via:

```
https://f{XXX}.backblazeb2.com/file/{bucket-name}/{file-path}
```

**Example:**

```
https://f004.backblazeb2.com/file/my-app-images/images/photo123_original.jpg
```

### Finding Your Public URL Base

1. Upload a test file to your public bucket
2. View the file details in B2 dashboard
3. Copy the **Friendly URL**
4. Extract the base part (everything before the file path)

**Example:**

- Full URL: `https://f004.backblazeb2.com/file/my-bucket/test.jpg`
- Base URL: `https://f004.backblazeb2.com/file/my-bucket`

### Custom Domain (Optional)

For production, use a custom domain:

1. Set up a CNAME record:

   ```
   cdn.yourdomain.com → f004.backblazeb2.com
   ```

2. Configure your bucket for custom domain in Backblaze dashboard

3. Update configuration:
   ```json
   {
     "Storage": {
       "PublicUrlBase": "https://cdn.yourdomain.com/file/your-bucket-name"
     }
   }
   ```

---

## Testing

### Test Image Upload

```csharp
[HttpPost("upload")]
public async Task<IActionResult> UploadImage(IFormFile file)
{
    var fileName = Guid.NewGuid().ToString();
    var result = await _imageService.ProcessImageAsync(file, fileName);

    // result.OriginalUrl will be a public URL like:
    // https://f004.backblazeb2.com/file/my-bucket/images/abc123_original.jpg

    return Ok(result);
}
```

### Verify Public Access

After uploading, test the URLs:

```bash
# Should return the image (no authentication needed for public buckets)
curl -I https://f004.backblazeb2.com/file/your-bucket/images/test_original.jpg
```

---

## Migration Checklist

- [ ] Create Backblaze B2 account
- [ ] Create public bucket
- [ ] Generate application key
- [ ] Update configuration files
- [ ] Configure AWS SDK for Backblaze endpoint
- [ ] Test image upload
- [ ] Verify public URLs work
- [ ] (Optional) Set up custom domain
- [ ] Migrate existing images from S3 to B2
- [ ] Update frontend to use new URLs
- [ ] Remove old AWS credentials
- [ ] Delete CloudFront distribution (if applicable)

---

## Cost Comparison

### Backblaze B2

- Storage: $0.005/GB/month (first 10GB free)
- Downloads: Free up to 3x storage, then $0.01/GB
- API calls: Free (first 2,500/day)
- No egress fees to Cloudflare CDN

### AWS S3 + CloudFront (for comparison)

- S3 Storage: $0.023/GB/month
- CloudFront: $0.085/GB for first 10TB
- API requests charged separately

**Estimated savings: 70-90% for typical workloads**

---

## Key Features Retained

✅ Image processing (resize, optimize, thumbnail generation)
✅ Multiple image variants (original + thumbnail)
✅ Cache-friendly headers (1-year max-age)
✅ JPEG optimization
✅ Secure deletion
✅ Metadata tracking

## Features Removed/Changed

❌ CloudFront signed URLs (not needed with public buckets)
❌ Server-side encryption configuration (B2 encrypts by default)
✅ **Simpler public URLs** (no complex signing)
✅ **Better performance** (fewer network hops)

---

## Troubleshooting

### "Access Denied" Errors

- Ensure bucket is set to **Public**
- Verify application key has **Read and Write** permissions
- Check that bucket name in config matches actual bucket

### "Invalid Endpoint" Errors

- Verify ServiceURL includes correct region: `s3.us-west-004.backblazeb2.com`
- Ensure `ForcePathStyle = true` in S3Config

### URLs Not Working

- Confirm `PublicUrlBase` in config is correct
- Test URL format: `{PublicUrlBase}/{key}`
- Verify files exist in bucket via B2 dashboard

### Performance Issues

- Consider enabling Backblaze CDN (Cloudflare integration)
- Use custom domain with CDN for better caching
- Ensure cache headers are set (already configured in code)

---

## Additional Resources

- [Backblaze B2 Documentation](https://www.backblaze.com/b2/docs/)
- [S3 Compatible API Reference](https://www.backblaze.com/b2/docs/s3_compatible_api.html)
- [AWS SDK for .NET with Backblaze](https://www.backblaze.com/b2/docs/integration_aws.html)

---

## Support

For issues or questions:

1. Check Backblaze B2 documentation
2. Verify configuration matches this guide
3. Test with AWS CLI: `aws s3 ls s3://your-bucket --endpoint-url=https://s3.us-west-004.backblazeb2.com`
