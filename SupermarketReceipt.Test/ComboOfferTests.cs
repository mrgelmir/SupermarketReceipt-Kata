using Xunit;

namespace SupermarketReceipt.Test;

public class ComboOfferTests
{
    private const double ToothBrushPrice = .99;

    private readonly Product toothbrush;
    private readonly Product toothPaste;
    private readonly ShoppingCart shoppingCart;

    public ComboOfferTests()
    {
        toothbrush = new Product("toothBrush", ProductUnit.Each);
        toothPaste = new Product("toothPaste", ProductUnit.Each);
        shoppingCart = new ShoppingCart();
    }

    [Fact]
    public void GetDiscount_ShouldBeNull_WhenRequiredProductIsMissing()
    {
        ComboOffer comboOffer = new(toothPaste, 2.0);

        Discount discount = comboOffer.GetDiscount(toothbrush, ToothBrushPrice, 1.0, shoppingCart);

        Assert.Null(discount);
    }

    [Fact]
    public void GetDiscount_ShouldNOtBeNull_WhenRequiredProductIsAdded()
    {
        ComboOffer comboOffer = new(toothPaste, 2.0);
        shoppingCart.AddItemQuantity(toothPaste, 2.0);

        Discount discount = comboOffer.GetDiscount(toothbrush, ToothBrushPrice, 1.0, shoppingCart);

        Assert.NotNull(discount);
    }

    [Fact]
    public void GetDiscount_ShouldBeNull_WhenRequiredAmountIsNotMet()
    {
        ComboOffer comboOffer = new(toothPaste, 2.0);
        shoppingCart.AddItemQuantity(toothPaste, 1.0);

        Discount discount = comboOffer.GetDiscount(toothbrush, ToothBrushPrice, 1.0, shoppingCart);

        Assert.Null(discount);
    }

    [Fact]
    public void GetDiscount_ShouldBeFullPrice()
    {
        ComboOffer comboOffer = new(toothPaste, 2.0);
        shoppingCart.AddItemQuantity(toothPaste, 2.0);

        Discount discount = comboOffer.GetDiscount(toothbrush, ToothBrushPrice, 1.0, shoppingCart);

        Assert.Equal(ToothBrushPrice, -discount.DiscountAmount);
    }

    [Fact]
    public void GetDiscount_ShouldAllowMultiple()
    {
        ComboOffer comboOffer = new(toothPaste, 2.0);
        shoppingCart.AddItemQuantity(toothPaste, 4.0);

        Discount discount = comboOffer.GetDiscount(toothbrush, ToothBrushPrice, 2.0, shoppingCart);

        Assert.Equal(2 * ToothBrushPrice, -discount.DiscountAmount);
    }
    
    [Fact]
    public void GetDiscount_IsCappedByRequiredQuantity()
    {
        ComboOffer comboOffer = new(toothPaste, 2.0);
        shoppingCart.AddItemQuantity(toothPaste, 4.0);

        Discount discount = comboOffer.GetDiscount(toothbrush, ToothBrushPrice, 2.0, shoppingCart);

        Assert.Equal(2 * ToothBrushPrice, -discount.DiscountAmount);
    }
    
    [Fact]
    public void GetDiscount_ShouldNotAddFreeMoney()
    {
        ComboOffer comboOffer = new(toothPaste, 2.0);
        shoppingCart.AddItemQuantity(toothPaste, 4.0);

        Discount discount = comboOffer.GetDiscount(toothbrush, ToothBrushPrice, 1.0, shoppingCart);

        Assert.Equal(ToothBrushPrice, -discount.DiscountAmount);
    }
}