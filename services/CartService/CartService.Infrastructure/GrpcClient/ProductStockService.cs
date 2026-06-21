using CartService.Application.Interfaces;
using CartService.Infrastructure.Grpc; // Простір імен із твого proto-файлу в кошику
using Grpc.Core;

namespace CartService.Infrastructure.GrpcClients;

public class ProductStockService : IProductStockService
{
    // Інжектуємо автоматично згенерований .NET-клієнт
    private readonly InventoryStorage.InventoryStorageClient _grpcClient;

    public ProductStockService(InventoryStorage.InventoryStorageClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<bool> IsInStockAsync(string productId, int quantity)
    {
        // 1. Пакуємо дані в об'єкт запиту, як вимагає контракт
        var request = new StockRequest
        {
            ProductId = productId,
            RequestedQuantity = quantity
        };

        try
        {
            // 2. Робимо реальний gRPC-дзвінок до мікросервісу Product.
            // Передаємо запит і cancellationToken (на випадок, якщо користувач скасує дію)
            var response = await _grpcClient.CheckProductStockAsync(request);

            // 3. Повертаємо результат, який нам прислав ProductService
            return response.IsAvailable;
        }
        catch (RpcException ex)
        {
            // Цей рядок виведе в консоль Кошика точну технічну причину (наприклад: StatusCode=Unavailable, DeadlineExceeded)
            Console.WriteLine($"!!! gRPC КРИТИЧНА ПОМИЛКА: Статус={ex.StatusCode}, Деталі={ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"!!! ІНША ПОМИЛКА ПІДКЛЮЧЕННЯ: {ex.Message}");
            return false;
        }
    }
}