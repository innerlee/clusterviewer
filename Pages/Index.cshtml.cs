using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace viewer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["path"] = Request.Query["path"];
            if (Directory.Exists(Request.Query["path"])){
                ViewData["haha"] = Directory.GetFiles(Request.Query["path"]);
            }else{
                ViewData["haha"] = new string[]{};
            }
        }
    }
}
