using System.Security.Claims;
using API.Data;
using API.Dto;
using API.Entity;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("items")]
        [AllowAnonymous]
#nullable enable
        public async Task<ActionResult<IEnumerable<ProductDto>>> listProducts([FromQuery] PageParams productParams, [FromQuery] FilterParams? filterParams)
        {
            var products = _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                                .AsNoTracking();
            if (filterParams != null)
            {
                if (filterParams.Name != "-1")
                {
                    products = products.Where(x => x.Name.Contains(filterParams.Name));
                }
                if (filterParams.Category != 0)
                {
                    products = products.Where(x => x.CategoryId == filterParams.Category);
                }
                if (filterParams.MinPrice != 0)
                {
                    products = products.Where(x => x.Price >= filterParams.MinPrice);
                }
                if (filterParams.MaxPrice != 0)
                {
                    products = products.Where(x => x.Price <= filterParams.MaxPrice);
                }

                switch (filterParams.sort)
                {
                    case 1:
                        products = products.OrderBy(x => x.Price);
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Price);
                        break;
                    case 4:
                        products = products.OrderBy(x => x.InsertedOn);
                        break;
                    default:
                        products = products.OrderByDescending(x => x.InsertedOn);
                        break;
                }
            }

            var result = await PagedList<ProductDto>.CreateAsync(products, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
        }

        [HttpGet("myitems/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getUserProducts(int userId, [FromQuery] PageParams productParams)
        {
            var products = _context.Products
                            .Include(u => u.User)
                            .Include(c => c.Category)
                            .Where(x => x.User.Id == userId)
                            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                            .AsNoTracking();

            var result = await PagedList<ProductDto>.CreateAsync(products, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
        }

        [HttpGet("item/{productId}")]
        public async Task<ActionResult<ProductDto>> getProductById(int productId)
        {
            var product = await _context.Products.Include(u => u.User).Include(c => c.Category).Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            return Ok(_mapper.Map<ProductDto>(product));
        }

        //List all items which have a similar name as the search term
        //Later, Pagination to be added
        [HttpGet("item/search/{searchTerm}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByName(String searchTerm, [FromQuery] PageParams productParams)
        {
            var productList = _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsNoTracking();


            var result = await PagedList<ProductDto>.CreateAsync(productList, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
        }

        [HttpGet("items/category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByCategory(int categoryId, [FromQuery] PageParams productParams)
        {
            var products = _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .Where(p => p.CategoryId == categoryId).ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsNoTracking();
            var result = await PagedList<ProductDto>.CreateAsync(products, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
        }

        [HttpGet("favorites")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getFavoriteProducts([FromQuery] PageParams productParams, [FromQuery] FilterParams? filterParams)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstAsync();
            var elements = await _context.UserFavorites.Where(x => x.UserId == user.Id).Select(x => x.ProductId).ToListAsync();
            var products = _context.Products.Include(u => u.User).Include(c => c.Category).Where(x => elements.Contains(x.ProductId)).ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                                .AsNoTracking(); ;
            if (filterParams != null)
            {
                if (filterParams.Name != "-1")
                {
                    products = products.Where(x => x.Name.Contains(filterParams.Name));
                }
                if (filterParams.Category != 0)
                {
                    products = products.Where(x => x.CategoryId == filterParams.Category);
                }
                if (filterParams.MinPrice != 0)
                {
                    products = products.Where(x => x.Price >= filterParams.MinPrice);
                }
                if (filterParams.MaxPrice != 0)
                {
                    products = products.Where(x => x.Price <= filterParams.MaxPrice);
                }

                switch (filterParams.sort)
                {
                    case 1:
                        products = products.OrderBy(x => x.Price);
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Price);
                        break;
                    case 4:
                        products = products.OrderBy(x => x.InsertedOn);
                        break;
                    default:
                        products = products.OrderByDescending(x => x.InsertedOn);
                        break;
                }
            }
            var result = await PagedList<ProductDto>.CreateAsync(products, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
        }


        [HttpPost("item/add")]
        [Authorize]
        public async Task<ActionResult> addProduct(RegisterProductDto registerProductDto)
        {
            if (!await _context.Users.AnyAsync(x => x.Id == registerProductDto.InsertedBy))
            {
                return BadRequest("User Not found");
            }
            var product = new Product
            {
                Name = registerProductDto.Name,
                Description = registerProductDto.Description,
                InsertedBy = registerProductDto.InsertedBy,
                InsertedOn = registerProductDto.InsertedOn,
                CategoryId = registerProductDto.CategoryId,
                Price = registerProductDto.Price,
                IsSale = registerProductDto.IsSale
            };
            _context.Products.Add(product);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Error while adding");
            }
        }

        [HttpDelete("delete/{productId}")]
        [Authorize]
        public async Task<ActionResult> deleteProduct(int productId)
        {

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Not permitted");
            }

            var product = await _context.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            if (product == null)
            {
                return BadRequest("No product found!");
            }
            if (user.Role != 2 || user.Id != product.InsertedBy)
            {
                return BadRequest("Not permitted to do that!");
            }


            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpPost("favorites/add"), Authorize]
        public async Task<ActionResult> addToFavorites([FromBody] int productId)
        {
            Console.WriteLine(productId);
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Not Permitted!!!");
            }

            if (!await _context.Products.AnyAsync(x => x.ProductId == productId))
            {
                return BadRequest("Product not found!");
            }

            if (await _context.UserFavorites.AnyAsync(x => x.UserId == user.Id && x.ProductId == productId))
            {
                return BadRequest("Already Favorite!");
            }

            var userFavorite = new UserFavorites
            {
                UserId = user.Id,
                ProductId = productId
            };

            _context.UserFavorites.Add(userFavorite);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(true);
            }
            return BadRequest("Something went South!!!");
        }
        
        [HttpDelete("favorites/remove/{productId}"), Authorize]
        public async Task<ActionResult> removeFavorite(int productId)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Something went South");
            }
            var item = await _context.UserFavorites.Where(x => x.ProductId == productId && x.UserId == user.Id).FirstOrDefaultAsync();
            if(item == null){
                return BadRequest();
            }
            _context.UserFavorites.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("edit/{id}")]
        [Authorize]
        public async Task<ActionResult> editProduct(int id, UpdateProductDto productToUpdate)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();

            var product = await _context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();

            if (product == null || user == null || product.InsertedBy != user.Id)
            {
                return BadRequest("Something went wrong!");
            }

            _mapper.Map<UpdateProductDto, Product>(productToUpdate, product);
            _context.Products.Update(product);
            if (await _context.SaveChangesAsync() > 0)
                return Ok();
            return BadRequest("Not Mapped!");
        }
    }
}