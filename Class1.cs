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
        string genean13(string text, int width, int height);
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
        public static Bitmap Generate2(string text, int width, int height)
        {
            BarcodeWriter writer = new BarcodeWriter();
            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            //writer.Format = BarcodeFormat.ITF;
            writer.Format = BarcodeFormat.CODE_39;
            EncodingOptions options = new EncodingOptions()
            {
                Width = width,
                Height = height,
                Margin = 2
            };
            writer.Options = options;
            Bitmap map = writer.Write(text);
            return map;
        }

        public string genean13(string text, int width, int height)
        {
            //m_log.Info("开始生成图片");
            Bitmap map = Generate2(text, width, height);
            m_log.Info("生成一维码完成:"+text+ ",width=" + width+ ",height=" + height);
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

