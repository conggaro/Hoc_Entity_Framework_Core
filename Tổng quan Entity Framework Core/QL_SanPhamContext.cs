using System;
using Microsoft.EntityFrameworkCore;


/*
    trong quá trình viết code
    có thể xảy ra ngoại lệ

    "A connection was successfully established with the server,
    but then an error occurred during the login process.
    (provider: SSL Provider, error: 0 - The certificate
    chain was issued by an authority that is not trusted.)"

    => Lỗi này thường xảy ra khi kết nối với máy chủ SQL Server bị lỗi chứng chỉ.
    Bạn có thể thử một số cách sau để khắc phục lỗi này:

    1. Tắt mã hóa kết nối: Trong chuỗi kết nối của bạn, hãy thêm "Encrypt = false"
    để sử dụng kết nối không được mã hóa.

    2. Tin tưởng chứng chỉ tự ký: Trong chuỗi kết nối của bạn,
    hãy thêm "TrustServerCertificate = true" để chấp nhận chứng
    chỉ tự ký.
    Lưu ý rằng điều này không an toàn và chỉ nên được sử dụng
    trong môi trường phát triển hoặc kiểm tra.

    3. Cài đặt chứng chỉ từ một CA tin cậy: Để giải quyết vấn đề này một cách an toàn,
    bạn nên cung cấp một chứng chỉ hợp lệ từ một CA tin cậy cho SQL Server của bạn.

    => tôi đã sử dụng cách 2 và thành công

    cụ thể: tôi thêm "TrustServerCertificate = true"
    ở bên trong biến chuoi_ket_noi
*/


namespace MyApp
{
    public class QL_SanPhamContext : DbContext
    {
        // thuộc tính "SanPhams" có kiểu dữ liệu DbSet<SanPham>
        // cho biết cơ sở dữ liệu có bảng mà thông tin
        // về bảng "SanPham" được biểu diễn bởi lớp "SanPham"
        public DbSet<SanPham> SanPhams { get; set; }


        // chuỗi kết nối tới cơ sở dữ liệu
        // ở đây tôi đang sử dụng SQL Server
        private const string chuoi_ket_noi = @"
            Server = localhost;
            Database = QL_SanPham;
            User ID = sa;
            Password = 123456;
            TrustServerCertificate = true
        ";


        /*
            phương thức OnConfiguring()
            được gọi mỗi khi một đối tượng DbContext được tạo

            ghi đè nó để thiết lập các cấu hình
            ví dụ như thiết lập chuỗi kết nối
        */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // gọi phương thức OnConfiguring()
            base.OnConfiguring(optionsBuilder);

            // gọi phương thức UseSqlServer()
            optionsBuilder.UseSqlServer(chuoi_ket_noi);
        }
    }
}