# Shop

### ProductAPI

#### 1 - DB context
- MySQL database creation
- Entities creation (Product and Category)
- ApplicationDbContext + Injection in the pipeline
- Dependencies: Pomelo, EntityFrameworkCore (.Tools, .Design)
- To migrate: add-migration ${name}; To Updated: update-database

#### 2 - Entities and DTOs
- Product and Category (n - 1)
- Product: Id, Name, Description, Price, Stock, ImageUrl, Category (Id)
- Category: Id, Name, ICollection<Products>

#### 3 - Controllers, Services, Repositories (+ AutoMapper)
- Application Logic: Controller <-- DTO --> Business Logic: Service <-- Entity --> Repository: DB interactions <--> Database
- Repositories: CategoryRepository (ICategoryRepository), ProductRepository (IProductRepository), UnitOfWork (IUnitOfWork)
- Services: CategoryService (ICategoryService <-- IService), ProductService (IService)
- Controller: CategoryController, ProductController

### Web
Web version integrating all microservices.

#### 1 - Controllers, Models, Services, Views
- Controller: Products Controller - consumes the data from Services
- Models: Replication of DTO sent from Microservices controllers
- Services: Fetch data from microservices using HttpClientFactory. ProductsService (IProductService), CategoryService (ICategoryService)
- Dependencies: Microsoft.Extensions.Http (for creating HttpClient to fetch microservices apis)
