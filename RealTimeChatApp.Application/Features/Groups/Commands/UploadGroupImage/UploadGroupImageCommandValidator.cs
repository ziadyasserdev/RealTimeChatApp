using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Features.Groups.Commands.UploadGroupImage
{
    public class UploadGroupImageCommandValidator : AbstractValidator<UploadGroupImageCommand>
    {
        private readonly string[] _allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        public UploadGroupImageCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Image is required.");

            RuleFor(x => x.Image)
                .Must(image => image != null && image.Length > 0)
                .WithMessage("Image cannot be empty.");

            RuleFor(x => x.Image)
                .Must(image =>
                {
                    if (image == null)
                        return false;

                    var extension = Path.GetExtension(image.FileName).ToLower();

                    return _allowedExtensions.Contains(extension);
                })
                .WithMessage("Only jpg, jpeg, png and webp images are allowed.");

            RuleFor(x => x.Image)
                .Must(image => image == null || image.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must not exceed 5 MB.");
        }
    }
}

