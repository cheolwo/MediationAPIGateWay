using Common.FileStorage;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
using 仁Infra;

namespace MediationAPIGateWay.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileService fileService, ILogger<FileController> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileDto fileDto)
        {
            try
            {
                // 파일 업로드 처리 (Azure Blob 등)
                var fileUrl = await _fileService.UploadFileToBlobAsync(fileDto);

                // 업로드된 파일 메타데이터 생성
                var fileMetadata = new FileMetadata
                {
                    FileUrl = fileUrl,
                    FileName = fileDto.FileName,
                    FileSize = fileDto.File.Length, // IFormFile의 Length 사용
                    ContentType = fileDto.File.ContentType,
                    CreatedDate = DateTime.UtcNow  // 공통적으로 생성일자 기록
                };

                // 메타데이터를 데이터베이스에 저장하는 로직 추가 가능
                // await _dbContext.FileMetadata.AddAsync(fileMetadata);
                // await _dbContext.SaveChangesAsync();

                // 파일 메타데이터 반환
                return Ok(fileMetadata);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }
    }

}
