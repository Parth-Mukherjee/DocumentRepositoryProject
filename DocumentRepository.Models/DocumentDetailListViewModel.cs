using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepository.Models
{
    public class DocumentDetailListViewModel
    {
        public Guid? documentID { get; set; }
        public string? documentName { get; set; }
        public string? documentCode { get; set; }
        public string? fileAddress { get; set; }

    }
}
