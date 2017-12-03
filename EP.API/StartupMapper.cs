using EP.API.Areas.Admin.ViewModels.News;
using EP.Data.Entities.News;
using ExpressMapper;

namespace EP.API
{
    public static class StartupMapper
    {
        public static void RegisterMapping()
        {
            NewsMapping();

            Mapper.Compile();
        }

        private static void NewsMapping()
        {
            Mapper.Register<NewsViewModel, NewsItem>()
                .Member(dest => dest.Title, src => src.Title.Trim())
                .Member(dest => dest.ShortContent, src => src.ShortContent.TrimNull())
                .Member(dest => dest.FullContent, src => src.FullContent.Trim());
        }

        private static string TrimNull(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim();
        }
    }
}
