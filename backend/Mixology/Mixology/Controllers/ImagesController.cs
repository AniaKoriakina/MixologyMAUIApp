using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mixology.Application.Features.Images.Commands.UploadBrandLogo;
using Mixology.Application.Features.Images.Commands.UploadCollectionImage;
using Mixology.Application.Features.Images.Commands.UploadMixImage;
using Mixology.Core.Base.Infrastructure;

namespace Mixology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public ImagesController(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet("mix/{mixId}")]
    public async Task<IActionResult> GetMixImage(long mixId)
    {
        var mix = await _unitOfWork.Mixes.GetByIdAsync(mixId);
        
        if (mix?.ImageData == null)
        {
            return NotFound(new { message = "Image not found" });
        }

        return File(mix.ImageData, mix.ImageContentType ?? "image/jpeg", mix.ImageFileName);
    }
    
    [HttpPost("mix/{mixId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadMixImage(long mixId, IFormFile image)
    {
        var command = new UploadMixImageCommand 
        { 
            MixId = mixId, 
            Image = image 
        };
        
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(new { message = "Image uploaded successfully" }) : BadRequest(result);
    }
    
    [HttpGet("brand/{brandId}")]
    public async Task<IActionResult> GetBrandLogo(long brandId)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(brandId);
        
        if (brand?.LogoData == null)
        {
            return NotFound(new { message = "Logo not found" });
        }

        return File(brand.LogoData, brand.LogoContentType ?? "image/jpeg", brand.LogoFileName);
    }
    
    [HttpPost("brand/{brandId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadBrandLogo(long brandId, IFormFile image)
    {
        var command = new UploadBrandLogoCommand 
        { 
            BrandId = brandId, 
            Image = image 
        };
        
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(new { message = "Logo uploaded successfully" }) : BadRequest(result);
    }
    
    [HttpGet("collection/{collectionId}")]
    public async Task<IActionResult> GetCollectionImage(long collectionId)
    {
        var collection = await _unitOfWork.Collections.GetByIdAsync(collectionId);
        
        if (collection?.ImageData == null)
        {
            return NotFound(new { message = "Image not found" });
        }

        return File(collection.ImageData, collection.ImageContentType ?? "image/jpeg", collection.ImageFileName);
    }
    
    [HttpPost("collection/{collectionId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCollectionImage(long collectionId, IFormFile image)
    {
        var command = new UploadCollectionImageCommand 
        { 
            CollectionId = collectionId, 
            Image = image 
        };
        
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(new { message = "Image uploaded successfully" }) : BadRequest(result);
    }
}
