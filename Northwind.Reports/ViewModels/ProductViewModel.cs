using System;

namespace Northwind.Web.ViewModels {
    /// <summary>
    /// Product ViewModel (Data Transfer Object
    /// </summary>
    public class ProductViewModel {
        public int Id { get; set; }

        public string ShipCountry { get; set; }

        public string ShipCity { get; set; }

        public decimal? Freight { get; set; }

        public DateTime? RequiredDate { get; set; }

        public string ShipAddress { get; set; }

        public string ShipPostalCode { get; set; }

        public string CategoryName { get; set; }

        public DateTime? OrderDate { get; set; }

        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public short? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Summa { get; set; }

        public int? ProductId { get; set; }
    }
}