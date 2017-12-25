using MongoDB.Bson;
using MongoDB.Driver;

namespace EP.Data.Extensions
{
    public static class MongoDbExtensions
    {
        public static bool IsInvalidObjectId(this string id)
        {
            return string.IsNullOrWhiteSpace(id) || !ObjectId.TryParse(id, out ObjectId objectId);
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
