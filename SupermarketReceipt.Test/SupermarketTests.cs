using System.Collections.Generic;
using Xunit;

namespace SupermarketReceipt.Test
{
    public class SupermarketTests
    {
        private readonly Product toothbrush;
        private readonly Product apples;
        private readonly ShoppingCart cart;
        private readonly SupermarketCatalog catalog;
        private readonly Teller teller;

        public SupermarketTests()
        {
            toothbrush = new Product("toothbrush", ProductUnit.Each);
            apples = new Product("apples", ProductUnit.Kilo);

            cart = new ShoppingCart();
            catalog = new FakeCatalog();
            teller = new Teller(catalog);

            PopulateCatalog();
        }

        [Fact]
        public void TenPercentDiscount()
        {
            // ARRANGE
            cart.AddItemQuantity(apples, 2.5);

            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(4.975, receipt.GetTotalPrice());
            Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
            Assert.Single(receipt.GetItems());
            ReceiptItem receiptItem = receipt.GetItems()[0];
            Assert.Equal(apples, receiptItem.Product);
            Assert.Equal(1.99, receiptItem.Price);
            Assert.Equal(2.5 * 1.99, receiptItem.TotalPrice);
            Assert.Equal(2.5, receiptItem.Quantity);
        }

        private void PopulateCatalog()
        {
            catalog.AddProduct(toothbrush, 0.99);
            catalog.AddProduct(apples, 1.99);
        }
    }
}