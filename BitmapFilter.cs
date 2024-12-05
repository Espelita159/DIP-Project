using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIP
{
    public static class BitmapFilter
    {
        public static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            if (m.Factor == 0) return false;

            Bitmap bSrc = (Bitmap)b.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;

            unsafe
            {
                byte* p = (byte*)bmData.Scan0;
                byte* pSrc = (byte*)bmSrc.Scan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        for (int colorOffset = 0; colorOffset < 3; colorOffset++)
                        {
                            int nPixel = 0;

                            nPixel += pSrc[colorOffset] * m.TopLeft;
                            nPixel += pSrc[3 + colorOffset] * m.TopMid;
                            nPixel += pSrc[6 + colorOffset] * m.TopRight;
                            nPixel += pSrc[stride + colorOffset] * m.MidLeft;
                            nPixel += pSrc[stride + 3 + colorOffset] * m.Pixel;
                            nPixel += pSrc[stride + 6 + colorOffset] * m.MidRight;
                            nPixel += pSrc[stride2 + colorOffset] * m.BottomLeft;
                            nPixel += pSrc[stride2 + 3 + colorOffset] * m.BottomMid;
                            nPixel += pSrc[stride2 + 6 + colorOffset] * m.BottomRight;

                            nPixel = nPixel / m.Factor + m.Offset;
                            nPixel = Math.Max(0, Math.Min(255, nPixel));

                            p[3 + stride + colorOffset] = (byte)nPixel;
                        }

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            return true;
        }

        public static bool GaussianBlur(Bitmap b)
        {
            ConvMatrix m = new ConvMatrix();

            m.TopLeft = 1; m.TopMid = 4; m.TopRight = 1;
            m.MidLeft = 4; m.Pixel = 8; m.MidRight = 4;
            m.BottomLeft = 1; m.BottomMid = 4; m.BottomRight = 1;

            m.Factor = 32;
            m.Offset = 0;

            return Conv3x3(b, m);
        }

        public static bool Smooth(Bitmap b, int nWeight = 1)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.Factor = m.TopLeft + m.TopMid + m.TopRight + m.MidLeft + m.Pixel + m.MidRight + m.BottomLeft + m.BottomMid + m.BottomRight;

            return Conv3x3(b, m);
        }

        public static bool Sharpen(Bitmap b)
        {
            ConvMatrix m = new ConvMatrix();

            m.TopLeft = 0; m.TopMid = -2; m.TopRight = 0;
            m.MidLeft = -2; m.Pixel = 11; m.MidRight = -2;
            m.BottomLeft = 0; m.BottomMid = -2; m.BottomRight = 0;

            m.Factor = 3;
            m.Offset = 0;

            return Conv3x3(b, m);
        }

        public static bool MeanRemoval(Bitmap b)
        {
            ConvMatrix m = new ConvMatrix();

            m.TopLeft = -1; m.TopMid = -1; m.TopRight = -1;
            m.MidLeft = -1; m.Pixel = 9; m.MidRight = -1;
            m.BottomLeft = -1; m.BottomMid = -1; m.BottomRight = -1;

            m.Factor = 1;
            m.Offset = 0;

            return Conv3x3(b, m);
        }

    }

}
