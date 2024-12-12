namespace FileManager.API.Contract;

public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
    public UploadImageRequestValidator()
    {
        RuleFor(e => e.Image)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator());

        RuleFor(e => e.Image)
            .Must((request, context) =>
            {
                var extension = Path.GetExtension(request.Image.FileName.ToLower());
                return FileSettings.Image.AllowedExtension.Contains(extension);
            })
            .WithMessage(FileSettings.Image.ErrorMessage)
            .When(e => e.Image is not null);
    }
}