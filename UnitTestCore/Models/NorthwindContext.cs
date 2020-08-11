using Microsoft.EntityFrameworkCore;

namespace UnitTestCore.Models {
    public partial class NorthwindContext : DbContext {
        public NorthwindContext() {
        }

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options) {
        }

        public virtual DbSet<AlphabeticalListOfProducts> AlphabeticalListOfProducts { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CurrentProductList> CurrentProductList { get; set; }
        public virtual DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCity { get; set; }
        public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
        public virtual DbSet<CustomerDemographics> CustomerDemographics { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<OrderDetailsExtended> OrderDetailsExtended { get; set; }
        public virtual DbSet<OrderSubtotals> OrderSubtotals { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersQry> OrdersQry { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrice { get; set; }
        public virtual DbSet<ProductsByCategory> ProductsByCategory { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarter { get; set; }
        public virtual DbSet<SummaryOfSalesByYear> SummaryOfSalesByYear { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Territories> Territories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("DataSource=Sample\\Db\\Northwind.sl3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<AlphabeticalListOfProducts>(entity => {
                entity.HasNoKey();

                entity.ToView("Alphabetical list of products");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int");

                entity.Property(e => e.CategoryName).HasColumnType("varchar(15)");

                entity.Property(e => e.Discontinued).HasColumnType("int");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasColumnType("int");

                entity.Property(e => e.ProductName).HasColumnType("varchar(40)");

                entity.Property(e => e.QuantityPerUnit).HasColumnType("varchar(20)");

                entity.Property(e => e.ReorderLevel).HasColumnType("int");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .HasColumnType("int");

                entity.Property(e => e.UnitPrice).HasColumnType("float(26)");

                entity.Property(e => e.UnitsInStock).HasColumnType("int");

                entity.Property(e => e.UnitsOnOrder).HasColumnType("int");
            });

            modelBuilder.Entity<Categories>(entity => {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnType("varchar(15)");
            });

            modelBuilder.Entity<CurrentProductList>(entity => {
                entity.HasNoKey();

                entity.ToView("Current Product List");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasColumnType("int");

                entity.Property(e => e.ProductName).HasColumnType("varchar(40)");
            });

            modelBuilder.Entity<CustomerAndSuppliersByCity>(entity => {
                entity.HasNoKey();

                entity.ToView("Customer and Suppliers by City");

                entity.Property(e => e.City).HasColumnType("varchar(15)");

                entity.Property(e => e.CompanyName).HasColumnType("varchar(40)");

                entity.Property(e => e.ContactName).HasColumnType("varchar(30)");
            });

            modelBuilder.Entity<CustomerCustomerDemo>(entity => {
                entity.HasKey(e => new { e.CustomerId, e.CustomerTypeId });

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.CustomerTypeId)
                    .HasColumnName("CustomerTypeID")
                    .HasColumnType("varchar(10)");
            });

            modelBuilder.Entity<CustomerDemographics>(entity => {
                entity.HasKey(e => e.CustomerTypeId);

                entity.Property(e => e.CustomerTypeId)
                    .HasColumnName("CustomerTypeID")
                    .HasColumnType("varchar(10)");
            });

            modelBuilder.Entity<Customers>(entity => {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Address).HasColumnType("varchar(60)");

                entity.Property(e => e.City).HasColumnType("varchar(15)");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.ContactName).HasColumnType("varchar(30)");

                entity.Property(e => e.ContactTitle).HasColumnType("varchar(30)");

                entity.Property(e => e.Country).HasColumnType("varchar(15)");

                entity.Property(e => e.Fax).HasColumnType("varchar(24)");

                entity.Property(e => e.Phone).HasColumnType("varchar(24)");

                entity.Property(e => e.PostalCode).HasColumnType("varchar(10)");

                entity.Property(e => e.Region).HasColumnType("varchar(15)");
            });

            modelBuilder.Entity<EmployeeTerritories>(entity => {
                entity.HasKey(e => new { e.EmployeeId, e.TerritoryId });

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasColumnType("int");

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("TerritoryID")
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<Employees>(entity => {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnType("varchar(60)");

                entity.Property(e => e.BirthDate).HasColumnType("timestamp");

                entity.Property(e => e.City).HasColumnType("varchar(15)");

                entity.Property(e => e.Country).HasColumnType("varchar(15)");

                entity.Property(e => e.Extension).HasColumnType("varchar(4)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.HireDate).HasColumnType("timestamp");

                entity.Property(e => e.HomePhone).HasColumnType("varchar(24)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.PhotoPath).HasColumnType("varchar(255)");

                entity.Property(e => e.PostalCode).HasColumnType("varchar(10)");

                entity.Property(e => e.Region).HasColumnType("varchar(15)");

                entity.Property(e => e.ReportsTo).HasColumnType("int");

                entity.Property(e => e.Title).HasColumnType("varchar(30)");

                entity.Property(e => e.TitleOfCourtesy).HasColumnType("varchar(25)");
            });

            modelBuilder.Entity<OrderDetails>(entity => {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.ToTable("Order Details");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasColumnType("int");

                entity.Property(e => e.Discount).HasColumnType("float(13)");

                entity.Property(e => e.Quantity).HasColumnType("int");

                entity.Property(e => e.UnitPrice).HasColumnType("float(26)");
            });

            modelBuilder.Entity<OrderDetailsExtended>(entity => {
                entity.HasNoKey();

                entity.ToView("Order Details Extended");

                entity.Property(e => e.Discount).HasColumnType("float(13)");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasColumnType("int");

                entity.Property(e => e.ProductName).HasColumnType("varchar(40)");

                entity.Property(e => e.Quantity).HasColumnType("int");

                entity.Property(e => e.UnitPrice).HasColumnType("float(26)");
            });

            modelBuilder.Entity<OrderSubtotals>(entity => {
                entity.HasNoKey();

                entity.ToView("Order Subtotals");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int");
            });

            modelBuilder.Entity<Orders>(entity => {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasColumnType("int");

                entity.Property(e => e.Freight).HasColumnType("float(26)");

                entity.Property(e => e.OrderDate).HasColumnType("timestamp");

                entity.Property(e => e.RequiredDate).HasColumnType("timestamp");

                entity.Property(e => e.ShipAddress).HasColumnType("varchar(60)");

                entity.Property(e => e.ShipCity).HasColumnType("varchar(15)");

                entity.Property(e => e.ShipCountry).HasColumnType("varchar(15)");

                entity.Property(e => e.ShipName).HasColumnType("varchar(40)");

                entity.Property(e => e.ShipPostalCode).HasColumnType("varchar(10)");

                entity.Property(e => e.ShipRegion).HasColumnType("varchar(15)");

                entity.Property(e => e.ShipVia).HasColumnType("int");

                entity.Property(e => e.ShippedDate).HasColumnType("timestamp");
            });

            modelBuilder.Entity<OrdersQry>(entity => {
                entity.HasNoKey();

                entity.ToView("Orders Qry");

                entity.Property(e => e.Address).HasColumnType("varchar(60)");

                entity.Property(e => e.City).HasColumnType("varchar(15)");

                entity.Property(e => e.CompanyName).HasColumnType("varchar(40)");

                entity.Property(e => e.Country).HasColumnType("varchar(15)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .HasColumnType("int");

                entity.Property(e => e.Freight).HasColumnType("float(26)");

                entity.Property(e => e.OrderDate).HasColumnType("timestamp");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int");

                entity.Property(e => e.PostalCode).HasColumnType("varchar(10)");

                entity.Property(e => e.Region).HasColumnType("varchar(15)");

                entity.Property(e => e.RequiredDate).HasColumnType("timestamp");

                entity.Property(e => e.ShipAddress).HasColumnType("varchar(60)");

                entity.Property(e => e.ShipCity).HasColumnType("varchar(15)");

                entity.Property(e => e.ShipCountry).HasColumnType("varchar(15)");

                entity.Property(e => e.ShipName).HasColumnType("varchar(40)");

                entity.Property(e => e.ShipPostalCode).HasColumnType("varchar(10)");

                entity.Property(e => e.ShipRegion).HasColumnType("varchar(15)");

                entity.Property(e => e.ShipVia).HasColumnType("int");

                entity.Property(e => e.ShippedDate).HasColumnType("timestamp");
            });

            modelBuilder.Entity<Products>(entity => {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int");

                entity.Property(e => e.Discontinued).HasColumnType("int");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.QuantityPerUnit).HasColumnType("varchar(20)");

                entity.Property(e => e.ReorderLevel).HasColumnType("int");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .HasColumnType("int");

                entity.Property(e => e.UnitPrice).HasColumnType("float(26)");

                entity.Property(e => e.UnitsInStock).HasColumnType("int");

                entity.Property(e => e.UnitsOnOrder).HasColumnType("int");
            });

            modelBuilder.Entity<ProductsAboveAveragePrice>(entity => {
                entity.HasNoKey();

                entity.ToView("Products Above Average Price");

                entity.Property(e => e.ProductName).HasColumnType("varchar(40)");

                entity.Property(e => e.UnitPrice).HasColumnType("float(26)");
            });

            modelBuilder.Entity<ProductsByCategory>(entity => {
                entity.HasNoKey();

                entity.ToView("Products by Category");

                entity.Property(e => e.CategoryName).HasColumnType("varchar(15)");

                entity.Property(e => e.Discontinued).HasColumnType("int");

                entity.Property(e => e.ProductName).HasColumnType("varchar(40)");

                entity.Property(e => e.QuantityPerUnit).HasColumnType("varchar(20)");

                entity.Property(e => e.UnitsInStock).HasColumnType("int");
            });

            modelBuilder.Entity<Region>(entity => {
                entity.Property(e => e.RegionId)
                    .HasColumnName("RegionID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.RegionDescription)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Shippers>(entity => {
                entity.HasKey(e => e.ShipperId);

                entity.Property(e => e.ShipperId)
                    .HasColumnName("ShipperID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.Phone).HasColumnType("varchar(24)");
            });

            modelBuilder.Entity<SummaryOfSalesByQuarter>(entity => {
                entity.HasNoKey();

                entity.ToView("Summary of Sales by Quarter");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int");

                entity.Property(e => e.ShippedDate).HasColumnType("timestamp");
            });

            modelBuilder.Entity<SummaryOfSalesByYear>(entity => {
                entity.HasNoKey();

                entity.ToView("Summary of Sales by Year");

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("int");

                entity.Property(e => e.ShippedDate).HasColumnType("timestamp");
            });

            modelBuilder.Entity<Suppliers>(entity => {
                entity.HasKey(e => e.SupplierId);

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .HasColumnType("int")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnType("varchar(60)");

                entity.Property(e => e.City).HasColumnType("varchar(15)");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.ContactName).HasColumnType("varchar(30)");

                entity.Property(e => e.ContactTitle).HasColumnType("varchar(30)");

                entity.Property(e => e.Country).HasColumnType("varchar(15)");

                entity.Property(e => e.Fax).HasColumnType("varchar(24)");

                entity.Property(e => e.Phone).HasColumnType("varchar(24)");

                entity.Property(e => e.PostalCode).HasColumnType("varchar(10)");

                entity.Property(e => e.Region).HasColumnType("varchar(15)");
            });

            modelBuilder.Entity<Territories>(entity => {
                entity.HasKey(e => e.TerritoryId);

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("TerritoryID")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.RegionId)
                    .HasColumnName("RegionID")
                    .HasColumnType("int");

                entity.Property(e => e.TerritoryDescription)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
