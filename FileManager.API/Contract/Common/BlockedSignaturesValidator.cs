

namespace FileManager.API.Contract.Common;

public class BlockedSignaturesValidator : AbstractValidator<IFormFile>
{
    public BlockedSignaturesValidator()
    {
        RuleFor(x => x)
            .Must((request, context) =>
            {
                BinaryReader binary = new(request.OpenReadStream());
                var bytes = binary.ReadBytes(2);
                var fileSequenceHex = BitConverter.ToString(bytes);

                foreach (var signature in FileSettings.FileExtension.BlockedSignatures)
                    if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                        return false;

                return true;
            }).WithMessage(FileSettings.FileExtension.ContentErrorMessage)
            .When(e => e is not null);
    }
}
