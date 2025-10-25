using ContractSystem.Core.DTO;
using ContractSystem.Core.Models.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.IRepositories
{
    public interface IDocumentRepository
    {
        public List<DocumentDTO> GetAll();

        public List<DocumentDTO> GetAllByUser(int userId);

        public DocumentDTO GetById(int id);

        public DocumentDTO Add(DocumentDTO documentDTO);

        public DocumentDTO Update(DocumentDTO documentDTO);

        public void Delete(DocumentDTO documentDTO);
    }
}
