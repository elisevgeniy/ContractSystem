using ContractSystem.Core;
using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private DataContext _dataContext;

        public DocumentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<DocumentDTO> GetAll()
        {
            return _dataContext.Documents.DefaultIfEmpty().ToList();
        }

        public DocumentDTO? GetById(int id)
        {
            return _dataContext.Documents.Find(id);
        }

        public DocumentDTO Add(DocumentDTO documentDTO)
        {
            _dataContext.Documents.Add(documentDTO);
            return documentDTO;
        }

        public DocumentDTO Update(DocumentDTO documentDTO)
        {
            _dataContext.Documents.Update(documentDTO);
            return documentDTO;
        }

        public void Delete(DocumentDTO documentDTO)
        {
            _dataContext.Documents.Remove(documentDTO);
        }
    }
}
