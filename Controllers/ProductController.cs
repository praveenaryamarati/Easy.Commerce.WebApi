﻿using Easy.Commerce.WebApi.Models;
using Easy.Commerce.WebApi.Security;
using Easy.Commerce.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Commerce.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        
        [AllowAnonymous]
        public ActionResult Get()
        {
            return Ok(productService.Get());
        }

        
        [HttpGet("{id}.{format?}", Name = "Get")]
        [Authorize(Policy = Policies.Customer)]
        public ActionResult Get(int id)
        {
            var product = this.productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        public ActionResult Post([FromBody] Product value)
        {
            var product = productService.Save(value);
            return CreatedAtAction("Get", new { id = product.ProductID }, value);
        }

        
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.Admin)]
        public ActionResult Put(int id, [FromBody] Product value)
        {
            if (id != value.ProductID)
            {
                return BadRequest();
            }
            productService.Save(value);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Admin)]
        public ActionResult Delete(int id)
        {
            if (!productService.Exist(id))
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
