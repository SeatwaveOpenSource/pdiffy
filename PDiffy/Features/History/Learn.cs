using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PDiffy.Data;
using PDiffy.Data.Stores;
using Environment = PDiffy.Infrastructure.Environment;

namespace PDiffy.Features.History
{
	public class Learn
	{
		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(x => x.Name).NotEmpty();
				RuleFor(x => x.CreatedDate).NotNull();
			}
		}

		public class Command : IAsyncRequest
		{
			public string Name { get; set; }
			public DateTime CreatedDate { get; set; }
		}

		public class CommandHandler : AsyncRequestHandler<Command>
		{
			readonly IImageStore _imageStore;

			public CommandHandler(IImageStore imageStore)
			{
				_imageStore = imageStore;
			}

			protected override async Task HandleCore(Command message)
			{
				var differenceImage = 
					_imageStore.Get(message.Name, Environment.DifferenceId)
					.Select(x => new HistoricalImage(x)).Single(x => x.CreatedDate == message.CreatedDate);

				Data.Biggy.KnownImageList.Add(new KnownImage
				{
					Name = message.Name,
					CreatedDate = message.CreatedDate,
					ImagePath = _imageStore.Save(new Bitmap(differenceImage.ImagePath), message.Name, Environment.LearnId)
				});
			}
		}
	}
}