using System.Threading.Tasks;
using MediatR;

namespace PDiffy.Features.TextDifferences
{
	public class DeleteAll
	{
		public class Command : IAsyncRequest
		{
			public bool DeleteAll { get; set; }
		}

		public class CommandHandler : AsyncRequestHandler<Command>
		{
			protected override async Task HandleCore(Command message)
			{
				if (message.DeleteAll)
					Data.Biggy.TextComparisons.Clear();
			}
		}
	}
}