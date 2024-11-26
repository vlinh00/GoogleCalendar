using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AT.Share.Model
{
    public class ProgressModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string Name { get; set; }
        public int ProjectTypeId { get; set; } // Khóa ngoại đến ProjectTypeModel
    }
}
