using MogoDbProductAPI.Data;
using MogoDbProductAPI.Domain.Contracts;
using MogoDbProductAPI.Domain.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MogoDbProductAPI.Service
{
    public class ProductService : IProductService
    {

        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            ValidateCreateDto(createProductDto);

            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
            };

            try
            {
                await _context.ProductCollection.InsertOneAsync(product);
                return MapToDto(product);

            }
            catch (Exception ex)
            {
                throw new MongoException($"Failed to create product : {ex.Message}");
            }

        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.ProductCollection.Find(Builders<Product>.Filter.Empty)
                    .ToListAsync();
                return products.Select(MapToDto);

            }
            catch (Exception ex)
            {
                throw new MongoException($"Failed to  retrieve product {ex.Message}");
            }
        }

        public async Task<ProductDto?> GetProductByIdAsync(string id)
        {
            ValidateObjectId(id);

            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var product = await _context.ProductCollection.Find(filter).FirstOrDefaultAsync()
                    ?? throw new MongoException($"Product with {id} not found");
                return MapToDto(product);

            }
            catch (Exception ex) when (ex is not MongoException)
            {
                throw new MongoException($"Database error {ex.Message}");
            }
        }

        public async Task<ProductDto> UpdateProductAsync(string id, UpdateProductDto updateProductDto)
        {
            ValidateObjectId(id);
            ValidateUpdateDto(updateProductDto);

            try
            {
                var update = Builders<Product>.Update
                    .Set(p => p.Name, updateProductDto.Name)
                    .Set(p => p.Description, updateProductDto.Description)
                    .Set(p => p.Price, updateProductDto.Price);

                var product = await _context.ProductCollection.FindOneAndUpdateAsync(
                    Builders<Product>.Filter.Eq(p => p.Id, id),
                    update,
                    new FindOneAndUpdateOptions<Product> { ReturnDocument = ReturnDocument.After }
                ) ?? throw new MongoException($"Product with ID {id} not found");

                return MapToDto(product);
            }
            catch (Exception ex) when (ex is not MongoException)
            {
                throw new MongoException($"Failed to update product: {ex.Message}");
            }
        }


        public async Task<bool> DeleteProductAsync(string id)
        {
            ValidateObjectId(id);

            try
            {
                var result = await _context.ProductCollection.DeleteOneAsync(p => p.Id == id);
                if (result.DeletedCount == 0)
                    throw new MongoException($"Product with ID {id} not found");

                return true;
            }
            catch (Exception ex) when (ex is not MongoException)
            {
                throw new MongoException($"Failed to delete product: {ex.Message}");
            }
        }







        // Validate id 
        private static void ValidateObjectId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new MongoException("Product ID cannot be empty");
            if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
                throw new MongoException("Invalid MongoDB ObjectId format");
        }

        // Validate create dto 
        private static void ValidateCreateDto(CreateProductDto dto)
        {
            if (dto == null)
                throw new MongoException("Product data is required");

            if (string.IsNullOrEmpty(dto.Name))
                throw new MongoException("Product name is required");
        }


        // Validate udpate dto 

        private static void ValidateUpdateDto(UpdateProductDto dto)
        {
            if (dto == null)
                throw new MongoException("Update data is required");

            if (string.IsNullOrEmpty(dto.Name))
                throw new MongoException("Product name is required");
        }

        // map everything to dto 

        private static ProductDto MapToDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };


        public async Task<PagedResult<ProductDto>> GetFilteredProductsAsync(
    string? search,
    string? sortBy,
    string? sortDirection,
    int page,
    int pageSize)
{
    var filterBuilder = Builders<Product>.Filter;
    var filter = FilterDefinition<Product>.Empty;

    if (!string.IsNullOrEmpty(search))
    {
        filter = filterBuilder.Or(
            filterBuilder.Regex(x => x.Name, new BsonRegularExpression(search, "i")),
            filterBuilder.Regex(x => x.Description, new BsonRegularExpression(search, "i"))
        );
    }

    var sortBuilder = Builders<Product>.Sort;
    SortDefinition<Product>? sort = null;

    if (!string.IsNullOrEmpty(sortBy))
    {
        if (sortBy.ToLower() == "name")
            sort = sortDirection == "desc" ? sortBuilder.Descending(x => x.Name) : sortBuilder.Ascending(x => x.Name);
        else if (sortBy.ToLower() == "price")
            sort = sortDirection == "desc" ? sortBuilder.Descending(x => x.Price) : sortBuilder.Ascending(x => x.Price);
    }

    var query = _context.ProductCollection.Find(filter);

    if (sort != null)
        query = query.Sort(sort);

    var totalCount = await query.CountDocumentsAsync();

    var results = await query
        .Skip((page - 1) * pageSize)
        .Limit(pageSize)
        .ToListAsync();

    return new PagedResult<ProductDto>
    {
        TotalCount = (int)totalCount,
        Items = results.Select(MapToDto)
    };
}

    }
}
