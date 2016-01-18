using System.Configuration;
using Microsoft.Win32;

namespace PDiffy.Infrastructure
{
    public static class Environment
    {
	    public static readonly string[] AllImageTypes = { OriginalId, ComparisonId, DifferenceId, LearnId };
        public const string OriginalId = "orig";
        public const string ComparisonId = "comp";
        public const string DifferenceId = "diff";
        public const string LearnId = "learn";

        public static string ImageStorePath
        {
            get
            {
                var imageStorePath = Get("pdiffyImageStorePath");
                return imageStorePath == null ? string.Empty : imageStorePath.ToString();
            }
            set { Set("pdiffyImageStorePath", value); }
        }

        public static string DataStorePath
        {
            get
            {
                var dataStorePath = Get("pdiffyDataStorePath");
                return dataStorePath == null ? string.Empty : dataStorePath.ToString();
            }
            set { Set("pdiffyDataStorePath", value); }
        }

        const string SubKeyPath = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";

        static object Get(string name)
        {
            object value = null;
            using (RegistryKey envKey = Registry.LocalMachine.OpenSubKey(SubKeyPath))
                if (envKey != null) value = envKey.GetValue(name);

            return value;
        }

        static void Set(string name, string value)
        {
            using (var envKey = Registry.LocalMachine.OpenSubKey(SubKeyPath, true))
                if (envKey != null) envKey.SetValue(name, value);
        }

		private static double _expiryDurationInHours;
		public static double ExpiryDurationInHours
		{
			get
			{
				if (_expiryDurationInHours > 0) return _expiryDurationInHours;

				double.TryParse(ConfigurationManager.AppSettings["ExpiryDurationInHours"], out _expiryDurationInHours);

				return (_expiryDurationInHours <= 0) ? _expiryDurationInHours = 72 : _expiryDurationInHours;
			}
		}
    }
}