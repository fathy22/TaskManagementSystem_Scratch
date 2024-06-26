using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Attachments;
using Abp.UI;

namespace TaskManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentAppService _attachmentAppService;

        public AttachmentController(IAttachmentAppService attachmentAppService)
        {
            _attachmentAppService = attachmentAppService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAttachment(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                int attachmentId = await _attachmentAppService.UploadAttachmentAsync(file);
                return Ok(new { Id = attachmentId, Message = "File uploaded successfully." });
            }
            catch (UserFriendlyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
