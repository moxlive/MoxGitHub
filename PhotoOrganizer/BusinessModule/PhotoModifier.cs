using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using PhotoOrganizer.BusinessModule.Photos;
using PhotoOrganizer.BusinessModule.Common;

namespace PhotoOrganizer.BusinessModule
{
    public class PhotoModifier
    {
        Settings settings;

        public PhotoModifier(Settings settings)
        {
            this.settings = settings;
        }

        public Bitmap CombinePicture(PhotoGroup group)
        {
            Photo front = group.Front;
            Photo back = group.Back;

            return CombinePicture(front.FullPath, back.FullPath);
        }

        public Bitmap CombinePicture(string frontPicPath, string backPicPath)
        {
            Image fi = Image.FromFile(frontPicPath);
            fi.RotateFlip(GetRotateType(settings.FrontPictureRotate));

            Image bi = Image.FromFile(backPicPath);
            bi.RotateFlip(GetRotateType(settings.BackPictureRotate));

            Bitmap newPic;

            try
            { 
                newPic = new Bitmap(fi.Width + bi.Width, Math.Max(fi.Height, bi.Height));
            }
            catch (ArgumentException ex)
            {
                //for some reason even BitMap is placed in Using(), process still runs out of memory(1G).
                //try to force garbage collection fixes this issue
                //http://stackoverflow.com/questions/748777/run-gc-collect-synchronously
                GC.Collect();
                GC.WaitForPendingFinalizers();
                newPic = new Bitmap(fi.Width + bi.Width, Math.Max(fi.Height, bi.Height));
            }

            using (Graphics g = Graphics.FromImage(newPic))
            {
                //todo, configurable
                g.InterpolationMode = InterpolationMode.Default;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.DrawImage(fi, 0, 0, fi.Width, fi.Height);
                g.DrawImage(bi, fi.Width, 0, bi.Width, bi.Height);
            }

            return newPic;

        }

        private static RotateFlipType GetRotateType(string degree)
        {
            switch (degree)
            {
                case "90":
                    return RotateFlipType.Rotate90FlipNone;
                case "180":
                    return RotateFlipType.Rotate180FlipNone;
                case "270":
                    return RotateFlipType.Rotate270FlipNone;
                default: 
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }
    }
}
