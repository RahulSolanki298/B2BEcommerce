using System.IO.Compression;
using API.ViewModals;
using Core.Entities;
using Core.IRepository;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }


        [HttpGet("ProductList")]
        public async Task<ActionResult> GetProductList()
        {
            var response = await _productRepo.GetProductList();
            return Ok(response);
        }


        [HttpGet("ProductId/{productId}")]
        public async Task<ActionResult> GetProductList(Guid productId)
        {
            var response = await _productRepo.GetProductById(productId);
            return Ok(response);
        }

        [HttpPost("BulkProductUpload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Ensure the file is an Excel file
                if (!file.FileName.EndsWith(".xlsx"))
                {
                    return BadRequest("Invalid file format. Please upload an Excel (.xlsx) file.");
                }

                // Read the file using EPPlus
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets[0]; // Assuming you want the first sheet

                        var rows = new List<ProductVM>();

                        // Loop through rows and columns
                        for (int row = 2; row <= worksheet.Dimension.Rows; row++) // Start at row 2 to skip header
                        {
                            var data = new ProductVM
                            {
                                ProductType = worksheet.Cells[row, 1].Text,  // Column A
                                CategoryName = worksheet.Cells[row, 2].Text, // Column B
                                SubCategoryName = worksheet.Cells[row, 3].Text, // Column C
                                ColorName = worksheet.Cells[row, 4].Text, // Column C
                                ShapeName = worksheet.Cells[row, 5].Text, // Column C
                                CaratName = worksheet.Cells[row, 6].Text,
                                CaratSize = worksheet.Cells[row, 7].Text,
                                ClarityName = worksheet.Cells[row, 8].Text,
                                Sku = worksheet.Cells[row, 9].Text,
                                Price = Convert.ToDecimal(worksheet.Cells[row, 10].Text),
                                UnitPrice = Convert.ToDecimal(worksheet.Cells[row, 11].Text),
                                Quantity = Convert.ToInt32(worksheet.Cells[row, 12].Text)
                            };
                            rows.Add(data);
                        }

                        await _productRepo.SaveProductList(rows);

                    }
                }

                return Ok("File uploaded and processed successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("BulkProductImagesUpload")]
        public async Task<IActionResult> UploadProductImages(IFormFile zipFile)
        {
            string fileName=string.Empty;
            string[] fileNameParts;
            string productType, category, color, shape, indexNumber, ext;

            if (zipFile == null || zipFile.Length == 0)
                return BadRequest("No file uploaded.");

            var extractedFolder = Path.Combine("UploadedFiles", "Temp");  // Temporary folder for extraction
            Directory.CreateDirectory(extractedFolder);

            // Save ZIP to disk temporarily
            var zipPath = Path.Combine(extractedFolder, zipFile.FileName);
            using (var fileStream = new FileStream(zipPath, FileMode.Create))
            {
                await zipFile.CopyToAsync(fileStream);
            }

            // Extract images from the ZIP file
            ZipFile.ExtractToDirectory(zipPath, extractedFolder);

            var files = Directory.GetFiles(extractedFolder);

            foreach (var file in files)
            {
                fileName = Path.GetFileName(file);

                fileNameParts = fileName.Split('_');
                
                if (fileNameParts.Length != 4) continue;

                productType = fileNameParts[0];
                category = fileNameParts[1];
                color = fileNameParts[2];
                shape = fileNameParts[3];
                indexNumber= Path.GetFileNameWithoutExtension(fileName);
                ext= Path.GetExtension(fileName);

                
                
                
            }

            return Ok("Images uploaded and organized successfully.");
        }

    }
}
