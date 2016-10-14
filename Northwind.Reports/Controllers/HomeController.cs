using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Calabonga.Portal.Config;
using Northwind.Data;
using Northwind.Web.Infrastructure;
using Northwind.Web.ViewModels;

namespace Northwind.Web.Controllers {
    public partial class HomeController : Controller {


        private readonly IConfigService<CurrentAppSettings> _configService;
        private readonly IProductService _productService;
        private readonly IEmailService _emailService;

        public HomeController(IContext context, IConfigService<CurrentAppSettings> configService, IProductService productService, IEmailService emailService) {

            _configService = configService;
            _productService = productService;
            _emailService = emailService;
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
            var fileName = "products_export.xls";
            var items = await _productService.GetDataForExportAsync(dateStart, dateEnd);
            var title = $"Отчет по товарам с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}";
            var html = ExportBuilder.Build(items, title);
            var emailTo = _configService.Config.ExportEmail;
            var emailFrom = _configService.Config.RobotEmail;

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream)) {

                writer.Write(html);
                writer.Flush();
                stream.Position = 0;
                using (var fileImage = new MemoryFile(stream, "application/vnd.ms-excel", fileName)) {
                    _emailService.SendMail(emailFrom, emailTo, title, title, new List<HttpPostedFileBase> { fileImage });
                    return new ExcelResult(fileName, html);
                }
            }
        }
    }
}

