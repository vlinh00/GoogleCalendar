using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AT.Share.Model
{
    public class ProjectTypeModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string Description { get; set; }
    }
}
