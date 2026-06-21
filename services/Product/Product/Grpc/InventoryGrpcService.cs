using Grpc.Core;
using Product.Application.Interfaces;
using ProductService.API.Grpc; // Простір імен, який ми вказали в option csharp_namespace у нашому proto-файлі

namespace ProductService.API.GrpcServices;

// Ми наслідуємося від базового класу, який .NET автоматично згенерував на основі нашого proto-файлу
public class InventoryGrpcService : InventoryStorage.InventoryStorageBase
{
    // Сюди через конструктор (Dependency Injection) ти інжектуєш свій сервіс продуктів,
    // який вміє ходити в базу даних твого мікросервісу Product.
    // Наприклад:
    // private readonly IProductService _productService;

    private readonly IProductService _productService;
    public InventoryGrpcService(IProductService productService)
    {
        _productService = productService;
    }

    // Перевизначаємо метод, який ми задекларували в inventory.proto
    public override async Task<StockResponse> CheckProductStock(StockRequest request, ServerCallContext context)
    {
        // 1. request.ProductId містить ID товару, який прислав Cart мікросервіс.
        // 2. request.RequestedQuantity містить кількість, яку хоче купити користувач.

        // ТУТ БУДЕ ТВОЯ РЕАЛЬНА ЛОГІКА З БАЗИ ДАНИХ. Наприклад:
        // var product = await _productService.GetByIdAsync(request.ProductId);
        // bool isAvailable = product != null && product.Quantity >= request.RequestedQuantity;

        var productId = int.Parse(request.ProductId);

        var product = await _productService.GetProductById(productId);

        Console.WriteLine(product.Stock);

        Console.WriteLine($"!!! МЕГА-ТЕСТ: gRPC запит дійшов! Перевіряємо товар з ID: {request.ProductId}");

        // Для першого тесту зробимо заглушку, що товар нібито є завжди
        bool isAvailable = product != null && product.Stock >= request.RequestedQuantity;

        // Повертаємо бінарну відповідь назад у Cart мікросервіс
        return new StockResponse
        {
            IsAvailable = isAvailable
        };
    }
}