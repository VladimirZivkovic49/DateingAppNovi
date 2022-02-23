using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
     
        Task<ImageUploadResult>AddPhotosAsync(IFormFile file);
        Task<DeletionResult>DeletePhotosAsync(string publicId);
        
   
    }  
  }