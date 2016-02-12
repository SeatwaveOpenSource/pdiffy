using System.Collections.Generic;

namespace PDiffy.Data.Stores
{
	public interface IImageStore
	{
		string Save(System.Drawing.Image image, string name, string type);
		IEnumerable<string> Get(string name, string imageType);
		void DeleteAll();
		void Delete(string name, string[] imageTypes);
	}
}