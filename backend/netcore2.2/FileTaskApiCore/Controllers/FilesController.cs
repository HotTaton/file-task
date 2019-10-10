using FileTaskApiCore.Models;
using FileTaskApiCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FileTaskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private FileModel _model = new FileModel();

        [HttpGet("[action]")]
        public ActionResult<FileViewModel> RootDirectory()
        {
            return new JsonResult(_model.GetFileTree());
        }

        [HttpPost("[action]")]
        public ActionResult<FileViewModel> GetDirectory([FromBody] string directoryPath)
        {
            return new JsonResult(_model.GetFileTree(directoryPath));
        }
    }
}
