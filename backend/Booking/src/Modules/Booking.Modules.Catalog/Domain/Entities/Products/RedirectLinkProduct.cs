using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities.Products;

public class RedirectLinkProduct : Product
{
    private RedirectLinkProduct(
        string productSlug,
        int storeId,
        string storeSlug,
        string title,
        string clickToPay,
        decimal price,
        string link,
        ProductStyle productStyle,
        string? subtitle,
        string? description) : base(
        productSlug,
        storeId,
        storeSlug,
        title,
        clickToPay,
        price,
        ProductType.RedirectLink,
        productStyle,
        subtitle,
        description)
    {
    }


    public static RedirectLinkProduct Create(
        string productSlug,
        int storeId,
        string storeSlug,
        string link,
        string title,
        ProductStyle productStyle,
        string subtitle,
        string description,
        string clickToPay,
        decimal price)
    {
        var redirectLinkProduct = new RedirectLinkProduct(
            productSlug,
            storeId,
            storeSlug,
            title,
            clickToPay,
            price,
            link,
            productStyle,
            subtitle,
            description);
        return redirectLinkProduct;
    }
}