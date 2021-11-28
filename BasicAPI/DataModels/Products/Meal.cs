using LiteDB;

namespace DataModels.Products
{
    public class Meal
    {
        [BsonId]
        public string Title { get; set; }
        public bool IsTitleValid()
        {
            var output = !string.IsNullOrEmpty(Title) &&
                         Title.Length <= 50;
            return output;
        }

        public string Description { get; set; }
        public bool IsDescriptionValid()
        {
            var output = !string.IsNullOrEmpty(Description) &&
                         Description.Length <= 250;
            return output;
        }

        public double Price { get; set; }
        public bool IsPriceValid()
        {
            var output = Price > 0;
            return output;
        }

        public string[] Ingredients { get; set; }
        public bool IsIngredientsValid()
        {
            var output = Ingredients != null &&
                         Ingredients.Length < 30;
            return output;
        }

        public string Thumbnail { get; set; }
        public bool IsThumbnailValid()
        {
            var output = !string.IsNullOrEmpty(Thumbnail) &&
                        Thumbnail.Length < 250;
            return output;
        }

        public string MealType { get; set; }
        public bool IsMealTypeValid()
        {
            var output = !string.IsNullOrEmpty(MealType) &&
                         MealType.Length < 30;
            return output;
        }

        public string[] Tags { get; set; }
        public bool IsTagsValid()
        {
            var output = Tags != null &&
                         Tags.Length <= 5;
            return output;
        }

        public bool HasValidData()
        {
            var output = IsTitleValid() &&
                         IsDescriptionValid() &&
                         IsPriceValid() &&
                         IsIngredientsValid() &&
                         IsThumbnailValid() &&
                         IsMealTypeValid() &&
                         IsTagsValid();

            return output;
        }
    }
}
