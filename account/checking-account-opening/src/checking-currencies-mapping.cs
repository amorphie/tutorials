public class CheckingCurrenciesMapping : Amorphie.Task.IMapping
{
    public IActionResult RequestHandler(
            Request downRequest,
            Request upRequest,
            Runtime runtime,
            Dictionary<string, Definition> definitions,
            Instance? instance,
            )
    {
        if (downRequest.HttpRequestMessage.Headers.AcceptLanguage != null && downRequest.HttpRequestMessage.Headers.AcceptLanguage.Count != 0)
        {
            var acceptLanguageValues = string.Join(",", downRequest.Headers.AcceptLanguage.Select(lang => lang.ToString()));
            upRequest.HttpRequestMessage.Headers.Add("Accept-Language", acceptLanguageValues);
        }

        if (downRequest.HttpRequestMessage.Headers.TryGetValues("User", out var userValues))
        {
            upRequest.HttpRequestMessage.Headers.TryAddWithoutValidation("User", userValues.FirstOrDefault());
        }

        if (downRequest.HttpRequestMessage.Headers.TryGetValues("Scope", out var scopeValues))
        {
            upRequest.HttpRequestMessage.Headers.TryAddWithoutValidation("Customer", scopeValues.FirstOrDefault());
        }
        return Ok();
    }

    public IActionResult ResponseHandler(
           Response downResponse,
           Response upResponse,
           Runtime runtime,
           Dictionary<string, Definition> definitions,
           Instance? instance,

           )
    {
        var downResponse.HttpResponseMessage = new HttpResponseMessage(upResponse.StatusCode);

        if (upResponse.HttpResponseMessage.Content != null)
        {
            var content = await upResponse.HttpResponseMessage.Content.ReadAsStringAsync();
            downResponse.HttpResponseMessage.Content = new StringContent(content);
        }

        return Ok();

    }
}