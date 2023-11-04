using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;
using System.Text;


/*
    ENTITY FRAMEWORK CORE

    Tạo Model trong Entity Framework
    dùng để ánh xạ các đối tượng vào cơ sở dữ liệu

    Khai báo các Model,
    sử dụng kỹ thuật Data Annotation
    để ánh xạ các đối tượng
    vào các thành phần của cơ sở dữ liệu
    với Entity Framework Core,
    
    thuộc tính [Column],
    thuộc tính [ForeignKey],
    thuộc tính [InverseProperty]
*/


/*
    BẠN SẼ PHẢI THÊM CÁC PACKAGE SAU:
    (bạn vào Manage NuGet Package của Visual Studio để thêm nhé)

    1. System.Data.SqlClient
    2. Microsoft.EntityFrameworkCore
    3. Microsoft.EntityFrameworkCore.SqlServer
    4. Microsoft.EntityFrameworkCore.Design
    5. Microsoft.Extensions.DependencyInjection
    6. Microsoft.Extensions.Logging
    7. Microsoft.Extensions.Logging.Console


    cái package thứ 6 và 7
    dùng để in câu lệnh SQL ra màn hình console
*/


namespace MyApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // tạo lớp "Sản Phẩm"
            // tiếng Anh là Product


            // tạo lớp "Thể loại"
            // tiếng Anh là Category


            /*
                Những khái niệm về tạo mối liên hệ trong EF Core

                Một mối liên hệ là ràng buộc (constraint hoặc binding)
                giữa hai đối tượng (entity)

                Như trong các hệ quản trị CSDL quan hệ,
                nó thể hiện bởi các khóa ngoại - foreign key (FK)

                
                
                Để tạo ra các mối quan hệ trong EF Core,
                trước tiên hiểu một số khái niệm sau:

                1. Bảng dữ liệu phụ thuộc
                hay bảng con (Dependent entity)
                là những bảng có chứa khóa ngoại (FK) tham chiếu đến bảng khác

                2. Bảng dữ liệu chính
                hay bảng cha (Principal entity)
                là bảng có chứa khóa chính

                3. Khóa chính - Primary Key (PK)
                là thuộc tính,
                chứa giá trị duy nhất để xác định dòng dữ liệu

                4. Khóa ngoại - Foreign Key (FK)
                là thuộc tính trong bảng con
                được sử dụng để lưu khóa chính của bảng cha

                
                
                Thuộc tính điều hướng (Navigation)
                thuộc tính này chứa tham chiếu đến một đối tượng
                từ bảng khác, có các loại như:
                
                1. Điều hướng tập hợp(Collection navigation)
                tham chiếu đến một tập hợp các đối tượng bảng khác (quan hệ một nhiều)

                2. Điều hướng tham chiếu (Reference navigation)
                tham chiếu đến một đối tượng khác. (quan hệ một một)

                3. Điều hướng nghịch (Inverse navigation)
                thuộc tính điều hướng tham chiếu
                đến một điều hướng khác,
                sử dụng để tạo FK
                tham chiếu đến đối tượng cùng kiểu
            */


            /*
                MỘT SỐ KIỂU ĐƯỢC HỖ TRỢ BỞI SQL SERVER NHƯ:

                1. bigint
                2. numeric
                3. bit
                4. smallint
                5. decimal
                6. smallmoney
                7. int
                8. tinyint
                9. money
                10. date
                11. datetimeoffset
                12. datetime2
                13. smalldatetime
                14. datetime
                15. time
                16. char
                17. varchar
                18. text
                19. nchar
                20. nvarchar
                21. ntext
                22. binary
                23. varbinary
                24. image
            */


            // tạo đối tượng
            CuaHangContext context = new CuaHangContext();


            // gọi phương thức DeleteDatabase()
            // để xóa database
            // nếu tồn tại QL_SanPham
            await context.DeleteDatabase();


            // gọi phương thức CreateDatabase()
            // để tạo lại cơ sở dữ liệu: QL_SanPham
            await context.CreateDatabase();


            // gọi phương thức InsertSampleData()
            // để thêm mới 2 bản ghi vào bảng "Thể Loại"
            // và thêm mới 5 bản ghi vào bảng "Sản Phẩm"
            await context.InsertSampleData();


            // gọi phương thức DisposeAsync()
            // để hủy kết nối, giải phóng
            await context.DisposeAsync();


            // gán lại vùng nhớ mới
            // chính là dòng code "new CuaHangContext()"
            context = new CuaHangContext();


            // gọi phương thức FindProduct()
            // để lấy bản ghi có Id = 2
            // từ bảng "Sản Phẩm"
            var ban_ghi = await context.FindProduct(2);
            
            
            // lấy đối tượng khóa ngoại
            var dt_FK = ban_ghi.FK_TheLoai;


            // nếu bản ghi khác null
            if (ban_ghi != null)
            {
                Console.WriteLine($"{ban_ghi.TenSanPham} co TheLoaiId = {ban_ghi.TheLoaiId}");


                // nếu đối tượng "dt_FK"
                // khác null
                // nếu true thì giá trị là "dt_FK.TenTheLoai"
                // nếu false thì giá trị là "Bản ghi đang null"
                string ten_TheLoai = (dt_FK != null) ? dt_FK.TenTheLoai : "Doi tuong FK_TheLoai đang null";


                // khi bạn query lên
                // thì có thể đối tượng "FK_TheLoai" sẽ bị null đó
                // còn mấy cái khác thì có dữ liệu bình thường
                Console.WriteLine(ten_TheLoai);
            }
        }
    }
}