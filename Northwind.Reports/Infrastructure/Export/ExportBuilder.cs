using System;
using System.Collections.Generic;
using Calabonga.Xml.Exports;
using Northwind.Web.ViewModels;

namespace Northwind.Web {
    public static class ExportBuilder {
        /// <summary>
        /// Build Excel file
        /// </summary>
        /// <param name="items">product</param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string Build(List<ProductViewModel> items, string title="Без заголовка") {
            var workbook = new Workbook();
            PropertiesBuilder.Apply(workbook);
            StylesBuilder.Apply(workbook);

            // create first worksheet
            var worksheet = new Worksheet("Лист 1");

            worksheet.AddCellWithStyle(0,0, title, StylesBuilder.TitleStyle, 6U);

            // adding headers
            worksheet.AddCellWithStyle(1, 0, "Номер заказа", StylesBuilder.Headerstyle);
            worksheet.AddCellWithStyle(1, 1, "Дата заказа", StylesBuilder.Headerstyle);
            worksheet.AddCellWithStyle(1, 2, "Артикул товара", StylesBuilder.Headerstyle);
            worksheet.AddCellWithStyle(1, 3, "Наименование товара", StylesBuilder.Headerstyle);
            worksheet.AddCellWithStyle(1, 4, "Количество", StylesBuilder.Headerstyle);
            worksheet.AddCellWithStyle(1, 5, "Цена", StylesBuilder.Headerstyle);
            worksheet.AddCellWithStyle(1, 6, "Сумма", StylesBuilder.Headerstyle);

            var row = 2;
            foreach (var item in items) {
                worksheet.AddCell(row, 0, item.Id);
                worksheet.AddCell(row, 1, item.OrderDate.ToString());
                worksheet.AddCell(row, 2, item.ProductId);
                worksheet.AddCell(row, 3, item.ProductName);
                worksheet.AddCell(row, 4, item.Quantity);
                worksheet.AddCell(row, 5, item.UnitPrice);
                worksheet.AddCellWithFormula(row, 6, 0, "=RC[-2]*RC[-1]");
                row++;
            }

            // appending footer with formulas
            worksheet.AddCellWithStyle(row, 0, string.Empty, StylesBuilder.SummaryStyle);
            worksheet.AddCellWithStyle(row, 1, string.Empty, StylesBuilder.SummaryStyle);
            worksheet.AddCellWithStyle(row, 2, string.Empty, StylesBuilder.SummaryStyle);
            worksheet.AddCellWithStyle(row, 3, string.Empty, StylesBuilder.SummaryStyle);
            worksheet.AddCellWithStyle(row, 4, string.Empty, StylesBuilder.SummaryStyle);
            worksheet.AddCellWithStyleAndFormula(row, 5, 0, "=AVERAGE(R[-" + (row - 1) + "]C:R[-1]C)", StylesBuilder.SummaryStyle);
            worksheet.AddCellWithStyleAndFormula(row, 6, 0, "=SUM(R[-" + (row - 1) + "]C:R[-1]C)", StylesBuilder.SummaryStyle);

            workbook.AddWorksheet(worksheet);
            return workbook.ExportToXML();
        }
    }
}
