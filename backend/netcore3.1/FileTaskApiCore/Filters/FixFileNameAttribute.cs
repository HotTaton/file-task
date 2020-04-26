using FileTaskApiCore.DataContract;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace FileTaskApiCore.Filters
{
    /// <summary>
    /// Фильтр, который позволяет убрать служебные символы из имени файла
    /// </summary>
    public class FixFileNameAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trimmingChar = (char)8234;
                        
            if (context.ActionArguments.ContainsKey("filePath"))
            {
                var strFileName = context.ActionArguments["filePath"]?.ToString();
                if (!string.IsNullOrEmpty(strFileName))
                {
                    strFileName = strFileName.Trim(trimmingChar);
                    context.ActionArguments["filePath"] = strFileName;
                }                
            }
            else if (context.ActionArguments.ContainsKey("file"))
            {
                if (context.ActionArguments["file"] is SaveFileViewModel saveFileObject)
                {
                    saveFileObject.FileName = saveFileObject.FileName.Trim(trimmingChar);
                    context.ActionArguments["file"] = saveFileObject;
                }
            }
            await next();
        }
    }
}
