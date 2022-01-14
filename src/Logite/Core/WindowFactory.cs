using Restless.Logite.View;
using Restless.Logite.ViewModel;
using System.Windows;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Provides static methods for creating application windows.
    /// </summary>
    public static class WindowFactory
    {
        #region Main
        /// <summary>
        /// Provides static methods for creating the main application window.
        /// </summary>
        public static class Main
        {
            /// <summary>
            /// Creates an instance of MainWindow.
            /// </summary>
            /// <returns>The window</returns>
            public static MainWindow Create()
            {
                MainWindow window = new()
                {
                    MinWidth = Config.Default.MainWindow.MinWidth,
                    MinHeight = Config.Default.MainWindow.MinHeight,
                };

                TextOptions.SetTextFormattingMode(window);
                return window;
            }
        }
        #endregion

        /************************************************************************/

        #region About
        /// <summary>
        /// Provides static methods for creating the about window.
        /// </summary>
        public static class About
        {
            /// <summary>
            /// Creates an instance of AboutWindow.
            /// </summary>
            /// <returns>The window</returns>
            public static AboutWindow Create()
            {
                AboutWindow window = new()
                {
                    Owner = Application.Current.MainWindow,
                    DataContext = new AboutWindowViewModel()
                };
                TextOptions.SetTextFormattingMode(window);
                return window;
            }
        }
        #endregion

        /************************************************************************/

        #region AddAlias
        ///// <summary>
        ///// Provides static methods for creating the add alias window.
        ///// </summary>
        //public static class AddAlias
        //{
        //    /// <summary>
        //    /// Creates an instance of an AddAliasWindow and its correspondinf view model.
        //    /// </summary>
        //    /// <returns></returns>
        //    public static AddAliasWindow Create()
        //    {
        //        var window = new AddAliasWindow()
        //        {
        //            Owner = Application.Current.MainWindow,
        //            DataContext = new AddAliasWindowViewModel()
        //        };

        //        //((AddAliasWindowViewModel)window.DataContext).

        //        TextOptions.SetTextFormattingMode(window);
        //        return window;
        //    }
        //}
        #endregion


        ///// <summary>
        ///// Closes the specified window
        ///// </summary>
        ///// <param name="id">The id</param>
        //public static void CloseWindow(Guid id)
        //{
        //    foreach (Window window in Application.Current.Windows)
        //    {
        //        if (window.DataContext is IViewIdentification viewId && viewId.ViewId.Equals(id))
        //        {
        //            window.Close();
        //        }
        //    }
        //}

        /************************************************************************/

        #region TextOptions (private)
        private static class TextOptions
        {
            public static void SetTextFormattingMode(DependencyObject element)
            {
                System.Windows.Media.TextOptions.SetTextFormattingMode(element, System.Windows.Media.TextFormattingMode.Display);
            }
        }
        #endregion
    }
}
