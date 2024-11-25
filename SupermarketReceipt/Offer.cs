using System.Globalization;

namespace SupermarketReceipt
{
    public enum SpecialOfferType
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwoForAmount,
        FiveForAmount,
    }

    public class Offer
    {
        public Offer(SpecialOfferType offerType, double argument)
        {
            this.offerType = offerType;
            this.argument = argument;
        }

        private readonly SpecialOfferType offerType;
        private readonly double argument;

        public Discount GetDiscount(Product p, double unitPrice, double quantity, ShoppingCart shoppingCart,
            CultureInfo culture)
        {
            Discount discount = null;

            if (offerType == SpecialOfferType.TwoForAmount)
            {
                int quantityAsInt = (int)quantity;

                if (quantityAsInt >= 2)
                {
                    const int discountCountItems = 2;
                    double total = argument * (quantityAsInt / discountCountItems) + quantityAsInt % 2 * unitPrice;
                    double discountN = unitPrice * quantity - total;
                    discount = new Discount(p, "2 for " + PrintPrice(argument, culture), -discountN);
                }
            }

            if (offerType == SpecialOfferType.ThreeForTwo)
            {
                int quantityAsInt = (int)quantity;

                if (quantityAsInt > 2)
                {
                    const int discountCountItems = 3;
                    int numberOfXs = quantityAsInt / discountCountItems;
                    double discountAmount = quantity * unitPrice
                        - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                    discount = new Discount(p, "3 for 2", -discountAmount);
                }
            }

            if (offerType == SpecialOfferType.TenPercentDiscount)
            {
                discount = new Discount(p, argument + "% off", -quantity * unitPrice * argument / 100.0);
            }

            if (offerType == SpecialOfferType.FiveForAmount)
            {
                int quantityAsInt = (int)quantity;
                if (quantityAsInt >= 5)
                {
                    const int discountCountItems = 5;
                    int numberOfXs = quantityAsInt / discountCountItems;
                    double discountTotal =
                        unitPrice * quantity - (argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                    discount = new Discount(p, discountCountItems + " for " + PrintPrice(argument, culture),
                        -discountTotal);
                }
            }

            return discount;
        }

        private string PrintPrice(double price, CultureInfo culture)
        {
            return price.ToString("N2", culture);
        }
    }
}