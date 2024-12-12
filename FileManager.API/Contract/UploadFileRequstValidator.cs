namespace FileManager.API.Contract;

public class UploadFileRequstValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequstValidator()
    {
        //RuleFor(e => e.File)
        //    .SetValidator(new FileSizeValidator());

        RuleFor(e => e.File)
            .SetValidator(new BlockedSignaturesValidator());

        //TODO: Validate File Name
    }
}
 