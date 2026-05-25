using POS.Database.Entities;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
using POS.Shared.DTOs.Sales;
using POS.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISaleRepository _salesRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGenerateInvoiceHelper _generateInvoiceHelper;
        public SalesService(ISaleRepository saleRepository, IProductRepository productRepository, IGenerateInvoiceHelper generateInvoice)
        {
            _salesRepository = saleRepository;
            _productRepository = productRepository;
            _generateInvoiceHelper = generateInvoice;
        }

        public async Task<Result<SaleResponseDto>> CreateSaleAsync(CreateSaleDto dto)
        {
            try
            {
                if (dto is null || dto.Items == null || !dto.Items.Any())
                {
                    return Result<SaleResponseDto>.Failure("Sale must contain at least one item");
                }

                var mergedProductIds = dto.Items.GroupBy(i => i.ProductId)
                    .Select(g => new CreateSaleItemDto
                    {
                        ProductId = g.Key,
                        Quantity = g.Sum(i => i.Quantity)
                    })
                    .ToList();

                var productIds = mergedProductIds.Select(i => i.ProductId).ToList();

                var products = await _productRepository.GetProductsforCreateSale(productIds);

                if (products.Count != productIds.Count)
                {
                    return Result<SaleResponseDto>.Failure("One or more products not found");
                }

                decimal totalAmount = 0;
                var generatedInvoiceNo = await _generateInvoiceHelper.GenerateInvoiceNumber();
                var saleItems = new List<SaleItem>();
                var productDict = products.ToDictionary(p => p.Id);

                foreach (var item in mergedProductIds)
                {
                    var product = productDict[item.ProductId];

                    if (item.Quantity <= 0)
                    {
                        return Result<SaleResponseDto>.Failure($"Quantity for product {product.Name} must be greater than zero");
                    }

                    if (product.StockQuantity == 0)
                    {
                        return Result<SaleResponseDto>.Failure($"Product {product.Name} is out of stock");
                    }

                    if (product.StockQuantity < item.Quantity)
                    {
                        return Result<SaleResponseDto>.Failure($"Insufficient stock for product {product.Name}");
                    }


                    decimal subTotalAmount = product.SellingPrice * item.Quantity;
                    totalAmount += subTotalAmount;

                    var saleItem = new SaleItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        SubTotal = subTotalAmount,
                        UnitPrice = product.SellingPrice,
                    };

                    saleItems.Add(saleItem);

                    product.StockQuantity -= item.Quantity;

                }

                var sale = new Sale
                {
                    InvoiceNo = generatedInvoiceNo,
                    SaleDate = DateTime.UtcNow.AddHours(6).AddMinutes(30),
                    UserId = 1,
                    TotalAmount = totalAmount,
                    SaleItems = saleItems,
                };

                await _salesRepository.CreateSaleAsync(sale);

                return Result<SaleResponseDto>.Success(new SaleResponseDto { Id = sale.Id, InvoiceNumber = generatedInvoiceNo });


            }
            catch (Exception ex)
            {
                return Result<SaleResponseDto>.Failure($"An error occurred while creating the sale: {ex.Message}");
            }

        }

        public async Task<Result<List<SaleDto>>> GetAllSalesAsync()
        {
            try
            {
                var result = await _salesRepository.GetAllSalesAsync();

               

                var sales = result.Select(s => new SaleDto
                {
                    Id = s.Id,
                    InvoiceNo = s.InvoiceNo,
                    SaleDate = s.SaleDate,
                    TotalAmount = s.TotalAmount,
                    Items = s.SaleItems.Select(i => new SaleItemDto
                    {
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        SubTotal = i.SubTotal,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                }).ToList();

                return Result<List<SaleDto>>.Success(sales);

            }
            catch (Exception ex)
            {
                return Result<List<SaleDto>>.Failure($"An error occurred while retrieving sales: {ex.Message}");
            }
        }


        public async Task<Result<SaleDto>> GetSaleByIdAsync(int id)
        {
            try
            {
                var result = await _salesRepository.GetSaleByIdAsync(id);
               
                var sale = new SaleDto
                {
                    Id = result!.Id,
                    InvoiceNo = result.InvoiceNo,
                    SaleDate = result.SaleDate,
                    TotalAmount = result.TotalAmount,
                    Items = result.SaleItems.Select(i => new SaleItemDto
                    {
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        SubTotal = i.SubTotal,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };
                return Result<SaleDto>.Success(sale);
            }
            catch (Exception ex)
            {
                return Result<SaleDto>.Failure($"An error occurred while retrieving the sale: {ex.Message}");
            }
        }
    }
}
