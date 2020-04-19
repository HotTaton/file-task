using FileTaskApiCore.Services;
using FileTaskApiCore.DataContract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FileTaskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService) => _fileService = fileService;

        [HttpGet("[action]")]
        public ActionResult<FileViewModel> RootDirectory()
        {
            return new JsonResult(_fileService.GetRootDirectory());
        }

        [HttpPost("[action]")]
        public ActionResult<FileViewModel> GetDirectory([FromBody] string directoryPath)
        {
            return new JsonResult(_fileService.GetFileTree(directoryPath));
        }

        [HttpPost("[action]")]
        public ActionResult<IEnumerable<IEnumerable<string>>> OpenFile([FromBody] string filePath)
        {
            return new JsonResult(_fileService.ReadFileData(filePath));
        }

        [HttpPost("[action]")]
        public ActionResult<string> SaveFile([FromBody] SaveFileViewModel file)
        {
            return new JsonResult(_fileService.SaveData(file));
        }
    }
}
