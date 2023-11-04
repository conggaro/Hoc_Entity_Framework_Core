using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MyApp
{
    // tạo lớp "Cửa Hàng Context"
    public class CuaHangContext : DbContext
    {
        // dòng code TrustServerCertificate = true
        // quan trọng đấy
        // nó liên quan đến chính sách bảo mật kết nối cơ sở dữ liệu
        // bạn viết thiếu
        // thì có thể
        // không kết nối được vào cơ sở dữ liệu


        // khai báo chuỗi kết nối
        private const string chuoi_ket_noi = @"
            Server = localhost;
            Database = QL_SanPham;
            User ID = sa;
            Password = 123456;
            TrustServerCertificate = true
        ";


        // bảng cha (bảng dữ liệu chính)
        public DbSet<TheLoai> theLoais { set; get; }            // bảng "Thể Loại"
        
        
        // bảng con (bảng phụ thuộc)
        public DbSet<SanPham> sanPhams { set; get; }            // bảng "Sản phẩm"


        // ghi đè phương thức
        // để cấu hình
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // vì kế thừa lớp DbContext
            // nên có thể gọi phương thức OnConfiguring() của lớp cha
            // bằng từ khóa "base"
            base.OnConfiguring(optionsBuilder);


            // tạo ILoggerFactory
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());


            // thiết lập làm việc với SqlServer
            optionsBuilder.UseSqlServer(chuoi_ket_noi);


            // thiết lập logging
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }


        #region Tạo cơ sở dữ liệu
        public async Task CreateDatabase()
        {
            String databasename = Database.GetDbConnection().Database;

            Console.WriteLine("Tao " + databasename);
            
            bool result = await Database.EnsureCreatedAsync();
            
            string resultstring = result ? "tao thanh cong" : "da co truoc do";
            
            Console.WriteLine($"CSDL {databasename} : {resultstring}");
        }
        #endregion


        #region Xóa cơ sở dữ liệu
        public async Task DeleteDatabase()
        {
            String databasename = Database.GetDbConnection().Database;
            
            Console.Write($"Co chac chan xoa {databasename} (y) ? ");
            
            string input = Console.ReadLine();

            // hỏi lại cho chắc
            if (input.ToLower() == "y")
            {
                bool deleted = await Database.EnsureDeletedAsync();
                string deletionInfo = deleted ? "da xoa" : "khong xoa duoc";
                Console.WriteLine($"{databasename} {deletionInfo}");
            }
        }
        #endregion


        #region Chèn dữ liệu mẫu vào cơ sở dữ liệu
        public async Task InsertSampleData()
        {
            // Thêm 2 bản ghi vào bảng "Thể Loại"
            var the_loai1 = new TheLoai() { Id = 1, TenTheLoai = "May tinh", MoTa = "San pham duoc ban tai Viet Nam" };
            var the_loai2 = new TheLoai() { Id = 2, TenTheLoai = "Dien Thoai", MoTa = "San pham duoc ban tai Nhat Ban" };


            // gọi phương thức AddRangeAsync()
            // để thêm 2 bản ghi vào ngữ cảnh Context
            await AddRangeAsync(the_loai1, the_loai2);


            // gọi phương thức SaveChangesAsync()
            // để lưu dữ liệu vào cơ sở dữ liệu
            await SaveChangesAsync();


            // thêm 5 bản ghi vào bảng "Sản Phẩm"
            await AddRangeAsync(
                new SanPham() { Id = 1, TenSanPham = "SP 1", GiaSanPham = 200, FK_TheLoai = the_loai1 },
                new SanPham() { Id = 2, TenSanPham = "SP 2", GiaSanPham = 500, FK_TheLoai = the_loai1 },
                new SanPham() { Id = 3, TenSanPham = "SP 3", GiaSanPham = 100, FK_TheLoai = the_loai2 },
                new SanPham() { Id = 4, TenSanPham = "SP 4", GiaSanPham = 88, FK_TheLoai = the_loai1 },
                new SanPham() { Id = 5, TenSanPham = "SP 5", GiaSanPham = 128, FK_TheLoai = the_loai2 }
            );


            // gọi phương thức SaveChangesAsync()
            // để lưu dữ liệu vào cơ sở dữ liệu
            await SaveChangesAsync();


            // in ra các sản phẩm
            // sử dụng vòng lặp "foreach"
            foreach (var item in sanPhams)
            {
                // tạo đối tượng stringBuilder
                StringBuilder stringBuilder = new StringBuilder();
                
                stringBuilder.Append($"ID: {item.Id}, ");
                
                stringBuilder.Append($"Ten san pham: {item.TenSanPham}, ");
                
                stringBuilder.Append($"The loai: ({item.TheLoaiId} - {item.FK_TheLoai.TenTheLoai})");
                
                Console.WriteLine(stringBuilder);
            }


            // KẾT QUẢ IN RA MÀN HÌNH KIỂU:
            // ID: 1, Ten san pham: SP 1, The loai: (1 - May tinh)
            // ID: 2, Ten san pham: SP 2, The loai: (1 - May tinh)
            // ID: 3, Ten san pham: SP 3, The loai: (2 - Dien Thoai)
            // ID: 4, Ten san pham: SP 4, The loai: (1 - May tinh)
            // ID: 5, Ten san pham: SP 5, The loai: (2 - Dien Thoai)
        }
        #endregion


        #region Lấy 1 bản ghi từ bảng "Sản phẩm" theo Id
        public async Task<SanPham> FindProduct(int id)
        {
            // khai báo đối tượng truy vấn
            var query = await (
                                from item in sanPhams
                                where item.Id == id
                                select item
                              )
                              .FirstOrDefaultAsync();
            
            return query;
        }
        #endregion
    }
}
