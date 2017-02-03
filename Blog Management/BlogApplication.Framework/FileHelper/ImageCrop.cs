using System;

namespace BlogApplication.Framework.FileHelper
{
    public static class ImageCrop
    {
        public static System.Drawing.Bitmap CropImage(byte[] data, int Width, int Height)
        {
            return CropImage(System.Drawing.Bitmap.FromStream(new System.IO.MemoryStream(data)), Width, Height);
        }
        public static System.Drawing.Bitmap CropImage(System.Drawing.Image Image, int Width, int Height)
        {
            if ((Image != null))
            {
                if (Image.Width > Width || Image.Height > Height)
                {
                    decimal Ratio = Convert.ToDecimal(Image.Width) / Convert.ToDecimal(Image.Height);
                    int SizedWidth = Image.Width;
                    int SizedHeight = Image.Height;
                    if (Ratio > 1)
                    {
                        if (Height > Image.Height)
                            Height = Image.Height;
                        SizedHeight = Height;
                        SizedWidth = Convert.ToInt32(SizedHeight * Ratio);
                    }
                    else
                    {
                        if (Width > Image.Width)
                            Width = Image.Width;
                        SizedWidth = Width;
                        SizedHeight = Convert.ToInt32(SizedWidth / Ratio);
                    }

                    System.Drawing.Bitmap NewImage = null;
                    if (Image.PixelFormat.ToString().Contains("Indexed"))
                    {
                        NewImage = new System.Drawing.Bitmap(Convert.ToInt32(SizedWidth), Convert.ToInt32(SizedHeight));
                    }
                    else
                    {
                        NewImage = new System.Drawing.Bitmap(SizedWidth, SizedHeight, Image.PixelFormat);
                    }
                    System.Drawing.Graphics Graph = System.Drawing.Graphics.FromImage(NewImage);
                    Graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    Graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    Graph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    Graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    System.Drawing.Rectangle Rect = new System.Drawing.Rectangle(0, 0, SizedWidth, SizedHeight);
                    Graph.DrawImage(Image, Rect);

                    System.Drawing.Rectangle CroppedRect = new System.Drawing.Rectangle((NewImage.Width - Width) / 2, (NewImage.Height - Height) / 2, Width, Height);
                    System.Drawing.Bitmap BMP = NewImage.Clone(CroppedRect, NewImage.PixelFormat);
                    return BMP;
                }
                else
                {
                    System.Drawing.Bitmap NewImage = null;
                    if (Image.PixelFormat.ToString().Contains("Indexed"))
                    {
                        NewImage = new System.Drawing.Bitmap(Convert.ToInt32(Image.Width), Convert.ToInt32(Image.Height));
                    }
                    else
                    {
                        NewImage = new System.Drawing.Bitmap(Image.Width, Image.Height, Image.PixelFormat);
                    }
                    System.Drawing.Graphics Graph = System.Drawing.Graphics.FromImage(NewImage);
                    Graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    Graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    Graph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    Graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    System.Drawing.Rectangle Rect = new System.Drawing.Rectangle(0, 0, Image.Width, Image.Height);
                    Graph.DrawImage(Image, Rect);
                    return NewImage;
                }
            }
            return null;
        }
    }
}