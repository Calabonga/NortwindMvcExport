using System;
using Calabonga.Xml.Exports;

namespace Northwind.Web {

    /// <summary>
    /// Property for Workbook
    /// </summary>
    public class PropertiesBuilder {

        /// <summary>
        /// Sets up workbook property
        /// </summary>
        /// <param name="workbook"></param>
        public static void Apply(Workbook workbook) {
            // properties
            workbook.Properties.Author = "Calabonga";
            workbook.Properties.Created = DateTime.Today;
            workbook.Properties.LastAutor = "Calabonga";
            workbook.Properties.Version = "14";

            // options sheets
            workbook.ExcelWorkbook.ActiveSheet = 1;
            workbook.ExcelWorkbook.DisplayInkNotes = false;
            workbook.ExcelWorkbook.FirstVisibleSheet = 1;
            workbook.ExcelWorkbook.ProtectStructure = false;
            workbook.ExcelWorkbook.WindowHeight = 800;
            workbook.ExcelWorkbook.WindowTopX = 0;
            workbook.ExcelWorkbook.WindowTopY = 0;
            workbook.ExcelWorkbook.WindowWidth = 600;
        }
    }
}
