using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Environment = PDiffy.Infrastructure.Environment;

namespace PDiffy.Data
{

	public static class PDiffyFileProperties
	{
		public static string DateFormat { get { return "yyyyMMdd-HHmmss.ffff"; } }

		///<remarks>
		// An example of the filename, as of now without the quotes "div.utility-nav..orig.20151218-170255.6254.png"
		///</remarks>
		public static string Name(this string fileName)
		{
			return fileName.Split(Environment.AllImageTypes, StringSplitOptions.None)[0].TrimEnd('.');
		}

		public static string Type(this string fileName)
		{
			return
				fileName.Contains(Environment.OriginalId) ? Environment.OriginalId
				: fileName.Contains(Environment.ComparisonId) ? Environment.ComparisonId
				: fileName.Contains(Environment.DifferenceId) ? Environment.DifferenceId
				: string.Empty;
		}

		public static DateTime Date(this string fileName)
		{
			var matches = Regex.Matches(fileName, @"\d{8}-\d{6}.\d{4}");

			if (matches.Count == 0)
				throw new ArgumentException("Could not parse the date part of the fileName", "fileName");

			return DateTime.ParseExact(matches[0].ToString(), DateFormat, CultureInfo.InvariantCulture);
		}
	}
}