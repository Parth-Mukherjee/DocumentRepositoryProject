using DocumentRepository.Data;
using DocumentRepository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;


namespace DocumentRepository.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;
        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddDocument(DocumentViewModel document)
        {
            if (document == null)
            {
                throw new ArgumentNullException();
            }
            if (string.IsNullOrEmpty(document.documentName) || string.IsNullOrEmpty(document.documentCode) )
            {
                throw new ArgumentException();
            }

            if (document.isUpdated == 0)  //Fresh Submit Case
            {
                //For fresh Document details entered in DB
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Path.GetFileName(document.uploadedFile.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.uploadedFile.CopyToAsync(stream);
                }
                //for fresh Document SUBMIT
                var documentModel = ToDocumentModel(document);
                 await _context.documentModels.AddAsync(documentModel);
                return await _context.SaveChangesAsync();
            }
            else   //Update Existing Document detail
            {
                if (document.uploadedFile != null)   //If file also sent then replace the old file with new one
                {
                    
                    var oldFileName = _context.documentModels.Where(doc => doc.documentID == document.documentID)
                    .Select(x => $"{x.uploadedFileDetails}{x.documentExtension}").FirstOrDefault();

                    var fileUpdatePath = Path.Combine("wwwroot", "uploads", oldFileName);
                    if (System.IO.File.Exists(fileUpdatePath))
                    {
                        System.IO.File.Delete(fileUpdatePath);
                    }

                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploads);
                    var fileName = Path.GetFileName(document.uploadedFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.uploadedFile.CopyTo(stream);
                    }
                }
                //For UPDATING document details 

              return await  UpdateDocumentDetail(document);
            }

        }

        public DocumentModel ToDocumentModel(DocumentViewModel document)
        {
            DocumentModel documentModel = new DocumentModel()
            {
                documentName = document.documentName,
                documentCode = document.documentCode,
                documentExtension = Path.GetExtension(document.uploadedFile.FileName).ToLower(),
                documentSize = Convert.ToInt32((document.uploadedFile.Length) / 1024 * 1024),
                uploadedFileDetails = Path.GetFileNameWithoutExtension(document.uploadedFile.FileName),
                uploadedBy = "TestUser",
                uploaded_DateTime = DateTime.Now
            };


            return  documentModel;
        }

        public async  Task<DocumentViewModel>? GetDocumentDetailByDocumentID(Guid documentID)
        {
            if(documentID == Guid.Empty )
            {
                return null;
            }
            DocumentViewModel? documentDetail = await _context.documentModels.Where(x => x.documentID == documentID)
                                                                             .Select(x => new DocumentViewModel 
                                                                             { documentName = x.documentName, 
                                                                              documentCode = x.documentCode 
                                                                              })
                                                                             .FirstOrDefaultAsync();
            //if (documentDetail == null)
            //{
            //    return null;
            //}
            return documentDetail;
        }

        public async Task<int> UpdateDocumentDetail(DocumentViewModel documentVm)  //Update the existing document details
        {

            if (documentVm == null)
            {
                throw new ArgumentNullException();
            }
            if (documentVm.documentID == Guid.Empty)
            {
                throw new ArgumentException();
            }


            //used in AddDocument method above
            var document = await _context.documentModels.FindAsync(documentVm.documentID);
            int changesSaved = 0;
            if (document != null)
            {
                document.documentName = documentVm.documentName;
                document.documentCode = documentVm.documentCode;
                document.updatedBy = "TestUser";
                document.updatedOn = DateTime.Now;
                if (documentVm.uploadedFile != null)
                {
                    document.uploadedFileDetails = Path.GetFileNameWithoutExtension(documentVm.uploadedFile.FileName);
                    document.documentExtension = Path.GetExtension(documentVm.uploadedFile.FileName).ToLower();
                    document.documentSize = Convert.ToInt32((documentVm.uploadedFile.Length) / 1024 * 1024);
                }
                changesSaved = await _context.SaveChangesAsync();
                return  changesSaved;  //Document Updated then savechange = 1
            }
            return changesSaved;
        }


        public async Task<DocumentViewModel> GetDocumentListAsPerPagenation(int pageIndex)   //As per the page index click from pagenation
        {                                                          //it will get the next 5 document details. 
            int maxRows = 5;
            DocumentViewModel documentvm = new DocumentViewModel();
            documentvm.documentDetailList = await _context.usp_getPagenatedDocumentList(pageIndex, maxRows);


            double result = (double) await _context.documentModels.CountAsync() / maxRows;
            int incrementedPageIndex = (int)Math.Ceiling(result);
            documentvm.pageCount = incrementedPageIndex;
            documentvm.currentIndex = pageIndex;
            return documentvm;
        }
    }


}
