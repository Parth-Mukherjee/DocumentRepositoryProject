using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepository.Models
{
    public class ErrorModel
    {
        [Key]
        public Guid logID { get; set; }
        public string? errorMessage { get; set; }
        public DateTime errorLogOn { get; set; }
        public string? errorStackTrace { get; set; }
        public string? errorRoute { get; set; }
    }
}
