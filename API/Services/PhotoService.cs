using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        
        private readonly Cloudinary _cloudinary;
        /* private readonly IOptions<CloudinarySettings> _configuration; */
        public  PhotoService(IOptions<CloudinarySettings>configuration)
        {

            var acc= new Account
            (
               configuration.Value.CloudName,
               configuration.Value.ApiKey,
                configuration.Value.ApiSecret
           );
                _cloudinary=new Cloudinary(acc);
        }


        public async Task<ImageUploadResult> AddPhotosAsync(IFormFile file)
        {
            var uploadResult =new ImageUploadResult();
            if(file.Length>0)
            {
                
                using var stream = file.OpenReadStream();
                var uploadParams= new ImageUploadParams
                {
                    File= new FileDescription(file.FileName,stream ),
                    Transformation = new Transformation().
                    Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult= await _cloudinary.UploadAsync(uploadParams);
            }
               return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotosAsync(string publicId)
        {

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
        
            return result;
        }
    }
}