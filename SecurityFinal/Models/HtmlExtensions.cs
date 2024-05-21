using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlExtensions
{
    public static HtmlString LineBreaksToBr(this IHtmlHelper htmlHelper, string text)
    {
        var result = new HtmlString(text.Replace("\r\n", "<br />").Replace("\n", "<br />"));
        return result;
    }
}