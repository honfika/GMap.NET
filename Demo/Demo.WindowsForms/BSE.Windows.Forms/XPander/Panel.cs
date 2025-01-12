﻿using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Demo.WindowsForms.Properties;

namespace BSE.Windows.Forms
{
    #region Class Panel

    /// <summary>
    ///     Used to group collections of controls.
    /// </summary>
    /// <remarks>
    ///     The Panel is a control that contains other controls.
    ///     You can use a Panel to group collections of controls such as the XPanderPanelList control.
    ///     On top of the Panel there is the captionbar. This captionbar may contain an image and text.
    ///     According to it's dockstyle and properties the panel is collapsable and/or closable.
    /// </remarks>
    /// <copyright>
    ///     Copyright © 2006-2008 Uwe Eichkorn
    ///     THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
    ///     KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
    ///     IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
    ///     PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER
    ///     REMAINS UNCHANGED.
    /// </copyright>
    [Designer(typeof(PanelDesigner))]
    [DesignTimeVisibleAttribute(true)]
    [DefaultEvent("CloseClick")]
    [ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
    public partial class Panel : BasePanel
    {
        #region FieldsPrivate

        private Rectangle _restoreBounds;
        private bool _bShowTransparentBackground;
        private bool _bShowXPanderPanelProfessionalStyle;
        private bool _bShowCaptionbar;
        private LinearGradientMode _linearGradientMode;
        private Image _imageClosePanel;
        private CustomPanelColors _customColors;
        private Image _imgHoverBackground;
        private System.Windows.Forms.Splitter _associatedSplitter;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the associated Splitter. If there is a splitter associated to a panel,
        ///     the splitter visibility always changes when the visibilty of this panel changes.
        /// </summary>
        /// <value>The associated <see cref="Splitter" /></value>
        [Description("The associated Splitter.")]
        [Category("Behavior")]
        public virtual System.Windows.Forms.Splitter AssociatedSplitter
        {
            get
            {
                return _associatedSplitter;
            }
            set
            {
                _associatedSplitter = value;
            }
        }

        /// <summary>
        ///     Gets the custom colors which are used for the panel.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The custom colors which are used for the panel.")]
        [Category("Appearance")]
        public CustomPanelColors CustomColors
        {
            get
            {
                return _customColors;
            }
        }

        /// <summary>
        ///     Expands the panel.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Expand
        {
            get
            {
                return base.Expand;
            }
            set
            {
                base.Expand = value;
            }
        }

        /// <summary>
        ///     LinearGradientMode of the panels background
        /// </summary>
        [Description("LinearGradientMode of the Panels background")]
        [DefaultValue(1)]
        [Category("Appearance")]
        public LinearGradientMode LinearGradientMode
        {
            get
            {
                return _linearGradientMode;
            }
            set
            {
                if (value.Equals(_linearGradientMode) == false)
                {
                    _linearGradientMode = value;
                    Invalidate(false);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the panels captionbar is displayed.
        /// </summary>
        /// <example>
        ///     <code>
        /// private void btnShowHideCaptionbar_Click(object sender, EventArgs e)
        /// {
        ///     //displaye or hides the captionbar at the top of the panel
        ///     panel6.ShowCaptionbar = !panel6.ShowCaptionbar;
        /// }
        /// </code>
        /// </example>
        [Description("A value indicating whether the panels captionbar is displayed.")]
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool ShowCaptionbar
        {
            get
            {
                return _bShowCaptionbar;
            }
            set
            {
                if (value.Equals(_bShowCaptionbar) == false)
                {
                    _bShowCaptionbar = value;
                    Invalidate(true);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the controls background is transparent.
        /// </summary>
        [Description("Gets or sets a value indicating whether the controls background is transparent")]
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool ShowTransparentBackground
        {
            get
            {
                return _bShowTransparentBackground;
            }
            set
            {
                if (value.Equals(_bShowTransparentBackground) == false)
                {
                    _bShowTransparentBackground = value;
                    Invalidate(false);
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the controls caption professional colorscheme is the same then the
        ///     XPanderPanels
        /// </summary>
        [Description(
            "Gets or sets a value indicating whether the controls caption professional colorscheme is the same then the XPanderPanels")]
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool ShowXPanderPanelProfessionalStyle
        {
            get
            {
                return _bShowXPanderPanelProfessionalStyle;
            }
            set
            {
                if (value.Equals(_bShowXPanderPanelProfessionalStyle) == false)
                {
                    _bShowXPanderPanelProfessionalStyle = value;
                    Invalidate(false);
                }
            }
        }

        /// <summary>
        ///     Gets the size and location of the panel in it's normal expanded state.
        /// </summary>
        /// <remarks>
        ///     A Rect that specifies the size and location of a panel before being either collapsed
        /// </remarks>
        [Browsable(false)]
        public Rectangle RestoreBounds
        {
            get
            {
                return _restoreBounds;
            }
        }

        #endregion

        #region MethodsPublic

        /// <summary>
        ///     Initializes a new instance of the Panel class.
        /// </summary>
        public Panel()
        {
            InitializeComponent();

            CaptionFont = new Font(SystemFonts.CaptionFont.FontFamily,
                SystemFonts.CaptionFont.SizeInPoints + 2.75F,
                FontStyle.Bold);
            BackColor = Color.Transparent;
            ForeColor = SystemColors.ControlText;
            ShowTransparentBackground = true;
            ShowXPanderPanelProfessionalStyle = false;
            ColorScheme = ColorScheme.Professional;
            LinearGradientMode = LinearGradientMode.Vertical;
            Expand = true;
            CaptionHeight = 27;
            ImageSize = new Size(18, 18);
            _bShowCaptionbar = true;
            _customColors = new CustomPanelColors();
            _customColors.CustomColorsChanged += OnCustomColorsChanged;
        }

        /// <summary>
        ///     Sets the PanelProperties for the Panel
        /// </summary>
        /// <param name="panelColors">The PanelColors table</param>
        public override void SetPanelProperties(PanelColors panelColors)
        {
            _imgHoverBackground = null;
            base.SetPanelProperties(panelColors);
        }

        /// <summary>
        ///     Gets the rectangle that represents the display area of the Panel.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                var padding = Padding;
                var displayRectangle = new Rectangle(
                    ClientRectangle.Left + padding.Left,
                    ClientRectangle.Top + padding.Top,
                    ClientRectangle.Width - padding.Left - padding.Right,
                    ClientRectangle.Height - padding.Top - padding.Bottom);

                if (_bShowCaptionbar)
                {
                    if (Controls.Count > 0)
                    {
                        var xpanderPanelList = Controls[0] as XPanderPanelList;
                        if (xpanderPanelList != null && xpanderPanelList.Dock == DockStyle.Fill)
                        {
                            displayRectangle = new Rectangle(
                                padding.Left,
                                CaptionHeight + padding.Top + Constants.BorderThickness,
                                ClientRectangle.Width - padding.Left - padding.Right,
                                ClientRectangle.Height - CaptionHeight - padding.Top - padding.Bottom -
                                2 * Constants.BorderThickness);
                        }
                        else
                        {
                            displayRectangle = new Rectangle(
                                padding.Left + Constants.BorderThickness,
                                CaptionHeight + padding.Top + Constants.BorderThickness,
                                ClientRectangle.Width - padding.Left - padding.Right -
                                2 * Constants.BorderThickness,
                                ClientRectangle.Height - CaptionHeight - padding.Top - padding.Bottom -
                                2 * Constants.BorderThickness);
                        }
                    }
                }

                return displayRectangle;
            }
        }

        #endregion

        #region MethodsProtected

        /// <summary>
        ///     Raises the ExpandClick event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnExpandClick(object sender, EventArgs e)
        {
            Expand = !Expand;
            base.OnExpandClick(sender, e);
        }

        /// <summary>
        ///     Raises the ExpandIconHoverState changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A HoverStateChangeEventArgs that contains the event data.</param>
        protected override void OnExpandIconHoverStateChanged(object sender, HoverStateChangeEventArgs e)
        {
            Invalidate(RectangleExpandIcon);
            base.OnExpandIconHoverStateChanged(sender, e);
        }

        /// <summary>
        ///     Raises the CloseIconHoverStat changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A HoverStateChangeEventArgs that contains the event data.</param>
        protected override void OnCloseIconHoverStateChanged(object sender, HoverStateChangeEventArgs e)
        {
            Invalidate(RectangleCloseIcon);
            base.OnCloseIconHoverStateChanged(sender, e);
        }

        /// <summary>
        ///     Paints the background of the control.
        /// </summary>
        /// <param name="pevent">A PaintEventArgs that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (ShowTransparentBackground)
            {
                base.OnPaintBackground(pevent);
                BackColor = Color.Transparent;
            }
            else
            {
                var rectangleBounds = ClientRectangle;
                if (_bShowCaptionbar)
                {
                    BackColor = Color.Transparent;
                    rectangleBounds = new Rectangle(
                        ClientRectangle.Left,
                        ClientRectangle.Top + CaptionHeight,
                        ClientRectangle.Width,
                        ClientRectangle.Height - CaptionHeight);
                }

                RenderBackgroundGradient(
                    pevent.Graphics,
                    rectangleBounds,
                    PanelColors.PanelContentGradientBegin,
                    PanelColors.PanelContentGradientEnd,
                    LinearGradientMode);
            }
        }

        /// <summary>
        ///     Raises the Paint event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var panelStyle = PanelStyle;
            if (_bShowCaptionbar == false)
            {
                return;
            }

            using (var antiAlias = new UseAntiAlias(e.Graphics))
            {
                var graphics = e.Graphics;
                using (var clearTypeGridFit = new UseClearTypeGridFit(graphics))
                {
                    var captionRectangle = CaptionRectangle;
                    var colorGradientBegin = PanelColors.PanelCaptionGradientBegin;
                    var colorGradientEnd = PanelColors.PanelCaptionGradientEnd;
                    var colorGradientMiddle = PanelColors.PanelCaptionGradientMiddle;
                    var colorText = PanelColors.PanelCaptionText;
                    bool bShowXPanderPanelProfessionalStyle = ShowXPanderPanelProfessionalStyle;
                    var colorSchema = ColorScheme;

                    if (bShowXPanderPanelProfessionalStyle
                        && colorSchema == ColorScheme.Professional
                        && panelStyle != PanelStyle.Office2007)
                    {
                        colorGradientBegin = PanelColors.XPanderPanelCaptionGradientBegin;
                        colorGradientEnd = PanelColors.XPanderPanelCaptionGradientEnd;
                        colorGradientMiddle = PanelColors.XPanderPanelCaptionGradientMiddle;
                        colorText = PanelColors.XPanderPanelCaptionText;
                    }

                    var image = Image;
                    var rightToLeft = RightToLeft;
                    var captionFont = CaptionFont;
                    var clientRectangle = ClientRectangle;
                    string strText = Text;
                    var dockStyle = Dock;
                    bool bExpand = Expand;
                    if (_imageClosePanel == null)
                    {
                        _imageClosePanel = Resources.closePanel;
                    }

                    var colorCloseIcon = PanelColors.PanelCaptionCloseIcon;
                    if (colorCloseIcon == Color.Empty)
                    {
                        colorCloseIcon = colorText;
                    }

                    bool bShowExpandIcon = ShowExpandIcon;
                    bool bShowCloseIcon = ShowCloseIcon;

                    switch (panelStyle)
                    {
                        case PanelStyle.Default:
                        case PanelStyle.Office2007:
                            DrawStyleDefault(graphics,
                                captionRectangle,
                                colorGradientBegin,
                                colorGradientEnd,
                                colorGradientMiddle);
                            break;
                    }

                    DrawBorder(
                        graphics,
                        clientRectangle,
                        captionRectangle,
                        PanelColors.BorderColor,
                        PanelColors.InnerBorderColor);

                    if (dockStyle == DockStyle.Fill || dockStyle == DockStyle.None ||
                        bShowExpandIcon == false && bShowCloseIcon == false)
                    {
                        DrawImagesAndText(
                            graphics,
                            captionRectangle,
                            CaptionSpacing,
                            ImageRectangle,
                            image,
                            rightToLeft,
                            captionFont,
                            colorText,
                            strText);

                        return;
                    }

                    if (bShowExpandIcon || bShowCloseIcon)
                    {
                        var imageExpandPanel = GetExpandImage(dockStyle, bExpand);

                        DrawImagesAndText(
                            graphics,
                            dockStyle,
                            CaptionSpacing,
                            captionRectangle,
                            clientRectangle,
                            ImageRectangle,
                            image,
                            rightToLeft,
                            bShowCloseIcon,
                            _imageClosePanel,
                            colorCloseIcon,
                            ref RectangleCloseIcon,
                            bShowExpandIcon,
                            bExpand,
                            imageExpandPanel,
                            colorText,
                            ref RectangleExpandIcon,
                            captionFont,
                            colorText,
                            PanelColors.PanelCollapsedCaptionText,
                            strText);

                        if (_imgHoverBackground == null)
                        {
                            _imgHoverBackground = GetPanelIconBackground(
                                graphics,
                                ImageRectangle,
                                PanelColors.PanelCaptionSelectedGradientBegin,
                                PanelColors.PanelCaptionSelectedGradientEnd);
                        }

                        if (_imgHoverBackground != null)
                        {
                            var rectangleCloseIcon = RectangleCloseIcon;
                            if (rectangleCloseIcon != Rectangle.Empty)
                            {
                                if (HoverStateCloseIcon == HoverState.Hover)
                                {
                                    graphics.DrawImage(_imgHoverBackground, rectangleCloseIcon);
                                    DrawIcon(graphics,
                                        _imageClosePanel,
                                        rectangleCloseIcon,
                                        colorCloseIcon,
                                        rectangleCloseIcon.Y);
                                }
                            }

                            var rectangleExpandIcon = RectangleExpandIcon;
                            if (rectangleExpandIcon != Rectangle.Empty)
                            {
                                if (HoverStateExpandIcon == HoverState.Hover)
                                {
                                    graphics.DrawImage(_imgHoverBackground, rectangleExpandIcon);
                                    DrawIcon(graphics,
                                        imageExpandPanel,
                                        rectangleExpandIcon,
                                        colorText,
                                        rectangleExpandIcon.Y);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Raises the PanelCollapsing event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A XPanderStateChangeEventArgs that contains the event data.</param>
        protected override void OnPanelCollapsing(object sender, XPanderStateChangeEventArgs e)
        {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
                foreach (Control control in Controls)
                {
                    control.Hide();
                }
            }

            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
                if (ClientRectangle.Width > CaptionHeight)
                {
                    _restoreBounds = ClientRectangle;
                }

                Width = CaptionHeight;
            }

            if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
            {
                if (ClientRectangle.Height > CaptionHeight)
                {
                    _restoreBounds = ClientRectangle;
                }

                Height = CaptionHeight;
            }

            base.OnPanelCollapsing(sender, e);
        }

        /// <summary>
        ///     Raises the PanelExpanding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A XPanderStateChangeEventArgs that contains the event data.</param>
        protected override void OnPanelExpanding(object sender, XPanderStateChangeEventArgs e)
        {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
                foreach (Control control in Controls)
                {
                    control.Show();
                }

                //When ClientRectangle.Width > CaptionHeight the panel size has changed
                //otherwise the captionclick event was executed
                if (ClientRectangle.Width == CaptionHeight)
                {
                    Width = _restoreBounds.Width;
                }
            }

            if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
            {
                Height = _restoreBounds.Height;
            }

            base.OnPanelExpanding(sender, e);
        }

        /// <summary>
        ///     Raises the PanelStyleChanged event
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        protected override void OnPanelStyleChanged(object sender, PanelStyleChangeEventArgs e)
        {
            OnLayout(new LayoutEventArgs(this, null));
            base.OnPanelStyleChanged(sender, e);
        }

        /// <summary>
        ///     Raises the CreateControl method.
        /// </summary>
        protected override void OnCreateControl()
        {
            _restoreBounds = ClientRectangle;
            MinimumSize = new Size(CaptionHeight, CaptionHeight);
            base.OnCreateControl();
        }

        /// <summary>
        ///     Raises the Resize event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            if (ShowExpandIcon)
            {
                if (Expand == false)
                {
                    if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                    {
                        if (Width > CaptionHeight)
                        {
                            Expand = true;
                        }
                    }

                    if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
                    {
                        if (Height > CaptionHeight)
                        {
                            Expand = true;
                        }
                    }
                }
                else
                {
                    if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                    {
                        if (Width == CaptionHeight)
                        {
                            Expand = false;
                        }
                    }

                    if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
                    {
                        if (Height == CaptionHeight)
                        {
                            Expand = false;
                        }
                    }
                }
            }

            base.OnResize(e);
        }

        /// <summary>
        ///     Raises the <see cref="Control.VisibleChanged" />VisibleChanged event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            var associatedSplitter = AssociatedSplitter;
            if (associatedSplitter != null)
            {
                associatedSplitter.Visible = Visible;
            }

            base.OnVisibleChanged(e);
        }

        #endregion

        #region MethodsPrivate

        /// <summary>
        ///     Gets the background for an panelicon image
        /// </summary>
        /// <param name="graphics">The Graphics to draw on.</param>
        /// <param name="rectanglePanelIcon"></param>
        /// <param name="backgroundColorBegin"></param>
        /// <param name="backgroundColorEnd"></param>
        /// <returns></returns>
        private static Image GetPanelIconBackground(Graphics graphics, Rectangle rectanglePanelIcon,
            Color backgroundColorBegin, Color backgroundColorEnd)
        {
            var rectangle = rectanglePanelIcon;
            rectangle.X = 0;
            rectangle.Y = 0;
            Image image = new Bitmap(rectanglePanelIcon.Width, rectanglePanelIcon.Height, graphics);
            using (var imageGraphics = Graphics.FromImage(image))
            {
                RenderBackgroundGradient(
                    imageGraphics,
                    rectangle,
                    backgroundColorBegin,
                    backgroundColorEnd,
                    LinearGradientMode.Vertical);
            }

            return image;
        }

        private static void DrawStyleDefault(Graphics graphics,
            Rectangle captionRectangle,
            Color colorGradientBegin,
            Color colorGradientEnd,
            Color colorGradientMiddle)
        {
            RenderDoubleBackgroundGradient(
                graphics,
                captionRectangle,
                colorGradientBegin,
                colorGradientMiddle,
                colorGradientEnd,
                LinearGradientMode.Vertical,
                true);
        }

        private static void DrawBorder(
            Graphics graphics,
            Rectangle panelRectangle,
            Rectangle captionRectangle,
            Color borderColor,
            Color innerBorderColor)
        {
            using (var borderPen = new Pen(borderColor))
            {
                // Draws the innerborder around the captionbar
                var innerBorderRectangle = captionRectangle;
                innerBorderRectangle.Width -= Constants.BorderThickness;
                innerBorderRectangle.Offset(Constants.BorderThickness, Constants.BorderThickness);
                ControlPaint.DrawBorder(
                    graphics,
                    innerBorderRectangle,
                    innerBorderColor,
                    ButtonBorderStyle.Solid);

                // Draws the outer border around the captionbar
                ControlPaint.DrawBorder(
                    graphics,
                    panelRectangle,
                    borderColor,
                    ButtonBorderStyle.Solid);

                // Draws the line below the captionbar
                graphics.DrawLine(
                    borderPen,
                    captionRectangle.X,
                    captionRectangle.Y + captionRectangle.Height,
                    captionRectangle.Width,
                    captionRectangle.Y + captionRectangle.Height);

                if (panelRectangle.Height == captionRectangle.Height)
                {
                    return;
                }

                // Draws the border lines around the whole panel
                var panelBorderRectangle = panelRectangle;
                panelBorderRectangle.Y = captionRectangle.Height;
                panelBorderRectangle.Height -= captionRectangle.Height + (int)borderPen.Width;
                panelBorderRectangle.Width -= (int)borderPen.Width;
                Point[] points =
                {
                    new Point(panelBorderRectangle.X, panelBorderRectangle.Y),
                    new Point(panelBorderRectangle.X, panelBorderRectangle.Y + panelBorderRectangle.Height),
                    new Point(panelBorderRectangle.X + panelBorderRectangle.Width,
                        panelBorderRectangle.Y + panelBorderRectangle.Height),
                    new Point(panelBorderRectangle.X + panelBorderRectangle.Width, panelBorderRectangle.Y)
                };
                graphics.DrawLines(borderPen, points);
            }
        }

        private static Image GetExpandImage(DockStyle dockStyle, bool bIsExpanded)
        {
            Image image = null;
            if (dockStyle == DockStyle.Left && bIsExpanded)
            {
                image = Resources.ChevronLeft;
            }
            else if (dockStyle == DockStyle.Left && bIsExpanded == false)
            {
                image = Resources.ChevronRight;
            }
            else if (dockStyle == DockStyle.Right && bIsExpanded)
            {
                image = Resources.ChevronRight;
            }
            else if (dockStyle == DockStyle.Right && bIsExpanded == false)
            {
                image = Resources.ChevronLeft;
            }
            else if (dockStyle == DockStyle.Top && bIsExpanded)
            {
                image = Resources.ChevronUp;
            }
            else if (dockStyle == DockStyle.Top && bIsExpanded == false)
            {
                image = Resources.ChevronDown;
            }
            else if (dockStyle == DockStyle.Bottom && bIsExpanded)
            {
                image = Resources.ChevronDown;
            }
            else if (dockStyle == DockStyle.Bottom && bIsExpanded == false)
            {
                image = Resources.ChevronUp;
            }

            return image;
        }

        #endregion
    }

    #endregion

    #region Class PanelDesigner

    /// <summary>
    ///     Extends the design mode behavior of a Panel control that supports nested controls.
    /// </summary>
    internal class PanelDesigner : ParentControlDesigner
    {
        #region FieldsPrivate

        #endregion

        #region MethodsPublic

        /// <summary>
        ///     Initializes a new instance of the PanelDesigner class.
        /// </summary>
        public PanelDesigner()
        {
        }

        /// <summary>
        ///     Initializes the designer with the specified component.
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
        }

        /// <summary>
        ///     Gets the design-time action lists supported by the component associated with the designer.
        /// </summary>
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                // Create action list collection
                var actionLists = new DesignerActionListCollection();

                // Add custom action list
                actionLists.Add(new PanelDesignerActionList(Component));

                // Return to the designer action service
                return actionLists;
            }
        }

        #endregion

        #region MethodsProtected

        /// <summary>
        ///     Called when the control that the designer is managing has painted
        ///     its surface so the designer can paint any additional adornments on
        ///     top of the control.
        /// </summary>
        /// <param name="e">A PaintEventArgs that provides data for the event.</param>
        protected override void OnPaintAdornments(PaintEventArgs e)
        {
            base.OnPaintAdornments(e);
        }

        #endregion
    }

    #endregion

    #region Class XPanderPanelListDesignerActionList

    /// <summary>
    ///     Provides the class for types that define a list of items used to create a smart tag panel for the Panel.
    /// </summary>
    public class PanelDesignerActionList : DesignerActionList
    {
        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the panels captionbar is displayed.
        /// </summary>
        public bool ShowCaptionbar
        {
            get
            {
                return Panel.ShowCaptionbar;
            }
            set
            {
                SetProperty("ShowCaptionbar", value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the controls background is transparent.
        /// </summary>
        public bool ShowTransparentBackground
        {
            get
            {
                return Panel.ShowTransparentBackground;
            }
            set
            {
                SetProperty("ShowTransparentBackground", value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the controls caption professional colorscheme is the same then the
        ///     XPanderPanels
        /// </summary>
        public bool ShowXPanderPanelProfessionalStyle
        {
            get
            {
                return Panel.ShowXPanderPanelProfessionalStyle;
            }
            set
            {
                SetProperty("ShowXPanderPanelProfessionalStyle", value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the expand icon of the panel is visible
        /// </summary>
        public bool ShowExpandIcon
        {
            get
            {
                return Panel.ShowExpandIcon;
            }
            set
            {
                SetProperty("ShowExpandIcon", value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the close icon is visible
        /// </summary>
        public bool ShowCloseIcon
        {
            get
            {
                return Panel.ShowCloseIcon;
            }
            set
            {
                SetProperty("ShowCloseIcon", value);
            }
        }

        /// <summary>
        ///     Gets or sets the style of the panel.
        /// </summary>
        public PanelStyle PanelStyle
        {
            get
            {
                return Panel.PanelStyle;
            }
            set
            {
                SetProperty("PanelStyle", value);
            }
        }

        /// <summary>
        ///     Gets or sets the color schema which is used for the panel.
        /// </summary>
        public ColorScheme ColorScheme
        {
            get
            {
                return Panel.ColorScheme;
            }
            set
            {
                SetProperty("ColorScheme", value);
            }
        }

        #endregion

        #region MethodsPublic

        /// <summary>
        ///     Initializes a new instance of the PanelDesignerActionList class.
        /// </summary>
        /// <param name="component">A component related to the DesignerActionList.</param>
        public PanelDesignerActionList(IComponent component)
            : base(component)
        {
            // Automatically display smart tag panel when
            // design-time component is dropped onto the
            // Windows Forms Designer
            base.AutoShow = true;
        }

        /// <summary>
        ///     Returns the collection of DesignerActionItem objects contained in the list.
        /// </summary>
        /// <returns> A DesignerActionItem array that contains the items in this list.</returns>
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            // Create list to store designer action items
            var actionItems = new DesignerActionItemCollection();

            actionItems.Add(
                new DesignerActionMethodItem(
                    this,
                    "ToggleDockStyle",
                    GetDockStyleText(),
                    "Design",
                    "Dock or undock this control in it's parent container.",
                    true));

            actionItems.Add(
                new DesignerActionPropertyItem(
                    "ShowTransparentBackground",
                    "Show transparent backcolor",
                    GetCategory(Panel, "ShowTransparentBackground")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                    "ShowXPanderPanelProfessionalStyle",
                    "Show the XPanderPanels professional colorscheme",
                    GetCategory(Panel, "ShowXPanderPanelProfessionalStyle")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                    "ShowCaptionbar",
                    "Show the captionbar on top of the panel",
                    GetCategory(Panel, "ShowCaptionbar")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                    "ShowExpandIcon",
                    "Show the expand panel icon (not at DockStyle.None or DockStyle.Fill)",
                    GetCategory(Panel, "ShowExpandIcon")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                    "ShowCloseIcon",
                    "Show the close panel icon (not at DockStyle.None or DockStyle.Fill)",
                    GetCategory(Panel, "ShowCloseIcon")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                    "PanelStyle",
                    "Select PanelStyle",
                    GetCategory(Panel, "PanelStyle")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                    "ColorScheme",
                    "Select ColorScheme",
                    GetCategory(Panel, "ColorScheme")));

            return actionItems;
        }

        /// <summary>
        ///     Dock/Undock designer action method implementation
        /// </summary>
        public void ToggleDockStyle()
        {
            // Toggle ClockControl's Dock property
            if (Panel.Dock != DockStyle.Fill)
            {
                SetProperty("Dock", DockStyle.Fill);
            }
            else
            {
                SetProperty("Dock", DockStyle.None);
            }
        }

        #endregion

        #region MethodsPrivate

        // Helper method that returns an appropriate
        // display name for the Dock/Undock property,
        // based on the ClockControl's current Dock 
        // property value
        private string GetDockStyleText()
        {
            if (Panel.Dock == DockStyle.Fill)
            {
                return "Undock in parent container";
            }
            else
            {
                return "Dock in parent container";
            }
        }

        private Panel Panel
        {
            get
            {
                return (Panel)Component;
            }
        }

        // Helper method to safely set a component’s property
        private void SetProperty(string propertyName, object value)
        {
            // Get property
            var property
                = TypeDescriptor.GetProperties(Panel)[propertyName];
            // Set property value
            property.SetValue(Panel, value);
        }

        // Helper method to return the Category string from a
        // CategoryAttribute assigned to a property exposed by 
        //the specified object
        private static string GetCategory(object source, string propertyName)
        {
            var property = source.GetType().GetProperty(propertyName);
            var attribute =
                (CategoryAttribute)property.GetCustomAttributes(typeof(CategoryAttribute), false)[0];
            if (attribute == null)
                return null;
            return attribute.Category;
        }

        #endregion
    }

    #endregion
}
