using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace SimpleEchoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //AutoMapper.Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<Microsoft.Bot.Connector.IMessageActivity, dataaccess.OnboardingTask>()
            //        .ForMember(dest => dest.FromId, opt => opt.MapFrom(src => src.From.Id))
            //        .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.Recipient.Id))
            //        .ForMember(dest => dest.FromName, opt => opt.MapFrom(src => src.From.Name))
            //        .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.Recipient.Name));
            //});
        }
    }
}
