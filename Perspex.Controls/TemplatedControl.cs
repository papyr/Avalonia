﻿// -----------------------------------------------------------------------
// <copyright file="TemplatedControl.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Perspex.Media;
    using Perspex.Styling;
    using Splat;

    public class TemplatedControl : Control, ITemplatedControl
    {
        public static readonly PerspexProperty<ControlTemplate> TemplateProperty =
            PerspexProperty.Register<TemplatedControl, ControlTemplate>("Template");

        public ControlTemplate Template
        {
            get { return this.GetValue(TemplateProperty); }
            set { this.SetValue(TemplateProperty, value); }
        }

        public sealed override void Render(IDrawingContext context)
        {
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Control child = ((IVisual)this).VisualChildren.SingleOrDefault() as Control;

            if (child != null)
            {
                child.Arrange(new Rect(finalSize));
                return child.ActualSize;
            }
            else
            {
                return new Size();
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Control child = ((IVisual)this).VisualChildren.SingleOrDefault() as Control;

            if (child != null)
            {
                child.Measure(availableSize);
                return child.DesiredSize.Value;
            }

            return new Size();
        }

        protected override void CreateVisualChildren()
        {
            if (this.Template != null)
            {
                this.Log().Debug(string.Format(
                    "Creating template for {0} (#{1:x8})",
                    this.GetType().Name,
                    this.GetHashCode()));

                var child = this.Template.Build(this);
                this.AddVisualChild(child);
                this.OnTemplateApplied();
            }
        }

        protected virtual void OnTemplateApplied()
        {
        }
    }
}
