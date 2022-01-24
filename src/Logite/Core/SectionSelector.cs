using Restless.Toolkit.Mvvm;
using System;
using System.Collections.Generic;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents a section selector
    /// </summary>
    public class SectionSelector : ObservableObject
    {
        #region Private
        private string title;
        private Section selectedSection;
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
        /// Gets the section list.
        /// </summary>
        public List<Section> Sections
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected section
        /// </summary>
        public Section SelectedSection
        {
            get => selectedSection;
            set
            {
                if (SetProperty(ref selectedSection, value) && selectedSection != null)
                {
                    Title = $"{TitlePreface}: {selectedSection?.Name}";
                    OnSectionChanged(selectedSection);
                }
            }
        }

        /// <summary>
        /// Raised when the section has changed
        /// </summary>
        public event EventHandler<Section> SectionChanged;
        #endregion

        /************************************************************************/

        #region Constructor
        public SectionSelector()
        {
            Sections = new List<Section>();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a new section
        /// </summary>
        /// <param name="id">The section id</param>
        /// <param name="name">The section name</param>
        public void Add(long id, string name)
        {
            Sections.Add(new Section(id, name));
        }

        /// <summary>
        /// Sets the selected section according to the section id.
        /// </summary>
        /// <param name="id">The id</param>
        public void SetSelectedSection(long id)
        {
            foreach (Section section in Sections)
            {
                if (section.Id == id)
                {
                    SelectedSection = section;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Raises the <see cref="SectionChanged"/> event.
        /// </summary>
        protected virtual void OnSectionChanged(Section e)
        {
            SectionChanged?.Invoke(this, e);
        }
        #endregion
    }
}
