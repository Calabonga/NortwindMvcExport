using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using Calabonga.Portal.Config;
using Northwind.Data;
using Northwind.Web.ViewModels;

namespace Northwind.Web.Controllers {
    public class HomeController : Controller {

        private readonly IContext _context;
        private readonly IConfigService<CurrentAppSettings> _configService;
        private readonly IProductService _productService;

        public HomeController(IContext context, IConfigService<CurrentAppSettings> configService, IProductService productService) {
            _context = context;
            _configService = configService;
            _productService = productService;
        }

        public ActionResult Index(Query query, DateTime? dateStart, DateTime? dateEnd) {
            if (query == null || query.Page == 0) {
                query = new Query { Page = 1, Size = _configService.Config.DefaultPagerSize };
            }
            var model = new ProductSelectorViewModel {
                Size = query.Size
            };
            if (dateStart.HasValue && dateEnd.HasValue) {
                model.DateStart = dateStart.Value;
                model.DateEnd = dateEnd.Value;
            }
            model.Products = _productService.Paged(query, model.DateStart, model.DateEnd);
            return View(model);
        }

        public async Task<ExcelResult> Export(DateTime dateStart, DateTime dateEnd) {
           
            var fileName = $"{dateStart.ToString(CultureInfo.InvariantCulture).Replace(".", "_")}-{dateEnd.ToString(CultureInfo.InvariantCulture).Replace(".", "_")}.xls";
            var items = await _productService.GetDataForExportAsync(dateStart, dateEnd);
            var title = $"Отчет по товарам с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}";
            var html = ExportBuilder.Build(items, title);
            return new ExcelResult(fileName, html);
        }
    }
}
