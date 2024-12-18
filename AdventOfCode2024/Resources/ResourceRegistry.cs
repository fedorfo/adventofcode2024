namespace AdventOfCode2024.Resources;

public class ResourceRegistry
{
    public static Stream GetResourceStream(string filename)
    {
        var resourceName = $"{typeof(ResourceRegistry).Namespace}.{filename}";
        return typeof(ResourceRegistry).Assembly.GetManifestResourceStream(resourceName) ??
               throw new ArgumentException($"Resource {resourceName} was not found.");
    }
}
