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
            OfferType = offerType;
            Argument = argument;
        }

        public SpecialOfferType OfferType { get; }
        public double Argument { get; }
    }
}