﻿using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Application.CustomLogs;
using Application.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Attachments;
using TaskManagementSystem.Core.Sessions;
using TaskManagementSystem.CustomLogs;
using TaskManagementSystem.Tasks;

namespace Application.Attachments
{
    public class AttachmentAppService : ApplicationService , IAttachmentAppService
    {
        private readonly Application.UnitOfWorks.IUnitOfWork _unitOfWork;
        private readonly ICustomLogAppService _customLogAppService;
        private readonly ICustomSession _customSession;
        public AttachmentAppService(ICustomLogAppService customLogAppService, Application.UnitOfWorks.IUnitOfWork unitOfWork, ICustomSession customSession)
        {
            _unitOfWork = unitOfWork;
            _customLogAppService = customLogAppService;
            _customSession = customSession;
        }

        public async Task<int> UploadAttachmentAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new UserFriendlyException("File is empty.");
                }
                var postedFileExtension = Path.GetExtension(file.FileName);
                if (postedFileExtension == ".exe" || postedFileExtension == ".dll" || postedFileExtension == ".ps1" || postedFileExtension == ".js")
                {
                    throw new Exception("not allowed files");
                }
                var filePath = Path.Combine("wwwroot", "attachments", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var attachment = new Attachment
                {
                    StoredFileName = file.FileName,
                    ContentType = file.ContentType,
                    Description = file.FileName,
                    Name = file.FileName,
                    Length = file.Length,
                    Uri= filePath,
                    Extension = postedFileExtension
                };
                if (_customSession.UserId != null)
                {
                    var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                    await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                    {
                        Description = $"{user.FirstName} {user.SecondName} Uploaded New Attachment : {attachment.Name}"
                    });
                }
                await _unitOfWork.GetRepository<Attachment>().Add(attachment);
                _unitOfWork.Save();

                return attachment.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        
        }
    }
}
