using FileManager.API.Settings;

namespace FileManager.API.Contract.Common;

public class FileSizeValidator : AbstractValidator<IFormFile>
{
    public FileSizeValidator()
    {
        RuleFor(x => x)
            .Must((request, context) => request.Length <= FileSettings.FileSize.MaxInBytes)
            .WithMessage(FileSettings.FileSize.ErrorMessageInBytes)
            .When(e => e is not null);
    }
}
