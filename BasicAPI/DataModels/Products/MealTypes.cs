using LiteDB;

namespace DataModels.Products
{
    public class MealTypes
    {
        [BsonId]
        public string Type { get; set; }
    }
}
