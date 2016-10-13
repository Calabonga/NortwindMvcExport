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
using Northwind.Web.ViewModels;

namespace Northwind.Web.Controllers {
    public class HomeController : Controller {


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
            var fileName = $"{dateStart.ToString(CultureInfo.InvariantCulture).Replace(".", "_")}-{dateEnd.ToString(CultureInfo.InvariantCulture).Replace(".", "_")}.xls";
            var items = await _productService.GetDataForExportAsync(dateStart, dateEnd);
            var title = $"Отчет по товарам с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}";
            var html = ExportBuilder.Build(items, title);
            var emailTo = _configService.Config.ExportEmail;
            var emailFrom = _configService.Config.RobotEmail;

//            var path = Environment.CurrentDirectory;
//            var fileAttachedName = $"{title}.xls";
//            var filePath = Path.Combine(path, fileAttachedName);
//            var fileStream = new FileStream(filePath, FileMode.Open);
            var stream = GenerateStreamFromString(html);
            var fileImage = new MemoryFile(stream, "application/vnd.ms-excel", $"{title}.xsl");

             _emailService.SendMail(emailFrom, emailTo, title, title, new List<HttpPostedFileBase> { fileImage });

            return new ExcelResult(fileName, html);
        }

        private Stream GenerateStreamFromString(string s) {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream)) {

                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
        }


        private class MemoryFile : HttpPostedFileBase {
            readonly Stream _stream;
            readonly string _contentType;
            readonly string _fileName;

            public MemoryFile(Stream stream, string contentType, string fileName) {
                _stream = stream;
                _contentType = contentType;
                _fileName = fileName;
            }

            public override int ContentLength {
                get { return (int)_stream.Length; }
            }

            public override string ContentType {
                get { return _contentType; }
            }

            public override string FileName {
                get { return _fileName; }
            }

            public override Stream InputStream {
                get { return _stream; }
            }

            public override void SaveAs(string filename) {
                using (var file = System.IO.File.Open(filename, FileMode.CreateNew))
                    
                    _stream.CopyTo(file);
            }
        }
    }
}

