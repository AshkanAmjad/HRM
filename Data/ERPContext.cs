using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Convertors;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data
{
    public class ERPContext : DbContext
    {

        public ERPContext(DbContextOptions<ERPContext> options) : base(options)
        {

        }

        //ایجاد جدول تمامی پرسنل  سه سطح در پایگاه داده
        public DbSet<Employees> Employees { get; set; }
        //ایجاد جدول  پرسنل سطح استان در پایگاه داده
        public DbSet<ProvinceLevel> ProvinceLevel { get; set; }
        //ایجاد جدول  پرسنل سطح شهرستان در پایگاه داده
        public DbSet<CountyLevel> CountyLevels { get; set; }
        //ایجاد جدول  پرسنل سطح بخش در پایگاه داده
        public DbSet<DistrictLevel> DistrictLevels { get; set; }
        //ایجاد جدول عنوان و شناسه وضعیت تاهل در پایگاه داده
        public DbSet<Marital> Maritals { get; set; }
        //ایجاد جدول عنوان و شناسه وضعیت استخدام در پایگاه داده
        public DbSet<Employment> Employments { get; set; }
        //ایجاد جدول عنوان و شناسه شعبه در پایگاه داده
        public DbSet<Department> Departments { get; set; }
        //ایجاد جدول  عنوان و شناسه نقش پرسنل در پایگاه داده
        public DbSet<Role> Roles { get; set; }
        //ایجاد جدول  عنوان و شناسه وضعیت جنسیت در پایگاه داده
        public DbSet<Gender> Genders { get; set; }
        //ایجاد جدول  عنوان و شناسه وضعیت شعبه های شهرستان در پایگاه داده
        public DbSet<County> Counties { get; set; }
        //ایجاد جدول  بارگذاری مدارک سطح استان در پایگاه داده
        public DbSet<DocumentUploadProvinceLevel> documentUploadProvinceLevels { get; set; }
        //ایجاد جدول  بارگذاری مدارک سطح شهرستان در پایگاه داده
        public DbSet<DocumentUploadCountyLevel> documentUploadCountyLevels { get; set; }
        //ایجاد جدول  بارگذاری مدارک سطح بخش در پایگاه داده
        public DbSet<DocumentUploadDistrictLevel> documentUploadDistrictLevels { get; set; }
        //ایجاد جدول  بارگذاری مدارک بین سطوح در پایگاه داده
        public DbSet<TransferDocumentsBetweenLevels> transferDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed Data ProcinceLevel
            //داده اولیه به جدول سطح استان
            modelBuilder.Entity<ProvinceLevel>().HasData(new Models.ProvinceLevel()
            {
                userId = 1,
                nationalCode = "0021047022",
                userPass = "123",
                department = 1,
                role = "مدیریت",
                fName = "اشکان",
                lName = "مطهری",
                gender = "مرد",
                education = "کارشناسی",
                dateOfBirth = "1377/02/20",
                tel = 09351225600,
                address = "کرج",
                maritalStatus = "مجرد",
                employmentStatus = "رسمی",
                workingStatus = "فعال",
                RegisterDate = DateTime.Now,

            }
            ); ; ;

            //داده اولیه به جدول تمامی پرسنل
            modelBuilder.Entity<Employees>().HasData(new Models.Employees()
            {
                userId = 1,
                nationalCode = "0021047022",
                userPass = "123",
                area = "استان",
                department = 1,
                role = "مدیریت",
                fName = "اشکان",
                lName = "مطهری",
                education = "کارشناسی",
                dateOfBirth = "1377/02/20",
                gender = "مرد",
                tel = 09351225600,
                address = "کرج",
                maritalStatus = "مجرد",
                employmentStatus = "رسمی",
                workingStatus = "فعال",
                RegisterDate = DateTime.Now,
                country = 0
            }
            );

            //داده اولیه به جدول نقش ها
            modelBuilder.Entity<Role>().HasData(new Role()
            { RoleId = 1, RoleTitle = "مدیریت" },
            new Role()
            { RoleId = 2, RoleTitle = "معاونت فناوری اطلاعات" },
            new Role()
            { RoleId = 3, RoleTitle = "معاونت سیاسی" },
            new Role()
            { RoleId = 4, RoleTitle = "معاونت فرهنگی" },
            new Role()
            { RoleId = 5, RoleTitle = "معاونت اجتماعی" },
            new Role()
            { RoleId = 6, RoleTitle = "معاونت مالی" },
            new Role()
            { RoleId = 7, RoleTitle = "معاونت امور فرا سطح" },
            new Role()
            { RoleId = 8, RoleTitle = "معاونت توسعه و پشتیبانی" },
            new Role()
            { RoleId = 9, RoleTitle = "معاونت منابع انسانی" },
            new Role()
            { RoleId = 10, RoleTitle = "معاونت حراست" }
            );

            //داده اولیه به جدول جنسیت
            modelBuilder.Entity<Gender>().HasData(new Gender()
            { genderId = 1, genderTitle = "مرد" },
            new Gender()
            { genderId = 2, genderTitle = "زن" }
            );

            //داده اولیه به جدول وضعیت تاهل
            modelBuilder.Entity<Marital>().HasData(new Marital()
            { maritalId = 1, maritalTitle = "مجرد" },
            new Marital()
            { maritalId = 2, maritalTitle = "متاهل" }
            );

            //داده اولیه به جدول وضعیت استخدام
            modelBuilder.Entity<Employment>().HasData(new Employment()
            { EmploymentId = 1, EmploymentTitle = "رسمی" },
            new Employment()
            { EmploymentId = 2, EmploymentTitle = "پیمانی" },
            new Employment()
            { EmploymentId = 3, EmploymentTitle = "قراردادی" },
            new Employment()
            { EmploymentId = 4, EmploymentTitle = "آزمایشی" }
            );

            //داده اولیه به جدول شعبه ها
            modelBuilder.Entity<Department>().HasData(new Department()
            { departmentId = 1, departmentTitle = "شعبه1" },
            new Department()
            { departmentId = 2, departmentTitle = "شعبه2" },
            new Department()
            { departmentId = 3, departmentTitle = "شعبه3" }
            );

            //داده اولیه به جدول شعبه های شهرستان
            modelBuilder.Entity<County>().HasData(new County()
            { countyId = 1, countyTitle = "شعبه1" },
            new County()
            { countyId = 2, countyTitle = "شعبه2" },
            new County()
            { countyId = 3, countyTitle = "شعبه3" }
            );
            #endregion

            #region filter-IsDelete
            //ایجاد فیلتر برای حذف کاربر
            modelBuilder.Entity<Employees>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<ProvinceLevel>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<CountyLevel>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<DistrictLevel>()
                .HasQueryFilter(u => !u.IsDelete);
            //ایجاد فیلتر برای حذف مدارک کاربر
            modelBuilder.Entity<DocumentUploadProvinceLevel>()
                .HasQueryFilter(d => !d.IsDelete);
            modelBuilder.Entity<DocumentUploadCountyLevel>()
                .HasQueryFilter(d => !d.IsDelete);
            modelBuilder.Entity<DocumentUploadDistrictLevel>()
                .HasQueryFilter(d => !d.IsDelete);
            //ایجاد فیلتر حذف برای نقش ها
            modelBuilder.Entity<Role>()
                .HasQueryFilter(r => !r.IsDelete);
            //ایجاد فیلتر برای تبادلات حذف شده
            modelBuilder.Entity<TransferDocumentsBetweenLevels>()
                .HasQueryFilter(d => !d.IsDelete);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
