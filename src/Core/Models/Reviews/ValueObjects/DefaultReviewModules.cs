namespace Core.Models.Reviews.ValueObjects;
public static class DefaultReviewModules
{
    public static IEnumerable<ReviewModuleTemplate> GetModuleTemplates()
    {
        return new List<ReviewModuleTemplate>()
        {
            new ReviewModuleTemplate
            {
                Name = "Cel i zakres pracy",
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = "Cel i zakres pracy",
                Type = ReviewModuleType.Number,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = "Struktura pracy",
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = "Struktura pracy",
                Type = ReviewModuleType.Number,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = "Część analityczno projektowa",
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = "Część analityczno projektowa",
                Type = ReviewModuleType.Number,
                Value = ""
            },
             new ReviewModuleTemplate
            {
                Name = "Źródła i redakcja pracy",
                Type = ReviewModuleType.Text,
                Value = ""
            },
            new ReviewModuleTemplate
            {
                Name = "Źródła i redakcja pracy",
                Type = ReviewModuleType.Number,
                Value = ""
            },
             new ReviewModuleTemplate
            {
                Name = "Ocena",
                Type = ReviewModuleType.Number,
                Value = ""
            }
        };
    }
}

