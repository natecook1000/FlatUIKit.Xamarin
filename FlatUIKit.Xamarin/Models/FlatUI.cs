using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace FlatUIKit
{
    public static partial class FlatUI
    {
        public static void Apply()
        {
            // bar button items
            SetBarButtonItemAppearance(UIBarButtonItem.Appearance, FlatUI.Color.PeterRiver, FlatUI.Color.BelizeHole, UIColor.White, 3f);

            //SetFlatNavigationBarAppearance(UINavigationBar.Appearance, FlatUI.Color.MidnightBlue);
            SetFlatNavigationBarAppearance(UINavigationBar.Appearance, FlatUI.Color.Carrot, FlatUI.Color.Clouds);
        }

        const string FontName = @"HelveticaNeue-Medium";
        const string BoldFontName = @"HelveticaNeue-Bold";

        public static UIFont FontOfSize(float size)
        {
            return UIFont.FromName(FontName, size);
        }

        public static UIFont BoldFontOfSize(float size)
        {
            return UIFont.FromName(BoldFontName, size);
        }

        public static void SetFlatNavigationBarAppearance(UINavigationBar.UINavigationBarAppearance appearance, UIColor color, UIColor textColor)
        {
            appearance.SetBackgroundImage(FlatUI.Image(color, 0), UIBarMetrics.Default & UIBarMetrics.LandscapePhone);
            UITextAttributes titleTextAttributes = appearance.GetTitleTextAttributes();
            if (titleTextAttributes == null)
                titleTextAttributes = new UITextAttributes();
            titleTextAttributes.TextShadowColor = UIColor.Clear;
            titleTextAttributes.TextShadowOffset = new UIOffset(0, 0);
            titleTextAttributes.TextColor = textColor;
            titleTextAttributes.Font = FlatUI.BoldFontOfSize(0);
            appearance.SetTitleTextAttributes(titleTextAttributes);
            if (appearance.RespondsToSelector(new MonoTouch.ObjCRuntime.Selector("setShadowImage:")))
                appearance.ShadowImage = FlatUI.Image(UIColor.Clear, 0);
        }

        public static void SetBarButtonItemAppearance(UIBarButtonItem.UIBarButtonItemAppearance appearance, UIColor color, UIColor highlightedColor, UIColor textColor, float cornerRadius)
        {
            UIImage backButtonPortraitImage = FlatUI.BackButtonImage(color,
                                                                     UIBarMetrics.Default,
                                                                     cornerRadius,
                                                                     color.Darken(2),
                                                                     3f);
            UIImage highlightedBackButtonPortraitImage = FlatUI.BackButtonImage(highlightedColor,
                                                                                UIBarMetrics.Default,
                                                                                cornerRadius,
                                                                                highlightedColor.Darken(2),
                                                                                3f);
            UIImage backButtonLandscapeImage = FlatUI.BackButtonImage(color,
                                                                      UIBarMetrics.LandscapePhone,
                                                                      2,
                                                                      color.Darken(2),
                                                                      3f);
            UIImage highlightedBackButtonLandscapeImage = FlatUI.BackButtonImage(highlightedColor,
                                                                                 UIBarMetrics.LandscapePhone,
                                                                                 2,
                                                                                 highlightedColor.Darken(2),
                                                                                 3f);

            appearance.SetBackButtonBackgroundImage(backButtonPortraitImage, UIControlState.Normal, UIBarMetrics.Default);
            appearance.SetBackButtonBackgroundImage(backButtonLandscapeImage, UIControlState.Normal, UIBarMetrics.LandscapePhone);
            appearance.SetBackButtonBackgroundImage(highlightedBackButtonPortraitImage, UIControlState.Highlighted, UIBarMetrics.Default);
            appearance.SetBackButtonBackgroundImage(highlightedBackButtonLandscapeImage, UIControlState.Highlighted, UIBarMetrics.LandscapePhone);

            appearance.SetBackButtonTitlePositionAdjustment(new UIOffset(1f, 1f), UIBarMetrics.Default);
            appearance.SetBackButtonTitlePositionAdjustment(new UIOffset(1f, 1f), UIBarMetrics.LandscapePhone);

            UIImage buttonImageNormal = FlatUI.Image(color, cornerRadius, color.Darken(2), 3f);
            //UIImage buttonImageNormal = FlatUI.Image(color, cornerRadius);
            //UIImage buttonImageHighlighted = FlatUI.Image(highlightedColor, cornerRadius);
            UIImage buttonImageHighlighted = FlatUI.Image(highlightedColor, cornerRadius, color.Darken(2), 3f);

            appearance.SetBackgroundImage(buttonImageNormal, UIControlState.Normal, UIBarMetrics.Default);
            appearance.SetBackgroundImage(buttonImageHighlighted, UIControlState.Highlighted, UIBarMetrics.Default);

            UITextAttributes titleTextAttributes = appearance.GetTitleTextAttributes(UIControlState.Normal);
            if (titleTextAttributes == null)
                titleTextAttributes = new UITextAttributes();
            titleTextAttributes.TextShadowColor = UIColor.Clear;
            titleTextAttributes.TextShadowOffset = new UIOffset(0, 0);
            titleTextAttributes.TextColor = textColor;
            titleTextAttributes.Font = FlatUI.FontOfSize(0);
            appearance.SetTitleTextAttributes(titleTextAttributes, UIControlState.Normal);
            appearance.SetTitleTextAttributes(titleTextAttributes, UIControlState.Highlighted);
        }

        public static UIImage ButtonImage(UIColor color, float cornerRadius, UIColor shadowColor, UIEdgeInsets shadowInsets)
        {
            UIImage topImage = Image(color, cornerRadius);
            UIImage bottomImage = Image(shadowColor, cornerRadius);
            float totalHeight = EdgeSize(cornerRadius) + shadowInsets.Top + shadowInsets.Bottom;
            float totalWidth = EdgeSize(cornerRadius) + shadowInsets.Left + shadowInsets.Right;
            float topWidth = EdgeSize(cornerRadius);
            float topHeight = EdgeSize(cornerRadius);
            RectangleF topRect = new RectangleF(shadowInsets.Left, shadowInsets.Top, topWidth, topHeight);
            RectangleF bottomRect = new RectangleF(0, 0, totalWidth, totalHeight);

            UIGraphics.BeginImageContextWithOptions(new SizeF(totalWidth, totalHeight), false, 0f);
            if (!bottomRect.Equals(topRect))
                bottomImage.Draw(bottomRect);
            topImage.Draw(topRect);
            UIImage buttonImage = UIGraphics.GetImageFromCurrentImageContext();
            UIEdgeInsets resizeableInsets = new UIEdgeInsets(cornerRadius + shadowInsets.Top,
                                                             cornerRadius + shadowInsets.Left,
                                                             cornerRadius + shadowInsets.Bottom,
                                                             cornerRadius + shadowInsets.Right);
            UIGraphics.EndImageContext();
            return buttonImage.CreateResizableImage(resizeableInsets);
        }

        public static float EdgeSize(float cornerRadius)
        {
            return cornerRadius * 2 + 1;
        }

        public static UIImage Image(UIColor color, float cornerRadius)
        {
            /*
            float minEdgeSize = EdgeSize(cornerRadius);
            RectangleF rect = new RectangleF(0, 0, minEdgeSize, minEdgeSize);
            UIBezierPath roundedRect = UIBezierPath.FromRoundedRect(rect, cornerRadius);
            roundedRect.LineWidth = 0;
            UIGraphics.BeginImageContextWithOptions(rect.Size, false, 0f);
            color.SetFill();
            roundedRect.Fill();
            roundedRect.Stroke();
            roundedRect.AddClip();
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image.CreateResizableImage(new UIEdgeInsets(cornerRadius, cornerRadius, cornerRadius, cornerRadius), UIImageResizingMode.Stretch);
            */
            return FlatUI.Image(color, cornerRadius, color, 0);
        }

        public static UIImage Image(UIColor color, float cornerRadius, UIColor borderColor, float borderWidth)
        {
            float minEdgeSize = EdgeSize(cornerRadius);
            RectangleF rect = new RectangleF(0, 0, minEdgeSize, minEdgeSize);
            UIBezierPath path = UIBezierPath.FromRoundedRect(rect, cornerRadius);
            path.LineWidth = borderWidth * -1;
            UIGraphics.BeginImageContextWithOptions(rect.Size, false, 0f);
            color.SetFill();
            borderColor.SetStroke();
            path.Fill();
            path.Stroke();
            path.AddClip();
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image.CreateResizableImage(new UIEdgeInsets(cornerRadius, cornerRadius, cornerRadius, cornerRadius), UIImageResizingMode.Stretch);
        }

        public static UIImage BackButtonImage(UIColor color, UIBarMetrics metrics, float cornerRadius)
        {
            return FlatUI.BackButtonImage(color, metrics, cornerRadius, color, 0);
        }

        public static UIImage BackButtonImage(UIColor color, UIBarMetrics metrics, float cornerRadius, UIColor borderColor, float borderWidth)
        {
            SizeF size;
            if (metrics.Equals(UIBarMetrics.Default))
                size = new SizeF(50, 30);
            else
                size = new SizeF(60, 23);

            UIBezierPath path = BezierPathForBackButton(new RectangleF(0, 0, size.Width, size.Height), cornerRadius);

            path.LineWidth = borderWidth * -1;

            UIGraphics.BeginImageContextWithOptions(size, false, 0f);
            color.SetFill();
            borderColor.SetStroke();
            path.AddClip();
            path.Fill();
            path.Stroke();
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image.CreateResizableImage(new UIEdgeInsets(cornerRadius, 15f, cornerRadius, cornerRadius), UIImageResizingMode.Stretch);
        }

        public static UIBezierPath BezierPathForBackButton(RectangleF rect, float radius)
        {
            var path = new UIBezierPath();
            var mPoint = new PointF(rect.Right - radius, rect.Y);
            var ctrlPoint = mPoint;
            path.MoveTo(mPoint);

            ctrlPoint.Y += radius;
            mPoint.X += radius;
            mPoint.Y += radius;
            if (radius > 0)
                path.AddArc(ctrlPoint, radius, (float)(Math.PI + Math.PI / 2), 0, true);

            mPoint.Y = rect.Bottom - radius;
            path.AddLineTo(mPoint);

            ctrlPoint = mPoint;
            mPoint.Y += radius;
            mPoint.X -= radius;
            ctrlPoint.X -= radius;
            if (radius > 0)
                path.AddArc(ctrlPoint, radius, 0, (float)(Math.PI / 2), true);

            mPoint.X = rect.X + 10f;
            path.AddLineTo(mPoint);

            path.AddLineTo(new PointF(rect.X, rect.Height / 2));

            mPoint.Y = rect.Y;
            path.AddLineTo(mPoint);

            path.ClosePath();
            return path;
        }
    }
}

