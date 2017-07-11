using System;
using System.Linq;
using System.Text;
using SmartSuiteApp.Core.Extentions;
using SmartSuiteApp.Core.Models.Enums;
using Xamarin.Forms;

namespace SmartSuiteApp.Controls
{
    public class OrientationImage : Image
    {
        private DeviceOrientation currentImageOrientation;

        public static readonly BindableProperty PortraitProperty = BindableProperty.Create(
            nameof(Portrait),
            typeof(string),
            typeof(OrientationImage));

        public static readonly BindableProperty LandscapeProperty = BindableProperty.Create(
            nameof(Landscape),
            typeof(string),
            typeof(OrientationImage));
        public static readonly BindableProperty RootWidthProperty = BindableProperty.Create(
            nameof(RootWidth),
            typeof(int),
            typeof(OrientationImage),
            0);

        public static readonly BindableProperty RootHeightProperty = BindableProperty.Create(
            nameof(RootHeight),
            typeof(int),
            typeof(OrientationImage),
            0);

        public string Portrait
        {
            get => (string)GetValue(PortraitProperty);
            set => SetValue(PortraitProperty, value);
        }

        public string Landscape
        {
            get => (string)GetValue(LandscapeProperty);
            set => SetValue(LandscapeProperty, value);
        }

        public int RootWidth
        {
            get => (int)GetValue(RootWidthProperty);
            set => SetValue(RootWidthProperty, value);
        }

        public int RootHeight
        {
            get => (int)GetValue(RootHeightProperty);
            set => SetValue(RootHeightProperty, value);
        }

        public OrientationImage()
        {
            currentImageOrientation = DeviceOrientation.None;
            Aspect = Aspect.AspectFill;
        }
        
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if ((RootWidth <= 0 || RootHeight <= 0) && (Width < 0 || Height < 0))
            {
                return;
            }

            var biggesWidth = RootWidth > Width ? RootWidth : width;
            var biggesHeight = RootHeight > Height ? RootHeight : Height;

            var orientation = biggesWidth > biggesHeight ? DeviceOrientation.Landscape : DeviceOrientation.Portait;

            if (orientation == DeviceOrientation.Portait && currentImageOrientation != orientation)
            {
                Source = ImageSource.FromFile(ToImageSourceFormat(Portrait));
            }
            else if (orientation == DeviceOrientation.Landscape && currentImageOrientation != orientation)
            {
                Source = ImageSource.FromFile(ToImageSourceFormat(Landscape));
            }

            currentImageOrientation = orientation;
        }

        private static string ToImageSourceFormat(string imageName)
        {
            var formatedImageName = imageName;

            var splitedImageString = imageName.Split('.');
            if (splitedImageString.Length > 1)
            {
                var format = splitedImageString.Last();
                if (format != null)
                {
                    format = format.UppercaseFirst();
                    var imageFormat = Enum.Parse(typeof(ImageFormat), format);
                    if (imageFormat != null)
                    {
                        var stringBuilder = new StringBuilder();
                        for (var i = 0; i < splitedImageString.Length - 1; i++)
                        {
                            stringBuilder.Append(splitedImageString[i]);
                        }
                        formatedImageName = stringBuilder.ToString().GetPathByName((ImageFormat)imageFormat);
                    }
                    else
                    {
                        formatedImageName = imageName.GetPathByName();
                    }
                }
            }
            else
            {
                formatedImageName = imageName.GetPathByName();
            }

            return formatedImageName;
        }
    }
}
