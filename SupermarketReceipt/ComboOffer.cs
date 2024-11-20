using System.Linq;

namespace SupermarketReceipt;

public class ComboOffer(Product requiredProduct, double requiredQuantity)
{
    public Discount GetDiscount(Product p, double unitPrice, double quantity, ShoppingCart shoppingCart)
    {
        ProductQuantity required = shoppingCart
            .GetItems()
            .FirstOrDefault(p2 => p2.Product.Equals(requiredProduct));

        if (required == null || required.Quantity < requiredQuantity)
        {
            return null;
        }

        int maxFreeItems = (int)(required.Quantity / requiredQuantity);

        int freeItems = int.Min((int)quantity, maxFreeItems);
        return new Discount(p, "", -unitPrice * freeItems);
    }
}