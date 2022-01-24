using System;

namespace Restless.Logite.Core
{
    /// <summary>
    /// Represents a single selection for <see cref="SectionSelector"/>
    /// </summary>
    public class Section
    {
        #region Properties
        /// <summary>
        /// Gets the section id.
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// Gets the section name.
        /// </summary>
        public string Name
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="id">The id for the section</param>
        /// <param name="name">The section name</param>
        public Section(long id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Id = id;
            Name = name;
        }
        #endregion
    }
}