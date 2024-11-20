using System.Globalization;

namespace SupermarketReceipt
{
    public class Offer
    {
        private readonly SpecialOfferType offerType;
        private readonly double argument;

        public Offer(SpecialOfferType offerType, double argument)
        {
            this.offerType = offerType;
            this.argument = argument;
        }

        public Discount GetDiscount(Product p, double unitPrice, double quantity, CultureInfo culture)
        {
            int quantityAsInt = (int)quantity;

            if (offerType == SpecialOfferType.TwoForAmount)
            {
                if (quantityAsInt >= 2)
                {
                    int numberOfXs = quantityAsInt / 2;
                    double total = argument * numberOfXs + quantityAsInt % 2 * unitPrice;
                    double discount = unitPrice * quantity - total;
                    return new Discount(p, "2 for " + PrintPrice(argument, culture), -discount);
                }
            }

            if (offerType == SpecialOfferType.ThreeForTwo)
            {
                if (quantityAsInt >= 3)
                {
                    int numberOfXs = quantityAsInt / 3;
                    double discountAmount = quantity * unitPrice
                        - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                    return new Discount(p, "3 for 2", -discountAmount);
                }
            }

            if (offerType == SpecialOfferType.TenPercentDiscount)
            {
                return new Discount(p, argument + "% off", -quantity * unitPrice * argument / 100.0);
            }

            if (offerType == SpecialOfferType.FiveForAmount)
            {
                if (quantityAsInt >= 5)
                {
                    int numberOfXs = quantityAsInt / 5;
                    double discountTotal =
                        unitPrice * quantity - (argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                    return new Discount(p, 5 + " for " + PrintPrice(argument, culture), -discountTotal);
                }
            }

            return null;
        }

        private static string PrintPrice(double price, CultureInfo culture)
        {
            return price.ToString("N2", culture);
        }
    }

    public enum SpecialOfferType
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwoForAmount,
        FiveForAmount,
    }
}