﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CategoryAdd
    {

            public string Title { get; set; }
          
            public IFormFile Picture { get; set; }
    }
}
