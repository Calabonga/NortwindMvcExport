using System;
using System.ComponentModel.DataAnnotations;
using Calabonga.PagedListLite;

namespace Northwind.Web.ViewModels {

    public class ProductSelectorViewModel {

        private DateTime _dateStart = new DateTime(1997, 1, 1);
        private DateTime _dateEnd = new DateTime(1997, 1, 31);

        public IPagedList<ProductViewModel> Products { get; set; }

        /// <summary>
        /// Property Начало периода
        /// </summary>
        [Display(Name = "Начало периода")]
        [Required(ErrorMessage = "Начало периода - обязательное поле")]
        [DataType(DataType.Date)]
        public DateTime DateStart {
            get { return _dateStart; }
            set { _dateStart = value; }
        }

        /// <summary>
        /// Property Окончание периода
        /// </summary>
        [Display(Name = "Окончание периода")]
        [Required(ErrorMessage = "Окончание периода - обязательное поле")]
        [DataType(DataType.Date)]
        public DateTime DateEnd {
            get { return _dateEnd; }
            set { _dateEnd = value; }
        }

        public int Size { get; set; }

        public int Index { get; set; }
    }
}