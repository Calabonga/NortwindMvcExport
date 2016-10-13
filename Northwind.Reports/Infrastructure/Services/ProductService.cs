using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.PagedListLite;
using Calabonga.Portal.Config;
using Northwind.Data;
using Northwind.Models;
using Northwind.Web.Controllers;
using Northwind.Web.ViewModels;

namespace Northwind.Web {

    public interface IProductService {
        /// <summary>
        /// Get products list by pageIndex and pageSize
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IPagedList<ProductViewModel> Paged(Query queryParams, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get product list for export
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        Task<List<ProductViewModel>> GetDataForExportAsync(DateTime dateStart, DateTime dateEnd);
    }

    public class ProductService : IProductService {

        private readonly IContext _context;
        private readonly IConfigService<CurrentAppSettings> _configService;

        public ProductService(IContext context, IConfigService<CurrentAppSettings> configService) {
            _context = context;
            _configService = configService;
        }

        /// <summary>
        /// Get products list by pageIndex and pageSize
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IPagedList<ProductViewModel> Paged(Query queryParams, DateTime startDate, DateTime endDate) {
            if (queryParams == null || queryParams.Page == 0) {
                queryParams = new Query { Page = 1, Size = _configService.Config.DefaultPagerSize };
            }

            var orders = GetData(startDate, endDate);
            return orders.TakePage(queryParams.Page, queryParams.Size);

        }

        /// <summary>
        /// Get product list for export
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public Task<List<ProductViewModel>> GetDataForExportAsync(DateTime dateStart, DateTime dateEnd) {
            var token = new CancellationToken();
            var items = GetData(dateStart, dateEnd);
            return items.ToListAsync(token);
        }

        private IQueryable<ProductViewModel> GetData(DateTime startDate, DateTime endDate) {
            IQueryable<ProductViewModel> orders;

            orders = from o in _context.Order
                     join d in _context.OrderDetail on o.ID equals d.OrderID
                     where o.OrderDate >= startDate && o.OrderDate <= endDate
                     orderby o.OrderDate
                     select new ProductViewModel {
                         Id = o.ID,
                         ShipCountry = o.ShipCountry,
                         ShipCity = o.ShipCity,
                         Freight = o.Freight,
                         ProductId = d.ProductID,
                         RequiredDate = o.RequiredDate,
                         ShipAddress = o.ShipAddress,
                         ShipPostalCode = o.ShipPostalCode,
                         CategoryName = d.Product.Category.Name,
                         OrderDate = o.OrderDate,
                         ProductName = d.Product.Name,
                         QuantityPerUnit = d.Product.QuantityPerUnit,
                         Quantity = d.Quantity,
                         UnitPrice = d.UnitPrice,
                         Summa = d.Quantity * d.UnitPrice
                     };

            return orders;
        }
    }
}
