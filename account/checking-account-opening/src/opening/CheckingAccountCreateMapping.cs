public class CheckingAccountCreateMapping : Amorphie.IMapping
{
    public IActionResult RequestHandler(
            Request downRequest,
            Request upRequest,
            Runtime runtime,
            Dictionary<string, Definition> definitions,
            Instance? instance,
            )
    {
        upRequest.url.RouteParameters["tckn"] = instance.data.attributes.customer.citizenshipNumber;

        var requestBody = new
        {
            accountName = instance.data.attributes.accountName,
            currency = instance.data.attributes.currency"
        };

        string jsonContent = JsonSerializer.Serialize(requestBody);

         upRequest.HttpRequestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

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
        instance.data.attributes.responses.openingResponse = upResponse.HttpResponseMessage.StatusCode;

        if (upResponse.HttpResponseMessage.Content != null)
        {
            var content = await upResponse.HttpResponseMessage.Content.ReadAsStringAsync();

            dynamic responseBody = JsonSerializer.Deserialize<dynamic>(content);

            instance.key = responseBody.IBAN;
            instance.data.attributes.account.iban = responseBody.IBAN;
            instance.data.attributes.account.prefix = responseBody.account.prefix;
            instance.data.attributes.account.number = responseBody.account.number;
            instance.data.attributes.account.suffix = responseBody.account.suffix;
        }
        
        return Ok();

    }
}