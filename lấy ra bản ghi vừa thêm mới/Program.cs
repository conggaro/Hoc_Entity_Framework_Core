using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


// bạn phải cài thêm thư viện
// Microsoft.EntityFrameworkCore.SqlServer
// để sử dụng phương thức UseSqlServer()


namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new MyDbContext())
            {
                // Tạo một đối tượng mới
                var newEntity = new MyEntity
                {
                    Name = "New Entity"
                };

                // Thêm đối tượng mới vào DbSet
                context.MyEntities.Add(newEntity);

                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                // Bây giờ, newEntity.Id sẽ chứa giá trị khóa chính của bản ghi vừa được thêm vào
                Console.WriteLine("Entity added successfully with ID: " + newEntity.Id);
            }
        }
    }


    // thêm câu lệnh khai báo bảng cho lớp MyEntity
    [Table("MyEntity")]
    public class MyEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }


    public class MyDbContext : DbContext
    {
        private const string chuoi_ket_noi = @"
            Server = localhost;
            Database = Database_Demo;
            User ID = sa;
            Password = 123456;
            TrustServerCertificate = true
        ";

        public DbSet<MyEntity> MyEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(chuoi_ket_noi);
        }
    }
}