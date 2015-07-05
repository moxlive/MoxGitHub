using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using PhotoOrganizer.BusinessModule.Photos;

namespace PhotoOrganizer.BusinessModule
{
    public class PhotoModifier
    {
        public Bitmap CombinePicture(PhotoGroup group)
        {
            Photo front = group.Front;
            Photo back = group.Back;

            Image fi = Image.FromFile(front.FullPath);
            Image bi = Image.FromFile(back.FullPath);

            Bitmap newPic = new Bitmap(fi.Width + bi.Width, Math.Max(fi.Height, bi.Height));
            using (Graphics g = Graphics.FromImage(newPic))
            {
                g.DrawImage(fi, 0, 0);
                g.DrawImage(bi, fi.Width, 0);
            }

            return newPic;

        }
    }
}
