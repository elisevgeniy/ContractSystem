using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace ContractSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApprovalController : ControllerBase
    {
        ApprovalService approvalService;

        public ApprovalController(ApprovalService approvalService)
        {
            this.approvalService = approvalService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApprovalOut>> Get()
        {
            return approvalService.GetAll();
        }

        [HttpPost]
        public ActionResult<IEnumerable<ApprovalOut>> Approve(int approvalId)
        {
            try
            {
                approvalService.Approve(approvalId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
