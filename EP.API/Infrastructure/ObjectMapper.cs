using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Areas.Admin.ViewModels.Emails;
using EP.API.Areas.Admin.ViewModels.News;
using EP.Data.Entities.Blobs;
using EP.Data.Entities.Emails;
using EP.Data.Entities.News;
using EP.Services.Extensions;
using ExpressMapper;
using System;

namespace EP.API.Infrastructure
{
    public static class ObjectMapper
    {
        public static void RegisterMapping()
        {
            EmailMapping();
            BlobMapping();
            NewsMapping();

            Mapper.Compile();
        }

        private static void EmailMapping()
        {
            Mapper.Register<EmailAccountViewModel, EmailAccount>()
                .Member(dest => dest.Email, src => src.Email.Trim())
                .Member(dest => dest.DisplayName, src => src.DisplayName.TrimNull())
                .Member(dest => dest.Host, src => src.Host.Trim())
                .Member(dest => dest.UserName, src => src.UserName.Trim())
                .Member(dest => dest.Host, src => src.Password.Trim());
        }

        private static void BlobMapping()
        {
            Mapper.Register<FolderViewModel, Blob>()
                .Member(dest => dest.Name, src => src.Name.Trim())
                .Member(dest => dest.Parent, src => src.Parent.Trim());
        }

        private static void NewsMapping()
        {
            Mapper.Register<NewsViewModel, NewsItem>()
                .Member(dest => dest.Title, src => src.Title.Trim())
                .Member(dest => dest.Ingress, src => src.Ingress.TrimNull())
                .Member(dest => dest.Content, src => src.Content.Trim())
                .Function(dest => dest.PublishedDate, src =>
                {
                    return src.Published ? DateTime.UtcNow : new DateTime?();
                });
        }
    }
}
