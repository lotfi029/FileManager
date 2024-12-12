using Microsoft.AspNetCore.Http.Metadata;

namespace FileManager.API.Services;

public class FileService(
    IWebHostEnvironment webHostEnvironment,
    ApplicationDbContext context) : IFileService
{
    private readonly string _filesPath = $"{webHostEnvironment.WebRootPath}/uploads";
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";
    private readonly ApplicationDbContext _context = context;

    public async Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var uploadFile = await SaveFilesAsync(file, cancellationToken);

        await _context.AddAsync(uploadFile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return uploadFile.Id;
    }
    public async Task<IEnumerable<Guid>> UploadManyAsync(UploadManyFilesRequest files, CancellationToken cancellationToken = default)
    {
        List<UploadedFile> uploadedFiles = [];
        foreach (var file in files.Files) 
            uploadedFiles.Add(await  SaveFilesAsync(file, cancellationToken));

        await _context.AddRangeAsync(uploadedFiles, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return uploadedFiles.Select(e => e.Id);
    }
    public async Task UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_imagesPath, image.FileName);

        using var stream = File.Create(path);
        await image.CopyToAsync(stream, cancellationToken);
    }
    public async Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.Files.FindAsync(id, cancellationToken);

        if (file == null)
            return ([], string.Empty, string.Empty);

        var path = Path.Combine(_filesPath, file.StoredFileName);
        MemoryStream memoryStream = new();
        using FileStream fileStream = new(path, FileMode.Open);
        fileStream.CopyTo(memoryStream);

        memoryStream.Position = 0;

        return (memoryStream.ToArray(), file.ContentType, file.FileName);
    }
    public async Task<(FileStream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.Files.FindAsync(id, cancellationToken);

        if (file == null)
            return (null, string.Empty, string.Empty);

        var path = Path.Combine(_filesPath, file.StoredFileName);
        
        var fileStream = File.OpenRead(path);

        return (fileStream, file.ContentType, file.FileName);
    }
    private async Task<UploadedFile> SaveFilesAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var randomName = Path.GetRandomFileName();

        var uploadFile = new UploadedFile
        {
            FileName = file.FileName,
            StoredFileName = randomName,
            ContentType = file.ContentType,
            FileExtension = Path.GetExtension(file.FileName)
        };

        var path = Path.Combine(_filesPath, randomName);

        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);

        return uploadFile;
    } 
    
}
