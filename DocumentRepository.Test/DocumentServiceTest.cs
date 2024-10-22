using DocumentRepository.Data;
using DocumentRepository.Models;
using DocumentRepository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepository.Test
{
    public class DocumentServiceTest
    {
        private readonly DocumentService _documentService;
        private readonly ApplicationDbContext _context;

        public DocumentServiceTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _documentService = new DocumentService(_context);
        }


        #region AddDocument
        [Fact]
        public async Task AddDocument_NulldocumentParameter_ReturnArgumentNullException()
        {
            DocumentViewModel documentViewModel = null;

           await  Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
               await  _documentService.AddDocument(documentViewModel);
            });
        }

        [Fact]
        public async Task AddDocument_NullFormFields_ReturnArgumentException()
        {
            DocumentViewModel documentViewModel = new DocumentViewModel() { documentName = "", documentCode = ""};

           await  Assert.ThrowsAsync<ArgumentException>(async () =>
            {
               await _documentService.AddDocument(documentViewModel);
            });
        }

        #endregion

        #region UpdateDocumentDetails

        [Fact]
        public async Task UpdateDocumentDetail_NulldocumentObject_ReturnArgumentNullException()
        {
            DocumentViewModel documentViewModel = null;

           await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
               await _documentService.AddDocument(documentViewModel);
            });
        }

        [Fact]
        public async Task UpdateDocumentDetail_NulldocumentID_ReturnArgumentException()
        {
            DocumentViewModel documentViewModel = new DocumentViewModel() { documentID = Guid.NewGuid() };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _documentService.AddDocument(documentViewModel);
            });
        }

        [Fact]
        public async Task UpdateDocumentDetail_WrongDocumentIDUpdateFailed_ReturnZero()
        {
            Guid documentID = Guid.Empty;
            var model = new DocumentViewModel
            {
                documentID = new Guid("85838588-1FF8-4B64-5A17-08DCF0CC3D1F"),  //This guid is not present so it will not be found
                documentName = "UpdatedDoc",
                documentCode = "Code456",
                uploadedFile = null // No new file
            };
            var result = await _documentService.UpdateDocumentDetail(model);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task UpdateDocumentDetail_ValidDocumentDetails_ReturnInt()
        {
            DocumentModel document = new DocumentModel()
            {
                documentID = new Guid("85838588-1FF8-4B64-5A17-08DCF0CC3D1F"),
                documentName = "MarkSheet",
                documentCode = "PDF",
                uploadedFileDetails = "TestFile",
                documentExtension = ".pdf",
                documentSize = 100,
                uploadedBy = "TestUser",
                uploaded_DateTime = DateTime.Now
            };
            _context.documentModels.Add(document);
           await  _context.SaveChangesAsync();

            var model = new DocumentViewModel
            {
                documentID = new Guid("85838588-1FF8-4B64-5A17-08DCF0CC3D1F"),
                documentName = "UpdatedDoc",
                documentCode = "Code456",
                uploadedFile = null // No new file
            };


            var result =await _documentService.UpdateDocumentDetail(model);

            // Assert
            Assert.Equal(1, result);
            var updatedDocument = await _context.documentModels.FindAsync(model.documentID);
            Assert.Equal("UpdatedDoc", updatedDocument.documentName);
            Assert.Equal("Code456", updatedDocument.documentCode);
            Assert.Equal("TestUser", updatedDocument.updatedBy);
        }

        [Fact]
        public async Task UpdateDocumentDetail_DocumentNotFound_ReturnsZero()  //We are providing documentID which is not present and then check for update
        {
            // Arrange
            var model = new DocumentViewModel
            {
                documentID = Guid.NewGuid(),
                documentName = "NonExistentDoc",
                documentCode = "Code000",
                uploadedFile = null
            };

            // Act
            var result = await _documentService.UpdateDocumentDetail(model);

            // Assert
            Assert.Equal(0, result);
        }

        #endregion

        #region DocumentDetailByDocumetID

        [Fact]
        public async Task DocumentDetailByDocumetID_NullDocumentID_ReturnNull()
        {
            Guid documentID = Guid.Empty;
           var result = await _documentService.GetDocumentDetailByDocumentID(documentID);
           Assert.Null(result);
        }
        
        [Fact]
        public async Task DocumentDetailByDocumetID_WrongDocumentID_ReturnNull()
        {
            Guid documentID = Guid.Empty;
           var result = await _documentService.GetDocumentDetailByDocumentID(documentID);
           Assert.Null(result);
        }




        #endregion

    }
}
