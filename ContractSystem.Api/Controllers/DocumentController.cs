using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.Service;
using Microsoft.AspNetCore.Mvc;
using ContractSystem.Core.Exceptions;

namespace ContractSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        DocumentService documentService;

        public DocumentController(DocumentService documentService)
        {
            this.documentService = documentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DocumentOut>> Get()
        {
            return documentService.GetAll();
        }

        [HttpGet("{id:int}")]
        public ActionResult<DocumentOut> GetById(int id)
        {
            try
            {
                return documentService.GetById(id);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<DocumentOut> Add(DocumentIn documentIn)
        {
            try
            {
                return documentService.AddDocument(documentIn);
            } catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
