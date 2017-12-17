﻿using EP.API.Areas.Admin.ViewModels.Emails;
using EP.API.Areas.Admin.ViewModels.News;
using EP.Data.Entities.Emails;
using EP.Data.Entities.News;
using EP.Services.Emails;
using ExpressMapper;

namespace EP.API
{
    public static class StartupMapper
    {
        public static void RegisterMapping()
        {
            EmailsMapping();
            NewsMapping();

            Mapper.Compile();
        }

        private static void EmailsMapping()
        {
            Mapper.Register<EmailAccount, EmailSetting>();
            Mapper.Register<EmailAccountViewModel, EmailAccount>()
                .Member(dest => dest.Email, src => src.Email.Trim())
                .Member(dest => dest.DisplayName, src => src.DisplayName.TrimNull())
                .Member(dest => dest.Host, src => src.Host.Trim())
                .Member(dest => dest.UserName, src => src.UserName.Trim())
                .Member(dest => dest.Host, src => src.Password.Trim());
        }

        private static void NewsMapping()
        {
            Mapper.Register<NewsViewModel, NewsItem>()
                .Member(dest => dest.Title, src => src.Title.Trim())
                .Member(dest => dest.Ingress, src => src.Ingress.TrimNull())
                .Member(dest => dest.Content, src => src.Content.Trim());
        }

        private static string TrimNull(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim();
        }
    }
}
