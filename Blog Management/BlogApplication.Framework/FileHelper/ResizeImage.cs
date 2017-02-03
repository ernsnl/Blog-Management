using System;
using System.Drawing;
using System.IO;

namespace BlogApplication.Framework.FileHelper
{
    public static class ResizeImage
    {
        public static System.Drawing.Bitmap ResizeImageFromByte(byte[] data, int Width, int Height)
        {
            try
            {
                System.Drawing.Image BMP = System.Drawing.Bitmap.FromStream(new System.IO.MemoryStream(data));
                if (BMP.Width > Width)
                {
                    Height = Width * BMP.Height / BMP.Width;
                }
                else
                {
                    Width = Height * BMP.Width / BMP.Height;
                }
                System.Drawing.Bitmap NewBMP = new System.Drawing.Bitmap(BMP, Width, Height);
                return NewBMP;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static System.Drawing.Bitmap ResizeImageFromStream(System.IO.Stream InputStream, int Width, int Height)
        {
            try
            {
                System.Drawing.Image BMP = System.Drawing.Bitmap.FromStream(InputStream);
                if (BMP.Width > Width)
                {
                    Height = Width * BMP.Height / BMP.Width;
                }
                else
                {
                    Width = Height * BMP.Width / BMP.Height;
                }
                System.Drawing.Bitmap NewBMP = new System.Drawing.Bitmap(BMP, Width, Height);

                return NewBMP;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}