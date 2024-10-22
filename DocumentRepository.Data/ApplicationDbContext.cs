using DocumentRepository.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<DocumentModel> documentModels { get; set; }
        public DbSet<ErrorModel> errorLogDetails { get; set; }

        public async Task<List<DocumentDetailListViewModel>> usp_getPagenatedDocumentList(int pageIndex, int pazeSize)
        {
            List<DocumentDetailListViewModel> documentVmList = new List<DocumentDetailListViewModel>(); 
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pazeSize)
            };
            List<DocumentModel> documentList = await documentModels.FromSqlRaw("EXEC [dbo].[usp_getPagenatedDocumentList] @PageIndex, @PageSize", parameters).ToListAsync();

            foreach(var document in documentList)
            {
                DocumentDetailListViewModel documentvm = new DocumentDetailListViewModel();
                documentvm.documentName = document.documentName;
                documentvm.documentCode = document.documentCode;
                documentvm.documentID = document.documentID;
                documentvm.fileAddress = GetDocumentPathByDocumentID(document.documentID);
                documentVmList.Add(documentvm);
            }
            return documentVmList;
        }


        public string GetDocumentPathByDocumentID(Guid documentID)
        {
            string? fileName =  documentModels.Where(doc => doc.documentID == documentID)
                 .Select(x => $"{x.uploadedFileDetails}{x.documentExtension}").FirstOrDefault();

            string? FilePath = "/uploads/" + fileName;
            return FilePath;
        }


         
    }


}
