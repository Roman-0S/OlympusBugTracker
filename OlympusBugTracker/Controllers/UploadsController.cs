﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;

namespace OlympusBugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UploadsController(ApplicationDbContext context) : ControllerBase
    {


        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60)]
        public async Task<IActionResult> GetImage(Guid Id)
        {

            FileUpload? image = await context.Images.FirstOrDefaultAsync(i => i.Id == Id);

            return image == null ? NotFound() : File(image.Data!, image.Type!);

        }


    }
}
