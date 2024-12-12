using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace FileManager.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController(IFileService fileService) : ControllerBase
{
    private readonly IFileService _fileService = fileService;

    [HttpPost("")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest uploadFileRequest, CancellationToken cancellationToken)
    {
        var fileId = await _fileService.UploadAsync(uploadFileRequest.File, cancellationToken);

        return CreatedAtAction(nameof(Download), new {id = fileId}, null);
    }
    [HttpPost("upload-many")]
    public async Task<IActionResult> UploadMany([FromForm] UploadManyFilesRequest uploadManyFileRequest, CancellationToken cancellationToken)
    {
        var fileIds = await _fileService.UploadManyAsync(uploadManyFileRequest, cancellationToken);

        return Ok(fileIds);
    }
    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest uploadImageRequest, CancellationToken cancellationToken)
    {
        await _fileService.UploadImageAsync(uploadImageRequest.Image, cancellationToken);

        return Created();
    }
    [HttpGet("download/{id:guid}")]
    public async Task<IActionResult> Download([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var (fileContent, contentType, fileName) = await _fileService.DownloadAsync(id, cancellationToken);


        return fileContent is [] ? NotFound() : File(fileContent, contentType, fileName);
    }
    [HttpGet("stream/{id:guid}")]
    public async Task<IActionResult> Stream([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var (fileContent, contentType, fileName) = await _fileService.StreamAsync(id, cancellationToken);


        return fileContent is null ? NotFound() : File(fileContent, contentType, fileName, true);
    }
}
