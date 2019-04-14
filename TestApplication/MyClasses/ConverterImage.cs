using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TestApplication.MyClasses
{
	public class ConverterImage
	{
		//ConvertImage to byteArray
		public byte[] ConvertImageToByteArray(Image x)
		{
			ImageConverter _imageConverter = new ImageConverter();
			byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
			return xByte;
		}

		//ConvertImage to byteArray 
		public byte[] ConvertImageToByteArray(string path)
		{
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			byte[] myByte = new byte[fs.Length];
			fs.Read(myByte, 0, Convert.ToInt32(fs.Length));
			return myByte;
		}

		//Convert byte to Image
		public Image ConvertToImage(byte[] bytes)
		{
			MemoryStream ms = new MemoryStream(bytes);
			Image returnImage = Image.FromStream(ms);
			return returnImage;

		}
		//Save Image
		public void SaveImage(string path,System.Drawing.Imaging.ImageFormat format,Image img)
		{
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				img.Save(fs, format);
				fs.Close();
			}
		}




	}
}
