using System.Drawing;
using API.ViewModals;
using Core.Entities;
using Core.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPropertiesController : ControllerBase
    {
        private readonly IGenericRepository<ProductClarity> _productClarityRepo;
        private readonly IGenericRepository<ProductColor> _productColorRepo;
        private readonly IGenericRepository<ProductShapes> _productShapesRepo;
        private readonly IGenericRepository<ProductCarat> _productCaratRepo;
        private readonly IGenericRepository<Category> _productCategoryRepo;
        public ProductPropertiesController(IGenericRepository<ProductClarity> productClarityRepo,
            IGenericRepository<ProductColor> productColorRepo,
            IGenericRepository<ProductShapes> productShapesRepo,
            IGenericRepository<ProductCarat> productCaratRepo,
            IGenericRepository<Category> productCategoryRepo)
        {
            _productClarityRepo = productClarityRepo;
            _productColorRepo = productColorRepo;
            _productShapesRepo = productShapesRepo;
            _productCaratRepo = productCaratRepo;
            _productCategoryRepo = productCategoryRepo;
        }

        #region Clarity
        /// <summary>
        /// Clarity Operations
        /// </summary>
        /// <returns></returns>
        /// 

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("clarity-list")]
        public async Task<ActionResult> GetClarityList()
        {
            try
            {
                var clarityList = await _productClarityRepo.ListAllAsync();
                if (clarityList == null || !clarityList.Any())
                {
                    return NotFound("No clarity records found.");
                }
                return Ok(clarityList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("clarity/clarityId/{clarityId}")]
        public async Task<ActionResult> GetClarity(int clarityId)
        {
            if (clarityId <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var clarity = await _productClarityRepo.GetByIdAsync(clarityId);
                if (clarity == null)
                {
                    return NotFound($"Clarity with ID {clarityId} not found.");
                }

                return Ok(clarity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("clarity/clarityName/{clarityName}")]
        public async Task<ActionResult> GetClarityByName(string clarityName)
        {
            if (!string.IsNullOrEmpty(clarityName))
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var clarityList = await _productClarityRepo.ListAllAsync();
                if (clarityList.Count == 0)
                {
                    return NotFound($"Clarity not found.");
                }

                var clarity = clarityList.Where(c => c.Name == clarityName).FirstOrDefault();

                return Ok(clarity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("save-clarity")]
        public async Task<ActionResult<ApiResponse<ProductClarity>>> SaveClarity(ProductClarity productClarity)
        {
            if (productClarity == null)
            {
                return BadRequest(new ApiResponse<ProductClarity>
                {
                    Success = false,
                    Message = "Product clarity data is required."
                });
            }

            if (string.IsNullOrEmpty(productClarity.Name))
            {
                return BadRequest(new ApiResponse<ProductClarity>
                {
                    Success = false,
                    Message = "Invalid product clarity data: 'Name' cannot be empty."
                });
            }

            try
            {
                if (productClarity.Id > 0)
                {
                    _productClarityRepo.Update(productClarity);
                }
                else
                {
                    // Validate the properties of the productClarity object
                    var clarityList = await _productClarityRepo.ListAllAsync();
                    if (clarityList.Any(c => c.Name == productClarity.Name))
                    {
                        return BadRequest(new ApiResponse<ProductClarity>
                        {
                            Success = false,
                            Message = "Clarity name already exists."
                        });
                    }


                    _productClarityRepo.Add(productClarity);
                }
                _productClarityRepo.SaveChanges();

                var response = await _productClarityRepo.GetByIdAsync(productClarity.Id);
                if (response == null)
                {
                    return NotFound(new ApiResponse<ProductClarity>
                    {
                        Success = false,
                        Message = "Failed to save clarity."
                    });
                }


                return Ok(new ApiResponse<ProductClarity>
                {
                    Success = true,
                    Message = "Clarity saved successfully.",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ProductClarity>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpDelete("delete-clarity/clarityId/{clarityId}")]
        public async Task<ActionResult<ApiResponse<ProductClarity>>> DeleteClarity(int clarityId)
        {
            // Step 1: Validate if the ProductClarity exists with the provided clarityId
            var productClarity = await _productClarityRepo.GetByIdAsync(clarityId);

            if (productClarity == null)
            {
                // If the clarityId doesn't exist, return a Not Found response
                return NotFound(new ApiResponse<ProductClarity>
                {
                    Success = false,
                    Message = $"Product clarity with ID {clarityId} not found."
                });
            }

            try
            {
                // Step 2: Delete the ProductClarity object
                _productClarityRepo.Delete(productClarity);
                _productClarityRepo.SaveChanges();

                // Step 3: Return success response
                return Ok(new ApiResponse<ProductClarity>
                {
                    Success = true,
                    Message = "Product clarity deleted successfully.",
                    Data = productClarity
                });
            }
            catch (Exception ex)
            {
                // If any exception occurs during the delete operation, return a 500 error
                return StatusCode(500, new ApiResponse<ProductClarity>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        #endregion

        #region Colors
        /// <summary>
        /// All Color related operations
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("color-list")]
        public async Task<ActionResult> GetColorList()
        {
            try
            {
                var colorList = await _productColorRepo.ListAllAsync();
                if (colorList == null || !colorList.Any())
                {
                    return NotFound("No color records found.");
                }
                return Ok(colorList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("color/colorId/{colorId}")]
        public async Task<ActionResult> GetColor(int colorId)
        {
            try
            {
                var colorDT = await _productColorRepo.GetByIdAsync(colorId);
                if (colorDT == null)
                {
                    return NotFound("No color records found.");
                }
                return Ok(colorDT);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("color/colorName/{colorName}")]
        public async Task<ActionResult> GetcolorByName(string colorName)
        {
            if (!string.IsNullOrEmpty(colorName))
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var colorList = await _productColorRepo.ListAllAsync();
                if (colorList.Count == 0)
                {
                    return NotFound($"Color not found.");
                }

                var color = colorList.Where(c => c.Name == colorName).FirstOrDefault();

                return Ok(color);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("save-product-color")]
        public async Task<ActionResult<ApiResponse<ProductColor>>> SaveProductColor(ProductColor productColor)
        {
            // Validate the input
            if (productColor == null)
            {
                return BadRequest(new ApiResponse<ProductColor>
                {
                    Success = false,
                    Message = "Product color data is required."
                });
            }

            if (string.IsNullOrEmpty(productColor.Name))
            {
                return BadRequest(new ApiResponse<ProductColor>
                {
                    Success = false,
                    Message = "Invalid product color data: 'Name' cannot be empty."
                });
            }

            try
            {
                // Update if the Id is greater than 0, otherwise add a new entry
                if (productColor.Id > 0)
                {
                    _productColorRepo.Update(productColor);
                }
                else
                {
                    var colorList = await _productColorRepo.ListAllAsync();
                    if (colorList.Any(c => c.Name == productColor.Name))
                    {
                        return BadRequest(new ApiResponse<ProductClarity>
                        {
                            Success = false,
                            Message = "Color name already exists."
                        });
                    }

                    _productColorRepo.Add(productColor);
                }

                // Save changes
                _productColorRepo.SaveChanges();

                // Retrieve the saved product color object
                var response = await _productColorRepo.GetByIdAsync(productColor.Id);
                if (response == null)
                {
                    return NotFound(new ApiResponse<ProductColor>
                    {
                        Success = false,
                        Message = "Failed to save product color."
                    });
                }

                // Return success response
                return Ok(new ApiResponse<ProductColor>
                {
                    Success = true,
                    Message = "Product color saved successfully.",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                // Log the exception (if you have a logger in place, log the exception details)
                return StatusCode(500, new ApiResponse<ProductColor>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpDelete("delete-color/colorId/{colorId}")]
        public async Task<ActionResult<ApiResponse<ProductColor>>> DeleteColor(int colorId)
        {
            // Step 1: Validate if the ProductColor exists with the provided colorId
            var productColor = await _productColorRepo.GetByIdAsync(colorId);

            if (productColor == null)
            {
                // If the clarityId doesn't exist, return a Not Found response
                return NotFound(new ApiResponse<ProductColor>
                {
                    Success = false,
                    Message = $"Product color with ID {colorId} not found."
                });
            }

            try
            {
                // Step 2: Delete the ProductClarity object
                _productColorRepo.Delete(productColor);
                _productColorRepo.SaveChanges();

                // Step 3: Return success response
                return Ok(new ApiResponse<ProductColor>
                {
                    Success = true,
                    Message = "Product color deleted successfully.",
                    Data = productColor
                });
            }
            catch (Exception ex)
            {
                // If any exception occurs during the delete operation, return a 500 error
                return StatusCode(500, new ApiResponse<ProductColor>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        #endregion

        #region Carats
        /// <summary>
        /// Carats
        /// </summary>
        /// <returns></returns>

        [HttpGet("carat-list")]
        public async Task<ActionResult> GetCaratList()
        {
            try
            {
                var caratList = await _productCaratRepo.ListAllAsync();
                if (caratList == null || !caratList.Any())
                {
                    return NotFound("No carat records found.");
                }
                return Ok(caratList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("carat/{caratId}")]
        public async Task<ActionResult> GetCaratById(int caratId)
        {
            try
            {
                var caratDT = await _productCaratRepo.GetByIdAsync(caratId);
                if (caratDT == null)
                {
                    return NotFound("No carat records found.");
                }
                return Ok(caratDT);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("carat/caratName/{caratName}")]
        public async Task<ActionResult> GetCaratByName(string caratName)
        {
            if (!string.IsNullOrEmpty(caratName))
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var caratList = await _productColorRepo.ListAllAsync();
                if (caratList.Count == 0)
                {
                    return NotFound($"Carat not found.");
                }

                var carat = caratList.Where(c => c.Name == caratName).FirstOrDefault();

                return Ok(carat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("save-product-carat")]
        public async Task<ActionResult<ApiResponse<ProductCarat>>> SaveProductCarat(ProductCarat productCarat)
        {
            // Validate the input
            if (productCarat == null)
            {
                return BadRequest(new ApiResponse<ProductCarat>
                {
                    Success = false,
                    Message = "Product carat data is required."
                });
            }

            if (string.IsNullOrEmpty(productCarat.Name))
            {
                return BadRequest(new ApiResponse<ProductCarat>
                {
                    Success = false,
                    Message = "Invalid product carat data: 'Name' cannot be empty."
                });
            }

            try
            {
                // Update if the Id is greater than 0, otherwise add a new entry
                if (productCarat.Id > 0)
                {
                    _productCaratRepo.Update(productCarat);
                }
                else
                {
                    var caratList = await _productCaratRepo.ListAllAsync();
                    if (caratList.Count > 0)
                    {
                        if (caratList.Any(x=>x.Name==productCarat.Name))
                        {
                            return BadRequest(new ApiResponse<ProductCarat>
                            {
                                Success = false,
                                Message = "Carat name already exists."
                            });
                        }
                        
                    }
                    _productCaratRepo.Add(productCarat);
                }

                // Save changes to the repository
                _productCaratRepo.SaveChanges();

                // Retrieve the saved product carat object
                var response = await _productCaratRepo.GetByIdAsync(productCarat.Id);
                if (response == null)
                {
                    return NotFound(new ApiResponse<ProductCarat>
                    {
                        Success = false,
                        Message = "Failed to save product carat.",
                    });
                }

                // Return success response
                return Ok(new ApiResponse<ProductCarat>
                {
                    Success = true,
                    Message = "Product carat saved successfully.",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                // Log the exception (consider adding a logging mechanism here)
                return StatusCode(500, new ApiResponse<ProductCarat>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}",
                });
            }
        }

        [HttpDelete("delete-carat/caratId/{caratId}")]
        public async Task<ActionResult<ApiResponse<ProductCarat>>> DeleteCarat(int caratId)
        {
            // Step 1: Validate if the ProductColor exists with the provided caratId
            var productCarat = await _productCaratRepo.GetByIdAsync(caratId);

            if (productCarat == null)
            {
                // If the clarityId doesn't exist, return a Not Found response
                return NotFound(new ApiResponse<ProductCarat>
                {
                    Success = false,
                    Message = $"Product carat with ID {caratId} not found."
                });
            }

            try
            {
                // Step 2: Delete the ProductClarity object
                _productCaratRepo.Delete(productCarat);
                _productCaratRepo.SaveChanges();

                // Step 3: Return success response
                return Ok(new ApiResponse<ProductCarat>
                {
                    Success = true,
                    Message = "Product carat deleted successfully.",
                    Data = productCarat
                });
            }
            catch (Exception ex)
            {
                // If any exception occurs during the delete operation, return a 500 error
                return StatusCode(500, new ApiResponse<ProductCarat>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        #endregion

        #region Shapes
        /// <summary>
        /// Shapes
        /// </summary>
        /// <returns></returns>

        [HttpGet("shape-list")]
        public async Task<ActionResult> GetShapeList()
        {
            try
            {
                var shapeList = await _productShapesRepo.ListAllAsync();
                if (shapeList == null || !shapeList.Any())
                {
                    return NotFound("No shape records found.");
                }
                return Ok(shapeList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("shape/{shapeId}")]
        public async Task<ActionResult> GetShapeById(int shapeId)
        {
            try
            {
                var shapeDT = await _productShapesRepo.GetByIdAsync(shapeId);
                if (shapeDT == null)
                {
                    return NotFound("No shape records found.");
                }
                return Ok(shapeDT);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("shape/shapeName/{shapeName}")]
        public async Task<ActionResult> GetShapeByName(string shapeName)
        {
            if (!string.IsNullOrEmpty(shapeName))
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var shapeList = await _productShapesRepo.ListAllAsync();
                if (shapeList.Count == 0)
                {
                    return NotFound($"Shape not found.");
                }

                var shape = shapeList.Where(c => c.Name == shapeName).FirstOrDefault();

                return Ok(shape);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("save-product-shape")]
        public async Task<ActionResult<ApiResponse<ProductShapes>>> SaveProductShape(ProductShapes productShape)
        {
            // Validate the input
            if (productShape == null)
            {
                return BadRequest(new ApiResponse<ProductShapes>
                {
                    Success = false,
                    Message = "Product shape data is required."
                });
            }

            if (string.IsNullOrEmpty(productShape.Name))
            {
                return BadRequest(new ApiResponse<ProductShapes>
                {
                    Success = false,
                    Message = "Invalid product shape data: 'Name' cannot be empty."
                });
            }

            try
            {
                // Update if the Id is greater than 0, otherwise add a new entry
                if (productShape.Id > 0)
                {
                    _productShapesRepo.Update(productShape);
                }
                else
                {
                    var shapeList= await _productShapesRepo.ListAllAsync();
                    if(shapeList.Count > 0)
                    {
                        var shape= shapeList.Where(x=>x.Name == productShape.Name).FirstOrDefault();
                        if(shape != null)
                        {
                            return BadRequest(new ApiResponse<ProductShapes>
                            {
                                Success = false,
                                Message = "Product shape already exists."
                            });
                        }
                    }
                    _productShapesRepo.Add(productShape);
                }

                // Save changes to the repository
                _productShapesRepo.SaveChanges();

                // Retrieve the saved product shape object
                var response = await _productShapesRepo.GetByIdAsync(productShape.Id);
                if (response == null)
                {
                    return NotFound(new ApiResponse<ProductShapes>
                    {
                        Success = false,
                        Message = "Failed to save product shape."
                    });
                }

                // Return success response
                return Ok(new ApiResponse<ProductShapes>
                {
                    Success = true,
                    Message = "Product shape saved successfully.",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ProductShapes>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpDelete("delete-shape/shapeId/{shapeId}")]
        public async Task<ActionResult<ApiResponse<ProductShapes>>> DeleteShape(int shapeId)
        {
            // Step 1: Validate if the ProductShape exists with the provided shapeId
            var productShape = await _productShapesRepo.GetByIdAsync(shapeId);

            if (productShape == null)
            {
                // If the clarityId doesn't exist, return a Not Found response
                return NotFound(new ApiResponse<ProductShapes>
                {
                    Success = false,
                    Message = $"Product shape with ID {shapeId} not found."
                });
            }

            try
            {
                // Step 2: Delete the ProductClarity object
                _productShapesRepo.Delete(productShape);
                _productShapesRepo.SaveChanges();

                // Step 3: Return success response
                return Ok(new ApiResponse<ProductShapes>
                {
                    Success = true,
                    Message = "Product shape deleted successfully.",
                    Data = productShape
                });
            }
            catch (Exception ex)
            {
                // If any exception occurs during the delete operation, return a 500 error
                return StatusCode(500, new ApiResponse<ProductShapes>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }


        #endregion

        #region Category

        [HttpGet("category-list")]
        public async Task<ActionResult> GetCategoryList()
        {
            var response = await _productCategoryRepo.ListAllAsync();
            return Ok(response);
        }

        [HttpGet("category/categoryId/{categoryId}")]
        public async Task<ActionResult> GetCategory(int categoryId)
        {
            var response = await _productCategoryRepo.GetByIdAsync(categoryId);
            return Ok(response);
        }

        [HttpGet("category/categoryName/{categoryName}")]
        public async Task<ActionResult> GetCategoryByName(string categoryName)
        {
            var response = await _productCategoryRepo.ListAllAsync();
            var category= response.Where(x=>x.Name == categoryName).FirstOrDefault();
            if (category != null)
            {
                return BadRequest("Category name already exist.");
            }
            return Ok(category);
        }

        #endregion

    }
}
