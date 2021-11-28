using LiteDB;

namespace DataModels.Products
{
    public class MealTags
    {
        [BsonId]
        public string Tag { get; set; }
    }
}
