﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace viewer.Pages
{

    public class Item
    {
        public string path;
        public string tag;

        public Item(string path, string tag)
        {
            this.path = path;
            this.tag = tag;
        }
    }

    public class ListModel : PageModel
    {
        private readonly ILogger<ListModel> _logger;

        public ListModel(ILogger<ListModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["list"] = Request.Query["list"];
            ViewData["page"] = Request.Query["page"];
            ViewData["pagesize"] = Request.Query["pagesize"];

            string listfile = Request.Query["list"].ToString();
            int page = 1;
            int.TryParse(ViewData["page"].ToString(), out page);
            int pagesize = 50;
            int.TryParse(ViewData["pagesize"].ToString(), out pagesize);

            List<Item> items = new List<Item>();
            if (System.IO.File.Exists(listfile))
            {
                using (var reader = new StreamReader(listfile))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split('\t', 2, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 1)
                        {
                            items.Add(new Item(values[0], values[0]));
                        }
                        else
                        {

                            items.Add(new Item(values[0], values[1]));
                        }
                    }
                    ViewData["items"] = items;
                }
            }
            ViewData["items"] = items.Skip((page - 1) * pagesize).Take(pagesize).ToList();
        }
    }
}