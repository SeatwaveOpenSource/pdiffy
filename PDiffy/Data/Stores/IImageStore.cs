namespace PDiffy.Data.Stores
{
	public interface IImageStore
	{
		string Save(System.Drawing.Image image, string name, string type);
		string[] Get(string name, string imageType);
		void DeleteAll();
		void Delete(string name, string[] imageTypes);
	}
}