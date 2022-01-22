using Restless.Logite.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Logite.ViewModel
{
    public class SettingsViewModel : ApplicationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class
        /// </summary>
        public SettingsViewModel()
        {
            DisplayName = Strings.MenuItemSettings;
        }
    }
}
