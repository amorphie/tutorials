public class OpeningSuccessRule : Amorphie.IRule
{
    public bool Execute(
            Runtime runtime,
            Dictionary<string, Definition> definitions,
            Instance instance,
            )
    {
        if (instance.data.attributes.responses.openingResponse == 200)
            return true;
        else
            return false;
    }
}
