using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepository.Models
{
    public class DocumentViewModel
    {

        public DocumentViewModel()
        {
            documentDetailList = new List<DocumentDetailListViewModel>();
        }

        [Key]
        public Guid documentID { get; set; }


        [DisplayName("Document Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Document Name should be witin 50 characters")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphanumeric values are allowed for Document'name")]
        public string? documentName { get; set; }


        [DisplayName("Document Code")]
        [Required]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only characters are acceptable")]
        [MaxLength(3, ErrorMessage = "Document Name should be witin 3 characters")]
        public string? documentCode { get; set; }


        [DisplayName("Upload File")]
        [FileValidation(new string[] {".xlsx",".xls",".pdf"}, 5*1024*1024, 0)]
        public IFormFile? uploadedFile { get; set; }

       // public string? FileAddress { get; set; }
        public List<DocumentDetailListViewModel> documentDetailList { get; set; }

        public string? updatedBy { get; set; }
        public DateTime? updatedOn { get; set; }
        public int? isUpdated { get; set; }
        public int? currentIndex { get; set; }
        public int? pageCount { get; set; }
//        public string? fileAddress { get; set; }


    }
}
