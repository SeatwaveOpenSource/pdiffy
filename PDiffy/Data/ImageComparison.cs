using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using PDiffy.Features.Shared.Libraries;
using Quarks;

namespace PDiffy.Data
{
	public class ImageComparison
	{
		[Key]
		public string Name { get; set; }
		[Key]
		public string Page { get; set; }
		[Key]
		public string Site { get; set; }

		public bool ComparisonStillValid { get { return LastComparisonDate != null && LastComparisonDate > SystemTime.Now.AddHours(Infrastructure.Environment.ExpiryDurationInHours); } }
		public DateTime? LastComparisonDate { get; set; }
		public bool HumanComparisonRequired { get; set; }

		[JsonIgnore]
		public Bitmap OriginalImage
		{
			get
			{
				return string.IsNullOrEmpty(OriginalImageUrl)
					? new Bitmap(OriginalImagePath)
					: capture(OriginalImageUrl);
			}
		}

		public string OriginalImageUrl { get; set; }
		public string OriginalImagePath { get; set; }

		[JsonIgnore]
		public Bitmap ComparisonImage
		{
			get
			{
				return string.IsNullOrEmpty(ComparisonImageUrl)
					? new Bitmap(ComparisonImagePath)
					: capture(ComparisonImageUrl);
			}
		}

		public string ComparisonImageUrl { get; set; }
		public string ComparisonImagePath { get; set; }

		public string DifferenceImagePath { get; set; }

		[JsonIgnore]
		public Bitmap DifferenceImage
		{
			get
			{
				return string.IsNullOrWhiteSpace(DifferenceImagePath)
					? new ImageDiffTool().CreateDifferenceImage(OriginalImage, ComparisonImage) //*
					: new Bitmap(DifferenceImagePath);
			}
			//* issue created on github to make the images same size before passing into create diff image
		}

		static Bitmap capture(string imageUrl)
		{
			using (var wc = new WebClient())
			using (var ms = new MemoryStream(wc.DownloadData(imageUrl)))
				return new Bitmap(ms);
		}
	}
}