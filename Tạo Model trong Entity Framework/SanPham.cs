using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp
{
    [Table("SanPham")]                                          // Ánh xạ bảng SanPham
    public class SanPham
    {
        [Key]                                                   // Là Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.None)]       // Tắt tự động tăng trong SQL
        public int Id { set; get; }


        [Required]                                              // Not Null
        [StringLength(50)]                                      // nvarchar(50)
        public string TenSanPham { set; get; }


        [Required]                                              // Not Null
        [Column(TypeName = "int")]                              // cột kiểu "money" trong SQL Server (tương ứng decimal trong Model C#)
        public int GiaSanPham { set; get; }


        // khóa ngoại (outer key): TheLoaiId
        // được khai báo nullable
        // là int?
        // dấu hỏi chấm cho phép biến kiểu int
        // có thể chứa giá trị null
        // biến int mặc định không lưu được giá trị "null" nhé
        public int? TheLoaiId { set; get; }


        // nếu muốn thiết lập mỗi một sản phẩm
        // thì tương ứng
        // có một "key" trỏ đến ID của bảng "Thể Loại"

        // rất đơn giản
        // hãy thêm một thuộc tính kiểu TheLoai
        // vào trong lớp SanPham


        // Sinh khóa ngoại Foreign Key (TheLoaiId ~ FK_TheLoai.Id)
        // ràng buộc đến khóa chính PK của bảng TheLoai
        [ForeignKey("TheLoaiId")]
        public TheLoai? FK_TheLoai { set; get; }
    }
}
