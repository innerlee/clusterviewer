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

    public class SelectItem
    {
        public string path;
        public string origpath;
        public string tag;

        public SelectItem(string path, string origpath, string tag)
        {
            this.path = path;
            this.origpath = origpath;
            this.tag = tag;
        }
    }

    public class SelectModel : PageModel
    {
        private readonly ILogger<SelectModel> _logger;

        public SelectModel(ILogger<SelectModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["folder"] = Request.Query["folder"];
            ViewData["root"] = Request.Query["root"];
            ViewData["list"] = Request.Query["list"];
            ViewData["listfile"] = Path.GetFileName(ViewData["list"].ToString());
            ViewData["page"] = 1;
            ViewData["pagesize"] = 500;
            var exts = new List<string>() { ".jpg", ".jpeg", ".png", ".gif" };

            string listfile = Request.Query["list"].ToString();
            int page = (int)(ViewData["page"]);
            if (int.TryParse(Request.Query["page"].ToString(), out page))
            {
                ViewData["page"] = page;
            }
            int pagesize = (int)(ViewData["pagesize"]);
            if (int.TryParse(Request.Query["pagesize"].ToString(), out pagesize))
            {
                ViewData["pagesize"] = pagesize;
            }

            List<SelectItem> items = new List<SelectItem>();
            if (Directory.Exists(ViewData["folder"].ToString()))
            {
                var folder = ViewData["folder"].ToString();
                ViewData["listfile"] = Path.GetFileName(folder);
                foreach (var item in Directory.EnumerateFiles(folder))
                {
                    if (exts.Contains(Path.GetExtension(item).ToLower()))
                    {
                        items.Add(new SelectItem(item, item, Path.GetFileName(item)));
                    }
                }
            }
            else if (System.IO.File.Exists(listfile))
            {
                using (var reader = new StreamReader(listfile))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(new char[0], 2, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 1)
                        {
                            items.Add(new SelectItem(Path.Combine(ViewData["root"].ToString(), values[0]), values[0], values[0]));
                        }
                        else
                        {
                            items.Add(new SelectItem(Path.Combine(ViewData["root"].ToString(), values[0]), values[0], values[0] + " " + values[1]));
                        }
                    }
                }
            }
            ViewData["totalitems"] = items.Count;
            page = (int)(ViewData["page"]);
            pagesize = (int)(ViewData["pagesize"]);
            ViewData["items"] = items.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
    }
}
