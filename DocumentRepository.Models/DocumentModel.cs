using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepository.Models
{
    public class DocumentModel
    {
        [Key]
        public Guid documentID { get; set; }
        public string? documentName { get; set; }
        public string? documentCode { get; set; }
        public string? uploadedFileDetails { get; set; }
        public int? documentSize { get; set; }
        public string? documentExtension { get; set; }
        public string? uploadedBy { get; set; }
        public DateTime? uploaded_DateTime { get; set; }
        public string? updatedBy { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
