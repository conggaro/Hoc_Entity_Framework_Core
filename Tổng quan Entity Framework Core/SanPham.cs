using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;


/*
    trước khi sử dụng model SanPham
    
    hãy bổ sung các thiết lập
    thông qua Attribute

    1. thiết lập ánh xạ lớp thành bảng
    => sử dụng thuộc tính Table
    ví dụ: [Table("SanPham")]

    2. thiết lập khóa chính
    => sử dụng thuộc tính Key
    ví dụ: [Key]

    3. thiết lập không cho null
    => sử dụng thuộc tính Required
    ví dụ: [Required]

    4. thiết lập độ dài ký tự
    của giá trị
    bên trong cái thuộc tính đấy
    => sử dụng thuộc tính StringLength, MinLength, MaxLength
    ví dụ:  [StringLength(50)]
            [MinLength(1)]
            [MaxLength(500)]

    5. thiết lập tự động tăng
    => sử dụng khai báo sau:
    
*/


namespace MyApp
{
    // tên bảng là "sản phẩm"
    [Table("SanPham")]
    public class SanPham
    {
        // trường Id là khóa chính
        // tự động tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        // mã sản phẩm thì không được null
        // và độ dài mã sản phẩm là 50
        [Required]
        [StringLength(50)]
        public string MaSanPham { get; set; }


        // độ dài tên sản phẩm là 50
        // viết dấu hỏi chấm là cho phép null
        [StringLength(50)]
        public string? TenSanPham { get; set; }
    }
}