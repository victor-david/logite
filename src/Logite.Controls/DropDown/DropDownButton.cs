using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Restless.Logite.Controls
{
    /// <summary>
    /// Represents a drop down button
    /// </summary>
    public class DropDownButton : ButtonBase
    {
        #region Private
        private bool ignoreClickChange;
        private static readonly Color DefaultDropDownShadowColor = Colors.LightGray;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownButton"/> class.
        /// </summary>
        public DropDownButton()
        {
        }

        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }
        #endregion

        /************************************************************************/

        #region Properties (data)
        /// <summary>
        /// Gets or sets the items source for the drop down items.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register
            (
                nameof(ItemsSource), typeof(IEnumerable), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                }
            );

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register
            (
                nameof(SelectedItem), typeof(object), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnSelectedItemChanged
                }
            );

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropDownButton control && control.CloseOnSelection)
            {
                control.IsDropDownOpen = false;
            }
        }

        /// <summary>
        /// Gets or sets the display member path.
        /// </summary>
        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DisplayMemberPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register
            (
                nameof(DisplayMemberPath), typeof(string), typeof(DropDownButton), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the item template for the drop down items
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register
            (
                nameof(ItemTemplate), typeof(DataTemplate), typeof(DropDownButton), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the custom logic for choosing a template used to display each item.
        /// </summary>
        public DataTemplateSelector ItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            set => SetValue(ItemTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemTemplateSelector"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register
            (
                nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(DropDownButton), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the style for the item container.
        /// </summary>
        public Style ItemContainerStyle
        {
            get => (Style)GetValue(ItemContainerStyleProperty);
            set => SetValue(ItemContainerStyleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemContainerStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register
            (
                nameof(ItemContainerStyle), typeof(Style), typeof(DropDownButton), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the selector for item container style.
        /// </summary>
        public StyleSelector ItemContainerStyleSelector
        {
            get => (StyleSelector)GetValue(ItemContainerStyleSelectorProperty);
            set => SetValue(ItemContainerStyleSelectorProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemContainerStyleSelector"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleSelectorProperty = DependencyProperty.Register
            (
                nameof(ItemContainerStyleSelector), typeof(StyleSelector), typeof(DropDownButton), new FrameworkPropertyMetadata()
            );
        #endregion

        /************************************************************************/

        #region Properties (appearance)
        /// <summary>
        /// Gets or sets the rollover brush.
        /// </summary>
        public Brush RolloverBrush
        {
            get => (Brush)GetValue(RolloverBrushProperty);
            set => SetValue(RolloverBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="RolloverBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RolloverBrushProperty = DependencyProperty.Register
            (
                nameof(RolloverBrush), typeof(Brush), typeof(DropDownButton), new PropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the brush for the drop down chevron.
        /// </summary>
        public Brush ChevronBrush
        {
            get => (Brush)GetValue(ChevronBrushProperty);
            set => SetValue(ChevronBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ChevronBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChevronBrushProperty = DependencyProperty.Register
            (
                nameof(ChevronBrush), typeof(Brush), typeof(DropDownButton), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Black
                }
            );

        /// <summary>
        /// Gets or sets corner radius.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            (
                nameof(CornerRadius), typeof(CornerRadius), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new CornerRadius(2,2,0,0)
                }
            );

        /// <summary>
        /// Gets or sets the background for the drop down items.
        /// </summary>
        public Brush DropDownBackground
        {
            get => (Brush)GetValue(DropDownBackgroundProperty);
            set => SetValue(DropDownBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DropDownBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropDownBackgroundProperty = DependencyProperty.Register
            (
                nameof(DropDownBackground), typeof(Brush), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Brushes.White
                }
            );

        /// <summary>
        /// Gets or sets the color for the drop down shadow.
        /// </summary>
        public Color DropDownShadowColor
        {
            get => (Color)GetValue(DropDownShadowColorProperty);
            set => SetValue(DropDownShadowColorProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DropDownShadowColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropDownShadowColorProperty = DependencyProperty.Register
            (
                nameof(DropDownShadowColor), typeof(Color), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultDropDownShadowColor,
                    PropertyChangedCallback = OnDropDownShadowColorChanged
                }
            );

        private static void OnDropDownShadowColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropDownButton control)
            {
                control.DropDownEffect = new DropShadowEffect()
                {
                    Color = (Color)e.NewValue,
                    ShadowDepth = 2.0
                };
            }
        }

        /// <summary>
        /// Gets the effect applied to the drop down.
        /// </summary>
        public Effect DropDownEffect
        {
            get => (Effect)GetValue(DropDownEffectProperty);
            private set => SetValue(DropDownEffectPropertyKey, value);
        }

        private static readonly DependencyPropertyKey DropDownEffectPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(DropDownEffect), typeof(Effect), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new DropShadowEffect()
                    {
                        Color = DefaultDropDownShadowColor,
                        ShadowDepth = 2.0
                    }
                }
            );

        /// <summary>
        /// Identifies the <see cref="DropDownEffect"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropDownEffectProperty = DropDownEffectPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the width for the drop down.
        /// </summary>
        public double DropDownWidth
        {
            get => (double)GetValue(DropDownWidthProperty);
            set => SetValue(DropDownWidthProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DropDownWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropDownWidthProperty = DependencyProperty.Register
            (
                nameof(DropDownWidth), typeof(double), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = double.NaN
                }
            );
        #endregion

        /************************************************************************/

        #region Properties (behavior)
        /// <summary>
        /// Gets or sets whether the drop down is open.
        /// </summary>
        /// <remarks>
        /// This property cannot be set to true until the control is loaded.
        /// </remarks>
        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register
            (
                nameof(IsDropDownOpen), typeof(bool), typeof(DropDownButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = false,
                    BindsTwoWayByDefault = true,
                    CoerceValueCallback = CoerceIsDropDownOpen,
                }
            );

        private static object CoerceIsDropDownOpen(DependencyObject d, object value)
        {
            if (d is DropDownButton control && !control.IsLoaded)
            {
                return false;
            }
            return value;
        }

        /// <summary>
        /// Gets or sets a value that determines whether the drop down closes upon selection of an item.
        /// </summary>
        public bool CloseOnSelection
        {
            get => (bool)GetValue(CloseOnSelectionProperty);
            set => SetValue(CloseOnSelectionProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CloseOnSelection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CloseOnSelectionProperty = DependencyProperty.Register
            (
                nameof(CloseOnSelection), typeof(bool), typeof(DropDownButton), new PropertyMetadata()
                {
                    DefaultValue = false
                }
            );
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <inheritdoc/>
        protected override void OnClick()
        {
            base.OnClick();
            if (!ignoreClickChange)
            {
                IsDropDownOpen = !IsDropDownOpen;
            }
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            // Prevents the drop down from closing and then opening again on mouse up.
            ignoreClickChange = IsDropDownOpen;
        }
        #endregion
    }
}