using Microsoft.AspNetCore.Http;

namespace 仁Infra
{
    public class FileDto
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; } // 사용자가 제공한 파일명
    }
}
