using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace eastbarcode
{
    [Guid("467E3E4C-4E88-4EF2-BE71-85F137D581AB")]
    public interface eastbarcode_Interface
    {
        [DispId(1)]
        string genbar1d(string text, int width, int height,string tfmt);
    }
    [Guid("2CEC4DE9-0E54-4AF1-938F-F0B61E7702ED"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface eastbarcode_Events
    {
    }
    [Guid("ABC62098-C203-4B9D-8711-AE9091A1C2B4"), ComSourceInterfaces(typeof(eastbarcode_Events))]
    public class Class1 : eastbarcode_Interface
    {
        public static ILog m_log = LogManager.GetLogger(typeof(Class1)); //LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 生成一维条形码
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static Bitmap Generate2(string text, int width, int height, string tfmt)
        {
            BarcodeWriter writer = new BarcodeWriter();
            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            //writer.Format = BarcodeFormat.ITF;
            if (string.IsNullOrEmpty(tfmt))
            {
                tfmt = "CODE_128";
            }
            switch (tfmt)
            {
                case "CODE_39":
                    writer.Format = BarcodeFormat.CODE_128;
                    break;
                case "CODE_93":
                    writer.Format = BarcodeFormat.CODE_93;
                    break;
                case "CODE_128":
                    writer.Format = BarcodeFormat.CODE_128;
                    break;
                case "CODABAR":
                    writer.Format = BarcodeFormat.CODABAR;
                    break;
                case "ITF":
                    writer.Format = BarcodeFormat.ITF;
                    break;
                default:
                    writer.Format = BarcodeFormat.CODE_128;
                    break;
            }
            EncodingOptions options = new EncodingOptions()
            {
                Width = width,
                Height = height,
                Margin = 1
            };
            writer.Options = options;
            Bitmap map = writer.Write(text);
            return map;
        }

        public string genbar1d(string text, int width, int height, string tfmt)
        {
            m_log.Info("开始生成一维码:"+ text);
            Bitmap map = Generate2(text, width, height, tfmt);
            m_log.Info("生成一维码完成:"+text+ ",宽度width=" + width+ ",高度height=" + height+",条码格式="+ tfmt);
            //将img转换成bmp格式，否则后面无法创建Graphics对象
            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(map, 0, 0);
            }
            string tpath = "c:\\eastwillpay\\bar\\pic\\" + text + ".jpg";
            bmpimg.Save(tpath, ImageFormat.Jpeg);
            return "0";
        }
    }
}

