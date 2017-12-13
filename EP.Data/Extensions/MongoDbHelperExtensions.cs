using MongoDB.Bson;
using MongoDB.Driver;

namespace EP.Data.Extensions
{
    public static class MongoDbHelperExtensions
    {
        public static bool IsInvalidObjectId(this string id)
        {
            ObjectId objectId;

            return string.IsNullOrWhiteSpace(id) || !ObjectId.TryParse(id, out objectId);
        }

        public static bool IsSuccess(this ReplaceOneResult result)
        {
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public static bool IsSuccess(this UpdateResult result)
        {
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public static bool IsSuccess(this DeleteResult result)
        {
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
