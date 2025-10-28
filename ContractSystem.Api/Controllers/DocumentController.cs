using ContractSystem.Core.Exceptions;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public ActionResult<DocumentOut> Update(DocumentUpdateIn document)
        {
            try
            {
                return Ok(documentService.Update(document));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int documentId)
        {
            try
            {
                documentService.Delete(documentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
