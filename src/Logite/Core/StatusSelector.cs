using Restless.Toolkit.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;


namespace Restless.Logite.Core
{
    public class StatusSelector : ObservableObject, IEnumerable<StatusSelection>
    {
        #region Private
        private List<StatusSelection> storage;
        private StatusSelection selectedStatus;
        private string title;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the preface that is applied to <see cref="Title"/>.
        /// </summary>
        public string TitlePreface
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the title used for the drop down.
        /// </summary>
        public string Title
        {
            get => title;
            private set => SetProperty(ref title, value);
        }

        /// <summary>
        /// Gets or sets the selected status.
        /// </summary>
        public StatusSelection SelectedStatus
        {
            get => selectedStatus;
            set
            {
                if (SetProperty(ref selectedStatus, value) && selectedStatus != null)
                {
                    HandleSelectedStatusChanged();
                }
            }
        }

        /// <summary>
        /// Gets the count of items that are currently selected.
        /// </summary>
        public int SelectionCount
        {
            get => this.Where(s => s.IsSelected).Count();
        }

        /// <summary>
        /// Gets the sum of all bit values from selectors that are currently selected.
        /// </summary>
        public long BitValueSelectedSum
        {
            get => GetBitValueSelectedSum();
        }

        /// <summary>
        /// Gets or sets a method that is called when <see cref="SelectedTag"/> changes.
        /// The <see cref="TagSelector"/> passed to the method will not be null.
        /// </summary>
        public Action<StatusSelection> OnSelectedStatusChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a method that is called when all tag selections are cleared.
        /// </summary>
        public Action OnClearAllStatus
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusSelector"/> class.
        /// </summary>
        public StatusSelector()
        {
            storage = new List<StatusSelection>();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a new status selection
        /// </summary>
        /// <param name="name">The selection name</param>
        /// <param name="value">The standard value for this selector, 200, 404, etc.</param>
        /// <param name="bitValue">The bit mapped value that identifies the status</param>
        /// <param name="brush">The brush used for this selection</param>
        public void AddStatus(string name, long value, long bitValue, Brush brush)
        {
            storage.Add(new StatusSelection(name, StatusSelectionType.Status, value, bitValue, brush));
        }

        /// <summary>
        /// Adds a separator selection
        /// </summary>
        public void AddSeparator()
        {
            storage.Add(new StatusSelection("xxx", StatusSelectionType.Separator, 0, -1, null));
        }

        /// <summary>
        /// Adds a clear status selection
        /// </summary>
        /// <param name="text">The text for the selection.</param>
        public void AddClearStatus(string text)
        {
            storage.Add(new StatusSelection(text, StatusSelectionType.Clear, 0, -1, null));
        }

        /// <summary>
        /// Initializes. Call after all items have been added.
        /// </summary>
        /// <param name="bitValue">The current bit value.</param>
        public void Initialize(long bitValue)
        {
            foreach (StatusSelection selection in this)
            {
                if ((selection.BitValue | bitValue) == bitValue)
                {
                    selection.IsSelected = true;
                }
            }
            UpdateTitle();
        }

        /// <summary>
        /// Provides an enumeration that enumerates items which are selected.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatusSelection> EnumerateSelected()
        {
            foreach (StatusSelection selection in this.Where(s => s.IsSelected))
            {
                yield return selection;
            }
        }
        #endregion

        /************************************************************************/

        #region IEnumerable interface
        public IEnumerator<StatusSelection> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return storage.GetEnumerator();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private long GetBitValueSelectedSum()
        {
            long sum = 0;
            foreach (StatusSelection selection in this.Where(s => s.IsSelected))
            {
                sum += selection.BitValue;
            }
            return sum;
        }

        private void UpdateTitle()
        {
            Title = $"{TitlePreface} ({SelectionCount})";
        }

        private void HandleSelectedStatusChanged()
        {
            switch (selectedStatus.Type)
            {
                case  StatusSelectionType.Status:
                    selectedStatus.IsSelected = !selectedStatus.IsSelected;
                    OnSelectedStatusChanged?.Invoke(selectedStatus);
                    break;

                case StatusSelectionType.Clear:
                    ClearAllStatusSelections();
                    break;
            }
            UpdateTitle();
        }

        private void ClearAllStatusSelections()
        {
            foreach (StatusSelection tagSelector in storage)
            {
                tagSelector.IsSelected = false;
            }
            OnClearAllStatus?.Invoke();
        }
        #endregion
    }
}