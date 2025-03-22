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
            upRequest.HttpRequestMessageHeaders.Add("Accept-Language", acceptLanguageValues);
        }

        if (downRequest.HttpRequestMessageHeaders.TryGetValues("User", out var userValues))
        {
            upRequest.HttpRequestMessageHeaders.TryAddWithoutValidation("User", userValues.FirstOrDefault());
        }

        if (downRequest.HttpRequestMessageHeaders.TryGetValues("Scope", out var scopeValues))
        {
            upRequest.HttpRequestMessageHeaders.TryAddWithoutValidation("Customer", scopeValues.FirstOrDefault());
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