using DocumentRepository.Models;
using System.Data;

namespace DocumentRepository.Services
{
    public interface IDocumentService
    {
        Task<int> AddDocument(DocumentViewModel document);
        Task<DocumentViewModel>? GetDocumentDetailByDocumentID(Guid documentID);
        Task<int> UpdateDocumentDetail(DocumentViewModel documentvm);
        Task<DocumentViewModel> GetDocumentListAsPerPagenation(int pageIndex);
    }
}
