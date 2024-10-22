using DocumentRepository.Models;
using DocumentRepository.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;

namespace DocumentRepository.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _service;
        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> AddDocument()
        {
            //throw new NotImplementedException();
             ViewBag.Issumbitted = TempData["IsSubmitted"] as int?;
            //ViewBag.Issumbitted = 1;
            DocumentViewModel documentViewModel = await _service.GetDocumentListAsPerPagenation(1); //1 = Get the document data for the first page of pegination 
            return View(documentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddDocument(DocumentViewModel document)
        {
            if (document.isUpdated == 1 && document.uploadedFile == null)
            {
                ModelState.Remove("uploadedFile");
            }
                if (ModelState.IsValid)
                {
                    TempData["IsSubmitted"] = await _service.AddDocument(document);
                    return RedirectToAction("AddDocument");
                }
                else
                {
                    document = await _service.GetDocumentListAsPerPagenation(1);
                    return View(document);
                }
                
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentDataForEdit(Guid documentID)
        {
            return Json(await _service.GetDocumentDetailByDocumentID(documentID));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentData([FromBody] DocumentViewModel documentViewModel)
        {
            int changesUpdated = await _service.UpdateDocumentDetail(documentViewModel);
            return Json(changesUpdated);
        }

        [HttpGet]
        public async Task<IActionResult> getDocumentDetailListPartial(int pageIndex)
        {
            var documentList = await _service.GetDocumentListAsPerPagenation(pageIndex);
            return PartialView("_DocumentDetailsListView", documentList.documentDetailList);
        }
    

    }
}
