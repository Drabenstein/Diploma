namespace Core.Models.Reviews.ValueObjects;
public static class DefaultReviewModules
{
    private const string Purpose = "Cel i zakres pracy";
    private const string Structure = "Struktura pracy";
    private const string Design = "Część analityczno projektowa";
    private const string Sources = "Źródła i redakcja pracy";
    private const string Assessment = "Ocena";
    public static IEnumerable<ReviewModuleTemplate> GetModuleTemplates()
    {
        return new List<ReviewModuleTemplate>()
        {
            new ReviewModuleTemplate
            {
                Name = Purpose,
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = Purpose,
                Type = ReviewModuleType.Number,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = Structure,
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = Structure,
                Type = ReviewModuleType.Number,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = Design,
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = Design,
                Type = ReviewModuleType.Number,
                Value = ""
            },
             new ReviewModuleTemplate
            {
                Name = Sources,
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = Sources,
                Type = ReviewModuleType.Number,
                Value = ""
            },
             new ReviewModuleTemplate
            {
                Name = Assessment,
                Type = ReviewModuleType.Number,
                Value = ""
            }
        };
    }
}

