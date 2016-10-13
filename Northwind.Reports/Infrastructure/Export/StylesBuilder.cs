using System.Collections.ObjectModel;
using Calabonga.Xml.Exports;

namespace Northwind.Web {

    /// <summary>
    /// Styles for Workbook
    /// </summary>
    public class StylesBuilder {

        public const string TitleStyle = "Title-bold";
        public const string Headerstyle = "header-gray";
        public const string SummaryStyle = "summary-red";

        /// <summary>
        /// Sets up styles for workbook
        /// </summary>
        /// <param name="workbook"></param>
        public static void Apply(Workbook workbook)
        {

            var s0 = new Style(TitleStyle)
            {
                Font =
                {
                    Size = 16,
                    Bold = true,
                    Color = "#000000"
                },
                Alignment = new Alignment
                {
                    Horizontal = Horizontal.Center
                }
            };
            workbook.AddStyle(s0);

            // create style s1 for header
            var s1 = new Style(SummaryStyle) {
                Font =
                {
                    Bold = true,
                    Color = "#000000"
                },
                Borders = GetBorders(),
                Interior = new Interior() {
                    Color = "#cccccc",
                    Pattern = Pattern.Solid
                }
            };
            workbook.AddStyle(s1);

            // create style s2 for header
            var s2 = new Style(Headerstyle) {
                Font =
                {
                    Bold = true,
                    Size = 12
                },
                Borders = GetBorders(),
                Interior = new Interior() {
                    Color = "#dedede",
                    Pattern = Pattern.Solid
                }
            };

            workbook.AddStyle(s2);
        }

        private static Collection<Border> GetBorders() {
            return new Collection<Border>
            {
                new Border
                {
                    Color = "#888",
                    Weight = Weight.Hairline,
                    LineStyle = LineStyle.Continuous,
                    Position = Position.Bottom
                },
                new Border
                {
                    Color = "#888",
                    Weight = Weight.Hairline,
                    LineStyle = LineStyle.Continuous,
                    Position = Position.Left
                },
                new Border
                {
                    Color = "#888",
                    Weight = Weight.Hairline,
                    LineStyle = LineStyle.Continuous,
                    Position = Position.Right
                },
                new Border
                {
                    Color = "#888",
                    Weight = Weight.Hairline,
                    LineStyle = LineStyle.Continuous,
                    Position = Position.Top
                }
            };
        }
    }
}
