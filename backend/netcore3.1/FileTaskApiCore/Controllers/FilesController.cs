using FileTaskApiCore.DataContract;
using FileTaskApiCore.DataContract.Response;
using FileTaskApiCore.Filters;
using FileTaskApiCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileTaskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;            
        }
            

        [HttpGet("[action]")]
        public async Task<ActionResult<Response<FileViewModel>>> RootDirectory()
        {
            var result = await _fileService.GetRootDirectory();
            return new JsonResult(result); 
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Response<FileViewModel>>> GetDirectory([FromBody] string directoryPath)
        {
            var result = await _fileService.GetFileTree(directoryPath);
            return new JsonResult(result);
        }

        [HttpPost("[action]")]
        [FixFileName]
        public async Task<ActionResult<Response<FileContentViewModel>>> OpenFile([FromBody] string filePath)
        {
            var result = await _fileService.ReadFileData(filePath);
            return new JsonResult(result);
        }

        [HttpPost("[action]")]
        [FixFileName]
        public async Task<ActionResult<Response<bool>>> SaveFile([FromBody] SaveFileViewModel file)
        {
            var result = await _fileService.SaveData(file);
            return new JsonResult(result);
        }
    }
}
