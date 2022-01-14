using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm.Collections;
using System;
using System.ComponentModel;
using System.Data;

namespace Restless.Logite.ViewModel
{
    /// <summary>
    /// Provides the logic that is used to display stats for a specified domain.
    /// </summary>
    public class DomainViewModel : ApplicationViewModel
    {
        #region Private
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the domain object for this domain view.
        /// </summary>
        public DomainRow Domain
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainViewModel"/> class.
        /// </summary>
        public DomainViewModel(DomainRow domain) : base()
        {
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
            DisplayName = Domain.DisplayName;

            //Commands.Add("ConfigureDomains", RunConfigureDomainsCommand);
            //Commands.Add("ConfigureLogDirectory", RunConfigureLogDirectoryCommand);
            //Commands.Add("ConfigureDatabaseDirectory", RunConfigureDatabaseDirectoryCommand);
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        #endregion

        /************************************************************************/

        #region Private methods
        private void RunConfigureDomainsCommand(object parm)
        {
        }
        #endregion
    }
}