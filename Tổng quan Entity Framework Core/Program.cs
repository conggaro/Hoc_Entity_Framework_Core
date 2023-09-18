using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;


/*
    giới thiệu về thư viện Entity Framework Core
    
    thư viện cung cấp khả năng ánh xạ đối tượng
    lập trình với các bảng trong "cơ sở dữ liệu"
    
    cơ bản dùng Entity Framework để tạo Database
    
    đọc, ghi, xóa dữ liệu

    sử dụng Linq để thực hiện các truy vấn
*/


/*
    Entity Framework Core là framework (thư viện khung)
    để ánh xạ đối tượng lớp (object class)
    vào cơ sở dữ liệu quan hệ

    nó cho phép ánh xạ vào các bảng trong cơ sở dữ liệu

    tạo cơ sở dữ liệu

    truy vấn với Linq

    tạo và cập nhật vào cơ sở dữ liệu (Database)
*/


/*
    để sử dụng Entity Framework Core
    hãy thêm các package cần thiết vào:
    
    1. System.Data.SqlClient
    2. Microsoft.EntityFrameworkCore
    3. Microsoft.EntityFrameworkCore.SqlServer
    4. Microsoft.EntityFrameworkCore.Design
    5. System.ComponentModel.DataAnnotations
*/


/*
    Model là các lớp
    
    các lớp này ánh xạ với các bảng
    trong cơ sở dữ liệu

    quy tắc ánh xạ:
    1. tên bảng thì ánh xạ thành tên lớp
    2. tên trường trong bảng thì ánh xạ thành thuộc tính trong lớp
*/


/*
    Phương thức EnsureDeletedAsync, EnsureCreatedAsync dùng kỹ thuật async,
    nếu muốn dùng phiên bản đồng bộ thì là EnsureDeleted, EnsureCreated
*/


namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
                THÔNG TIN CƠ SỞ DỮ LIỆU

                1. tên cơ sở dữ liệu: QL_SanPham
                2. tên bảng: SanPham
            */


            /*
                TẠO DB CONTEXT
                
                DbContext trong Entity Framework
                là ngữ cảnh làm việc
                
                nó biểu diễn, chứa các thông tin cần thiết
                của một phiên làm việc với cơ sở dữ liệu

                để tạo ra mối liên hệ bảng SanPham
                trong cơ sở dữ liệu
                và model SanPham
                thì phải tạo ra "context" như sau:
                1. tạo lớp kế thừa DbContext
                2. đặt tên là "QL_SanPhamContext"
                3. tạo thuộc tính có kiểu DbSet<SanPham>
                4. ghi đè phương thức OnConfiguring()

                lớp "QL_SanPhamContext" mang ý nghĩa
                như là một cơ sở dữ liệu
                
                thuộc tính có kiểu DbSet<SanPham> chính là bảng
                trong cơ sở dữ liệu

                trong đó, cần ghi đè phương thức OnConfiguring()
                để cấu hình chuỗi kết nối
            */


            // từ lớp kế thừa DbContext là QL_SanPhamContext
            // có thể tương tác với cơ sở dữ liệu
            // bằng cách gọi các phương thức thích hợp
            // trong DbContext


            /*
                TẠO VÀ XÓA CƠ SỞ DỮ LIỆU

                DbContext có thuộc tính "Database"

                thuộc tính "Database" là đối tượng có kiểu DatabaseFacade

                => từ đó có thể tạo (hoặc xóa) cơ sở dữ liệu
            */


            /*
                THÊM BẢN GHI VÀO BẢNG

                các đối tượng DbContext hoặc đối tượng DbSet
                có phương thức Add() để bạn thêm đối tượng phù hợp
                vào DbContext

                nó nhận tham số là đối tượng Model cần thêm vào

                sau đó gọi phương thức SaveChange()
                để thực hiện cập nhật dữ liệu vào trong SQL Server

                
                ví dụ đối tượng DbContext: dt_DbContext
                ví dụ đối tượng DbSet: SanPhams


                ngoài ra nó còn có AddAsync(), SaveChangesAsync()
                để dành cho những người muốn code bất đồng bộ
                code nhiều tác vụ, nhiều luồng xử lý
            */


            /*
                ĐỌC DỮ LIỆU TỪ BẢNG
                TRUY VẤN VỚI LINQ

                các đối tượng DbSet có phương thức ToList()
                và ToArray()
                
                => để chuyển các bản ghi thành List (hoặc Array)
            */


            // gọi hàm tạo cơ sở dữ liệu
            Tao_CoSoDuLieu();

            
            // gọi hàm thêm mới 1 bản ghi
            Console.WriteLine();
            ThemMoi_1_BanGhi();


            // gọi hàm thêm mới nhiều bản ghi
            Console.WriteLine();
            ThemMoi_Nhieu_BanGhi();


            // gọi hàm lấy tất cả bản ghi
            Console.WriteLine();
            Lay_TatCa_BanGhi();


            // gọi hàm truy vấn bản ghi
            Console.WriteLine();
            TruyVan_BanGhi("SP099");

            
            // gọi hàm cập nhật bản ghi
            Console.WriteLine();
            CapNhat_BanGhi(3, "May loc nuoc");


            // gọi hàm xóa bản ghi
            Console.WriteLine();
            Xoa_BanGhi(4);


            // gọi hàm xóa cơ sở dữ liệu
            Console.WriteLine();
            Xoa_CoSoDuLieu();
        }


        #region TẠO CƠ SỞ DỮ LIỆU
            // tạo cơ sở dữ liệu QL_SanPham
            // tên "QL_SanPhamContext" từ chuỗi kết nối
            // được viết trong lớp QL_SanPhamContext

            // gồm tất cả các bảng
            // được định nghĩa bởi các
            // thuộc tính có kiểu DbSet

            public static void Tao_CoSoDuLieu()
            {
                // sử dụng using
                // tức là lớp QL_SanPhamContext
                // có kế thừa interface IDisposable

                // nếu không kế thừa IDisposable
                // thì viết code như bên dưới sẽ bị báo lỗi ngay

                // using có tác dụng quản lý vùng nhớ
                // trong máy tính
                // và tự động xóa vùng nhớ khi không dùng đến

                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // tạo biến tên cơ sở dữ liệu
                    string ten_CSDL = dt_DbContext.Database.GetDbConnection().Database;


                    // in ra tên cơ sở dữ liệu
                    // sử dụng dấu đô la $
                    // giúp viết code kiểu nội suy
                    // ngôn ngữ JavaScript cũng viết code nội suy bằng dấu $
                    Console.WriteLine($"Tao {ten_CSDL}");


                    // phương thức EnsureCreated()
                    // là phương thức của lớp DatabaseFacade
                    // có tác dụng tạo ra cơ sở dữ liệu
                    // nếu Database đang không tồn tại
                    // nếu Database đang tồn tại thì không làm gì cả


                    // phương thức EnsureCreated()
                    // trả về kiểu bool
                    bool ketQua = dt_DbContext.Database.EnsureCreated();


                    // tạo biến thông báo
                    // cách viết code bên dưới
                    // là câu lệnh điều kiện toán tử 3 ngôi
                    string thong_bao = ketQua ? "Tao thanh cong" : "Da ton tai truoc do";


                    // in thông báo ra màn hình
                    Console.WriteLine($"Co so du lieu {ten_CSDL}: {thong_bao}");
                }
            }
        #endregion


        #region XÓA CƠ SỞ DỮ LIỆU
            public static void Xoa_CoSoDuLieu()
            {
                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // tạo biến tên cơ sở dữ liệu
                    String ten_CSDL = dt_DbContext.Database.GetDbConnection().Database;
                    

                    // in ra màn hình câu hỏi
                    // có chắc chắn xóa cơ sở dữ liệu không
                    // nếu đồng ý thì bấm chữ "Y"
                    Console.Write($"Co chac chan xoa {ten_CSDL} (Y)? ");
                    

                    // tạo biến input
                    // để hứng dữ liệu nhập từ bàn phím
                    string input = Console.ReadLine();


                    // xử lý bằng câu lệnh điều kiện if()
                    // khi người dùng bấm phím "Y"
                    if (input.ToLower() == "y")
                    {
                        // phương thức EnsureDeleted()
                        // có tác dụng xóa cơ sở dữ liệu


                        // tạo biến kết quả
                        bool ketQua = dt_DbContext.Database.EnsureDeleted();
                        

                        // tạo biến thông báo
                        string thong_bao = ketQua ? "Da xoa" : "Khong xoa duoc";
                        

                        Console.WriteLine($"{ten_CSDL}: {thong_bao}");
                    }
                }
            }
        #endregion


        #region THÊM MỚI 1 BẢN GHI
            public static void ThemMoi_1_BanGhi()
            {
                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // thêm sản phẩm
                    dt_DbContext.SanPhams.Add(new SanPham
                        {
                            MaSanPham = "SP001",
                            TenSanPham = "Laptop"
                        }
                    );


                    // tạo biến đếm
                    // thực hiện cập nhật thay đổi trong DbContext lên Server
                    // phương thức SaveChanges()
                    // sẽ trả về số lượng bản ghi
                    // mà bạn đã thêm vào cơ sở dữ liệu
                    int dem = dt_DbContext.SaveChanges();
                    

                    // in ra màn hình
                    Console.WriteLine($"Da luu {dem} san pham");
                }
            }
        #endregion


        #region THÊM MỚI NHIỀU BẢN GHI
            public static void ThemMoi_Nhieu_BanGhi()
            {
                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // tạo đối tượng sp1, sp2, sp3, sp4
                    SanPham sp1 = new SanPham() { MaSanPham = "SP012", TenSanPham = "Dien thoai"};
                    SanPham sp2 = new SanPham() { MaSanPham = "SP054", TenSanPham = "May lanh"};
                    SanPham sp3 = new SanPham() { MaSanPham = "SP099", TenSanPham = "May giat"};
                    SanPham sp4 = new SanPham() { MaSanPham = "SP033", TenSanPham = "Tu lanh" };


                    // thêm nhiều sản phẩm
                    dt_DbContext.AddRange(new object[] {sp1, sp2, sp3, sp4});


                    // tạo biến đếm
                    // thực hiện cập nhật thay đổi trong DbContext lên Server
                    // phương thức SaveChanges()
                    // sẽ trả về số lượng bản ghi
                    // mà bạn đã thêm vào cơ sở dữ liệu
                    int dem = dt_DbContext.SaveChanges();


                    // in ra màn hình
                    Console.WriteLine($"Da luu {dem} san pham");
                }
            }
        #endregion


        #region LẤY TẤT CẢ BẢN GHI
            public static void Lay_TatCa_BanGhi()
            {
                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // tạo biến danh sách
                    // để lấy tất cả sản phẩm trong bảng
                    var danh_sach = dt_DbContext.SanPhams.ToList();


                    // in ra màn hình
                    Console.WriteLine("Tat ca san pham:");


                // dùng cách này
                // để lấy được danh sách tên cột
                var danh_sach_TenCot = dt_DbContext.Model.FindEntityType(typeof(SanPham))
                    .GetProperties()
                    .Select(p => p.GetColumnName())
                    .ToList();


                // in ra tên cột bằng vòng lặp foreach
                foreach (string item in danh_sach_TenCot)
                {
                    Console.Write($"{item, -15}");
                }


                Console.WriteLine();
                foreach (SanPham item in danh_sach)
                    {
                        Console.Write($"{item.Id, -15}");
                        Console.Write($"{item.MaSanPham,-15}");
                        Console.Write($"{item.TenSanPham, -15}\n");
                    }
                }
            }
        #endregion


        #region TRUY VẤN BẢN GHI
            public static void TruyVan_BanGhi(string thamSo)
            {
                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // dùng LINQ để truy vấn đến DbSet SanPhams (bảng SanPham)
                    // lấy các sản phẩm có mã sản phẩm là SP099
                    List<SanPham> truy_van = (from item in dt_DbContext.SanPhams
                                    where (item.MaSanPham == thamSo)
                                    select item
                                   )
                                   .ToList();


                    // in ra thông báo
                    Console.WriteLine($"Cac san pham co ma la \"{thamSo}\"");
                    

                    // dùng cách này
                    // để lấy được danh sách tên cột
                    var danh_sach_TenCot = dt_DbContext.Model.FindEntityType(typeof(SanPham))
                        .GetProperties()
                        .Select(p => p.GetColumnName())
                        .ToList();


                    // in ra tên cột bằng vòng lặp foreach
                    foreach (string item in danh_sach_TenCot)
                    {
                        Console.Write($"{item, -15}");
                    }
                    Console.WriteLine();


                    // in ra các bản ghi truy vấn được
                    foreach (SanPham item in truy_van)
                    {
                        Console.Write($"{item.Id, -15}");
                        Console.Write($"{item.MaSanPham, -15}");
                        Console.Write($"{item.TenSanPham, -15}\n");
                    }
                }
            }
        #endregion


        #region CẬP NHẬT BẢN GHI
            public static void CapNhat_BanGhi(int thamSo_ID, string thamSo_Ten)
            {
                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // tạo biến bản ghi
                    // để truy vấn bản ghi theo ID
                    var ban_ghi = (
                                    from item in dt_DbContext.SanPhams
                                    where (item.Id == thamSo_ID)
                                    select item
                                  ).FirstOrDefault();


                    // nếu tìm được bản ghi
                    // thì giá trị sẽ khác null
                    if (ban_ghi != null)
                    {
                        // đổi tên sản phẩm
                        ban_ghi.TenSanPham = thamSo_Ten;
                        

                        // in ra thông báo
                        Console.WriteLine($"San pham co Id = {ban_ghi.Id}");
                        Console.WriteLine($"=> Co ten moi = \"{ban_ghi.TenSanPham}\"");
                        

                        // gọi phương thức SaveChanges()
                        // để cập nhật cơ sở dữ liệu
                        dt_DbContext.SaveChanges();
                    }
                }
            }
        #endregion


        #region XÓA BẢN GHI
            public static void Xoa_BanGhi(int thamSo_ID)
            {
                using (var dt_DbContext = new QL_SanPhamContext())
                {
                    // tạo biến bản ghi
                    // để truy vấn bản ghi theo ID
                    var ban_ghi = (
                                    from item in dt_DbContext.SanPhams
                                    where (item.Id == thamSo_ID)
                                    select item
                                  ).FirstOrDefault();


                    // nếu tìm được bản ghi
                    // thì giá trị sẽ khác null
                    if (ban_ghi != null)
                    {
                        // gọi phương thức Remove()
                        // để xóa bản ghi
                        dt_DbContext.Remove(ban_ghi);


                        // gọi phương thức SaveChanges()
                        // để cập nhật cơ sở dữ liệu
                        dt_DbContext.SaveChanges();


                        // in ra thông báo
                        Console.WriteLine($"Da xoa ban ghi co Id = {thamSo_ID}");
                    }
                }
            }
        #endregion
    }
}