using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRepository.Models
{
    public class FileValidationAttribute : ValidationAttribute
    {
        private readonly string[] _SupportingfileExtension;
        private readonly long _fileSize;
        private readonly int _isUpdated;

        public FileValidationAttribute(string[] fileExtension, long fileSize , int isUpdated)
        {
            _SupportingfileExtension = fileExtension;
            _fileSize = fileSize;
            _isUpdated = isUpdated; //isUpdate = 0 means case for fresh submit of document ,
                                     //isUpdate = 1 means Update case for document ,
                                     
        }
        protected override ValidationResult IsValid(object? value , ValidationContext validationContext)
        {

            var contextProperty = validationContext.ObjectType.GetProperty(_isUpdated.ToString());

            //var contextValue = (bool)contextProperty.GetValue(validationContext.ObjectInstance);

            var file = value as IFormFile;
            if (_isUpdated == 0)  //For Submit Document Case
            {
                if (file == null)
                {
                    return new ValidationResult("Kindly select a PDF or Excel file to proceed");
                }

                if (file.Length > _fileSize)
                {
                    return new ValidationResult($"File size exceeds the {_fileSize / (1024 * 1024)} MB Limit");
                }

                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (string.IsNullOrEmpty(fileExtension) || !_SupportingfileExtension.Contains(fileExtension))
                {
                    return new ValidationResult("Invalid file type. Only Excel and PDF files are allowed");
                }

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] fileBytes = stream.ToArray();
                    string content = System.Text.Encoding.UTF8.GetString(fileBytes);
                    if (content.Length == 0)
                    {
                        return new ValidationResult("This file is empty, Kindly try with different file");
                    }
                }

            }
            else //For Update Document Case
            {
                if (file != null)
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (string.IsNullOrEmpty(fileExtension) || !_SupportingfileExtension.Contains(fileExtension))
                    {
                        return new ValidationResult("Invalid file type. Only Excel and PDF files are allowed");
                    }

                    if (file.Length > _fileSize)
                    {
                        return new ValidationResult($"File size exceeds the {_fileSize / (1024 * 1024)} MB Limit");
                    }

                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        byte[] fileBytes = stream.ToArray();
                        string content = System.Text.Encoding.UTF8.GetString(fileBytes);
                        if (content.Length == 0)
                        {
                            return new ValidationResult("This file is empty, Kindly try with different file");
                        }
                    }

                }
            }



            
            return ValidationResult.Success;
        }
    }
}
