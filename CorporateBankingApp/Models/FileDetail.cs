using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateBankingApp.Models
{
    public class FileDetail
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public DateTime DateUploaded { get; set; }

    }
}
