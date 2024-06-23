using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Attachments
{
    public interface IAttachmentAppService
    {
        Task<int> UploadAttachmentAsync(IFormFile file);
    }
}
