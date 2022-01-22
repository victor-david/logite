using Restless.Logite.Controls;
using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.Database.Tables;
using Restless.Logite.Resources;
using Restless.Logite.ViewModel.Domain;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Restless.Logite.ViewModel
{
    public class MainWindowViewModel : ApplicationViewModel
    {
        #region Private
        private ApplicationViewModel selectedViewModel;
        private readonly ViewModelCache viewModelCache;
        private GridLength mainNavigationWidth;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

        private MainWindowViewModel()
        {
            DisplayName = $"{AppInfo.Assembly.Product} {AppInfo.Assembly.VersionMajor}";
            Commands.Add("SaveData", p => DatabaseController.Instance.Save());
            Commands.Add("ExitApp", p => Application.Current.MainWindow.Close());
            Commands.Add("ResetWindow", RunResetWindowCommand);
            Commands.Add("OpenAbout", RunOpenAboutWindow);
            Commands.Add("NavigateStart", p => NavigatorItems.Select<StartViewModel>());
            Commands.Add("NavigateSettings", p => NavigatorItems.Select<SettingsViewModel>());
            Commands.Add("NavigateTable", p => NavigatorItems.Select<TableViewModel>());
            MainNavigationWidth = new GridLength(Config.MainNavigationWidth, GridUnitType.Pixel);
            viewModelCache = new ViewModelCache();
            NavigatorItems = new NavigatorItemCollection(3);
            NavigatorItems.SelectedItemChanged += NavigatorItemsSelectedItemChanged;
            RegisterStandardNavigatorItems();
            RegisterDomainNavigatorItems();
            StartupNavigation();
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the navigator items
        /// </summary>
        public NavigatorItemCollection NavigatorItems
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected view model.
        /// </summary>
        public ApplicationViewModel SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                var prevSelect = selectedViewModel;
                if (SetProperty(ref selectedViewModel, value))
                {
                    ChangeViewModelActivationState(prevSelect, selectedViewModel);
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the main navigation pane.
        /// </summary>
        public GridLength MainNavigationWidth
        {
            get => mainNavigationWidth;
            set
            {
                if (SetProperty(ref mainNavigationWidth, value))
                {
                    Config.MainNavigationWidth = (int)value.Value;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Public methods
        ///// <summary>
        ///// Navigates to the specified view model
        ///// </summary>
        ///// <typeparam name="T">The view model type</typeparam>
        //public void NavigateTo<T>() where T : ApplicationViewModel, new()
        //{
        //    NavigateTo(typeof(T));
        //}

        /// <summary>
        /// Gets geometry for a navigator item.
        /// </summary>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>The geometry resource, or null if it doesn't exist.</returns>
        public Geometry GetGeometry(object resourceId)
        {
            return LocalResources.Get<Geometry>(resourceId);
        }


        #endregion

        /************************************************************************/

        #region Protected methods
        /// <inheritdoc/>
        protected override void OnClosing()
        {
            base.OnClosing();
            Window main = Application.Current.MainWindow;
            if (main.WindowState != WindowState.Maximized)
            {
                Config.MainWindowWidth = (int)main.Width;
                Config.MainWindowHeight = (int)main.Height;
            }
            if (main.WindowState != WindowState.Minimized)
            {
                Config.MainWindowState = main.WindowState;
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void RegisterStandardNavigatorItems()
        {
            NavigatorItems.Add<StartViewModel>(NavigationGroup.Services, Strings.MenuItemStart, false, GetGeometry(GeometryKeys.ClipboardGeometryKey));
            NavigatorItems.Add<ImportViewModel>(NavigationGroup.Services, Strings.MenuItemImport, false, GetGeometry(GeometryKeys.FileGeometryKey));
            NavigatorItems.Add<SettingsViewModel>(NavigationGroup.Tools, Strings.MenuItemSettings, false, GetGeometry(GeometryKeys.SettingsGeometryKey));
            NavigatorItems.Add<TableViewModel>(NavigationGroup.Tools, Strings.MenuItemTableInfo, false, GetGeometry(GeometryKeys.DatabaseGeometryKey));
        }

        private void RegisterDomainNavigatorItems()
        {
            NavigatorItems.Clear<DomainViewModel>();
            foreach (DomainRow domain in DatabaseController.Instance.GetTable<DomainTable>().EnumerateAll())
            {
                NavigatorItems.Add<DomainViewModel>(NavigationGroup.Domain, domain.DisplayName, true, GetGeometry(GeometryKeys.LogGeometryKey), domain.Id);
            }
        }

        private void NavigatorItemsSelectedItemChanged(object sender, NavigatorItem navItem)
        {
            if (navItem != null && navItem.TargetType.IsAssignableTo(typeof(ApplicationViewModel)))
            {
                SelectedViewModel = viewModelCache.GetByNavigationItem(navItem);
            }
        }

        private void StartupNavigation()
        {
            if (Config.NavigateStart)
            {
                NavigatorItems.Select<StartViewModel>();
            }
        }

        private void RunResetWindowCommand(object parm)
        {
            Window main = Application.Current.MainWindow;
            main.Width = Config.Default.MainWindow.Width;
            main.Height = Config.Default.MainWindow.Height;
            main.Top = (SystemParameters.WorkArea.Height / 2) - (main.Height / 2);
            main.Left = (SystemParameters.WorkArea.Width / 2) - (main.Width / 2);
            main.WindowState = WindowState.Normal;
        }

        private void RunOpenAboutWindow(object parm)
        {
            WindowFactory.About.Create().ShowDialog();
        }

        /// <summary>
        /// Changes the activation state of the specified view models.
        /// </summary>
        /// <param name="previous">The previously selected view model. Gets deactived.</param>
        /// <param name="current">The currently selected view model. Gets activated.</param>
        private void ChangeViewModelActivationState(ApplicationViewModel previous, ApplicationViewModel current)
        {
            /* Deactivate the previously selected view model */
            previous?.Deactivate();
            current?.Activate();
        }

        #endregion
    }
}
