namespace FileManager.API.Settings;

public static class FileSettings
{
    public partial class FileSize
    {
        public const int MaxInMB = 1;
        public const int MaxInBytes = MaxInMB * 1024 * 1024;
        public static readonly string ErrorMessageInMB = $"Max File size is {MaxInMB} MB.";
        public static readonly string ErrorMessageInBytes = $"Max File size is {MaxInBytes} Bytes.";
    }
    public partial class FileExtension
    {
        // White Listing

        // Black Listing
        public static readonly string[] BlockedSignatures = ["4D-5A", "2F-2A", "D0-CF"];
        public const string ContentErrorMessage = "Not allowed file content";
    }
    public partial class Image
    {
        public static readonly string[] AllowedExtension = [".jpg", ".jpeg", ".png"];
        public const string ErrorMessage = "File Extension Is Not Allowed";
    }
}
