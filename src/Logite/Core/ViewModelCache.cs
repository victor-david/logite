using Restless.Logite.Controls;
using Restless.Logite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents view models that have been cached for reuse.
    /// </summary>
    public class ViewModelCache : List<ApplicationViewModel>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCache"/> class.
        /// </summary>
        public ViewModelCache()
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets the <see cref="ApplicationViewModel"/> specified by <see cref="NavigatorItem"/>
        /// from cache. If it doesn't exists, creates it and caches it first.
        /// </summary>
        /// <param name="navItem">The navigation item.</param>
        /// <returns>An <see cref="ApplicationViewModel"/> object.</returns>
        public ApplicationViewModel GetByNavigationItem(NavigatorItem navItem)
        {
            if (navItem == null) throw new ArgumentNullException(nameof(navItem));

            //if (navItem.GroupIndex == NavigationGroup.Domain && navItem.TargetType == typeof(DomainViewModel))
            //{
            //    return GetDomainItem(navItem, owner);
            //}
            return GetStandardItem(navItem);
        }

        /// <summary>
        /// Signals all <see cref="ApplicationViewModel"/> objects in the collection
        /// that they are closing.
        /// </summary>
        public void SignalClosing()
        {
            foreach (var item in this)
            {
                item.SignalClosing();
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private ApplicationViewModel GetStandardItem(NavigatorItem navItem)
        {
            foreach (var item in this)
            {
                if (item.GetType() == navItem.TargetType)
                {
                    return item;
                }
            }

            ApplicationViewModel createdItem = Activator.CreateInstance(navItem.TargetType) as ApplicationViewModel;
            Add(createdItem);
            return createdItem;
        }

        private ApplicationViewModel GetDomainItem(NavigatorItem navItem, ApplicationViewModel owner)
        {
            throw new NotImplementedException();
            //foreach (var item in this.OfType<DomainViewModel>())
            //{
            //    if (item.Account.Id == navItem.Id)
            //    {
            //        return item;
            //    }
            //}

            //var account = DatabaseController.Instance.GetTable<AccountTable>().GetSingleRecord(navItem.Id);
            //var createdItem = new RegisterViewModel(owner, account);
            //Add(createdItem);
            //return createdItem;
        }
        #endregion
    }
}
