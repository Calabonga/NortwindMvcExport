using Calabonga.Portal.Config;

namespace Northwind.Web {

    public class CurrentAppSettings : AppSettings {

        /// <summary>
        /// Current application version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Email uses to send report (excel-file)
        /// </summary>
        public string ExportEmail { get; set; }
    }
}