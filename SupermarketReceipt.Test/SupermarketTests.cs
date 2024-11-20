using System.Collections.Generic;
using Xunit;

namespace SupermarketReceipt.Test
{
    public class SupermarketTests
    {
        private readonly Product toothPaste;
        private readonly Product toothbrush;
        private readonly Product apples;
        private readonly ShoppingCart cart;
        private readonly SupermarketCatalog catalog;
        private readonly Teller teller;

        public SupermarketTests()
        {
            toothPaste = new Product("toothpaste", ProductUnit.Each);
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

        [Fact]
        public void FiveForOne_Valid()
        {
            cart.AddItemQuantity(apples, 5);
            teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, apples, 9.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(9.0, receipt.GetTotalPrice());
        }

        [Fact]
        public void FiveForOne_TooFew()
        {
            cart.AddItemQuantity(apples, 4);
            teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, apples, 9.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(7.96, receipt.GetTotalPrice());
        }

        [Fact]
        public void FiveForOne_TooMuch()
        {
            cart.AddItemQuantity(apples, 6);
            teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, apples, 9.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(10.99, receipt.GetTotalPrice());
        }

        [Fact]
        public void TenPercentOff()
        {
            cart.AddItemQuantity(toothbrush, 10.0);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(8.91, receipt.GetTotalPrice());
        }

        [Fact]
        public void ThreeForTwo()
        {
            cart.AddItemQuantity(toothbrush, 3.0);
            teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, toothbrush, 0.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(1.98, receipt.GetTotalPrice());
        }

        [Fact]
        public void ThreeForTwo_TooFew()
        {
            cart.AddItemQuantity(toothbrush, 2.0);
            teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, toothbrush, 0.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(1.98, receipt.GetTotalPrice());
        }

        [Fact]
        public void ThreeForTwo_TooMuch()
        {
            cart.AddItemQuantity(toothbrush, 5.0);
            teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, toothbrush, 0.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(3.96, receipt.GetTotalPrice());
        }

        [Fact]
        public void ThreeForTwo_Double()
        {
            cart.AddItemQuantity(toothbrush, 6.0);
            teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, toothbrush, 0.0);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(3.96, receipt.GetTotalPrice());
        }

        [Fact]
        public void TwoForAmount()
        {
            cart.AddItemQuantity(toothbrush, 2.0);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, toothbrush, 1.5);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(1.5, receipt.GetTotalPrice());
        }

        [Fact]
        public void TwoForAmount_TooFew()
        {
            cart.AddItemQuantity(toothbrush, 1.0);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, toothbrush, 1.5);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(.99, receipt.GetTotalPrice());
        }

        [Fact]
        public void TwoForAmount_TooMuch()
        {
            cart.AddItemQuantity(toothbrush, 3.0);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, toothbrush, 1.5);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(1.5 + .99, receipt.GetTotalPrice());
        }

        [Fact]
        public void TwoForAmount_Double()
        {
            cart.AddItemQuantity(toothbrush, 4.0);
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, toothbrush, 1.5);

            Receipt receipt = teller.ChecksOutArticlesFrom(cart);

            Assert.Equal(3.0, receipt.GetTotalPrice());
        }
        
        [Fact(Skip = "Not implemented yet")]
        public void Combo()
        {
            cart.AddItemQuantity(toothbrush, 1.0);
            cart.AddItemQuantity(toothPaste, 2.0);
            
            // How do I do this?
            // teller.AddSpecialOffer(toothbrush, new ComboOffer(toothPaste, 2.0));
            
            Receipt receipt = teller.ChecksOutArticlesFrom(cart);
            Assert.Equal(2.0, receipt.GetTotalPrice());
        }
        
        private void PopulateCatalog()
        {
            catalog.AddProduct(toothPaste, 1.0);
            catalog.AddProduct(toothbrush, 0.99);
            catalog.AddProduct(apples, 1.99);
        }
    }
}