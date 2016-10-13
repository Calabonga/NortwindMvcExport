using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Northwind.Web {
    /// <summary>
	/// Выдает пользователю для загрузки файл Excel.
	/// </summary>
	public class ExcelResult : ActionResult {

        /// <summary>
        /// Создает экземпляр класса, которые выдает файл Excel
        /// </summary>
        /// <param name="fileName">наименование файла для экспорта</param>
        /// <param name="report">готовый набор данные для экпорта</param>
        public ExcelResult(string fileName, StringBuilder report) {
            if (report == null) throw new ArgumentNullException(nameof(report));
            FileName = fileName;
            Html = report.ToString();
        }

        /// <summary>
        /// Создает экземпляр класса, которые выдает файл Excel
        /// </summary>
        /// <param name="fileName">наименование файла для экспорта</param>
        /// <param name="html">готовый набор данные для экпорта</param>
        public ExcelResult(string fileName, string html) {
            FileName = fileName;
            Html = html;
        }

        /// <summary>
        /// StringBuilder с подготовленным отчетом (html)
        /// </summary>
        public string Html { get; private set; }

        /// <summary>
        /// Название файла для выгрузки
        /// </summary>
        public string FileName { get; private set; }


        public override void ExecuteResult(ControllerContext context) {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.BufferOutput = true;
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment; filename={FileName}");
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Write(Html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}
