using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using PhotoOrganizer.BusinessModule.Photos;

namespace PhotoOrganizer.BusinessModule
{
    public class PhotoModifier
    {
        public Bitmap CombinePicture(PhotoGroup group)
        {
            Photo front = group.Front;
            Photo back = group.Back;

            //todo check this out, configurable
            Image fi = Image.FromFile(front.FullPath);
            fi.RotateFlip(RotateFlipType.Rotate90FlipNone);

            //todo check this out, configurable
            Image bi = Image.FromFile(back.FullPath);
            bi.RotateFlip(RotateFlipType.Rotate270FlipNone);

            Bitmap newPic = new Bitmap(fi.Width + bi.Width, Math.Max(fi.Height, bi.Height));
            using (Graphics g = Graphics.FromImage(newPic))
            {
                //todo, configurable
                g.InterpolationMode = InterpolationMode.Default;
                g.CompositingQuality = CompositingQuality.HighSpeed; 
                g.DrawImage(fi, 0, 0,fi.Width, fi.Height);
                g.DrawImage(bi, fi.Width, 0, bi.Width, bi.Height);
            }

            return newPic;

        }
    }
}
