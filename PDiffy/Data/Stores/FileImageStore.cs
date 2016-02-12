using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Quarks;
using Quarks.IEnumerableExtensions;
using Environment = PDiffy.Infrastructure.Environment;

namespace PDiffy.Data.Stores
{
	public class FileImageStore : IImageStore
	{
		public string Save(System.Drawing.Image image, string name, string type)
		{
			using (image)
			{
				name = ConvertStringToHex(name);
				var fullPath = Path.Combine(Environment.ImageStorePath, string.Join(".", name, type, SystemTime.Now.ToString(PDiffyFileProperties.DateFormat), "png"));
				var folder = Path.GetDirectoryName(fullPath);

				if (folder != null && !Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				image.Save(fullPath);

				return fullPath;
			}
		}

		public void Delete(string name, string[] imageTypes)
		{
			name = ConvertStringToHex(name);
			imageTypes.SelectMany(imageType => Get(name, imageType)).ForEach(File.Delete);
		}

		public void DeleteAll()
		{
			Directory.Delete(Environment.ImageStorePath, true);
		}

		public IEnumerable<string> Get(string name, string imageType)
		{
			//Note: Example filename w\ quotes: "2e70726f6d6f2e6d61747269782e74776f6c696e65733e68322e64696666.20160107-145147.4123.png"
			var matchFunction = new Func<string, bool>(i => Regex.IsMatch(i, @"\\[a-zA-Z0-9]*.\d{8}-\d{6}.\d{4}.png", RegexOptions.Compiled | RegexOptions.IgnoreCase));

			var files = Directory.GetFiles(Environment.ImageStorePath);
			return files
				.Where(matchFunction)
				.Where(path =>
					ConvertHexToString(Path.GetFileName(path)).Name() == name &&
					ConvertHexToString(Path.GetFileName(path)).Type() == imageType).ToList();
		}

		public static string ConvertStringToHex(string asciiString)
		{
			var result = string.Empty;
			foreach (var c in asciiString)
				result += String.Format("{0:x2}", Convert.ToUInt32(((int) c).ToString()));

			return result;
		}

		public static string ConvertHexToString(string hexValue)
		{
			try
			{
				var value = string.Empty;
				while (hexValue.Length > 0)
				{
					value += Convert.ToChar(Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
					hexValue = hexValue.Substring(2, hexValue.Length - 2);
				}
				return value;
			}
			catch
			{
				//failing conversion can only mean a wrong file exists in store
				return "some_nonhexa_badly_named_file_eg_thumbs.db";
			}

		}
	}
}