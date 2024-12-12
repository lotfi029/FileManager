namespace FileManager.API.Contract;

public class UploadManyFilesRequstValidator : AbstractValidator<UploadManyFilesRequest>
{
    public UploadManyFilesRequstValidator()
    {
        RuleForEach(e => e.Files)
            .SetValidator(new FileSizeValidator());

        RuleForEach(e => e.Files)
            .SetValidator(new BlockedSignaturesValidator());

        //TODO: Validate File Name
    }
}
 