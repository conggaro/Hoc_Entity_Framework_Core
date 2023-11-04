using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp
{
    [Table("TheLoai")]                                          // Ánh xạ bảng TheLoai
    public class TheLoai
    {
        [Key]                                                   // Là Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.None)]       // Tắt tự động tăng trong SQL
        public int Id { set; get; }


        [StringLength(100)]                                     // nvarchar(100)
        public string TenTheLoai { set; get; }


        // khai báo dùng string?
        // để lúc chương trình biên dịch sẽ
        // tạo ra cột MoTa
        // cho NULL trong cơ sở dữ liệu
        [Column(TypeName = "ntext")]                            // Cột (trường) kiểu "ntext" trong SQL Server
        public string? MoTa { set; get; }
    }
}
