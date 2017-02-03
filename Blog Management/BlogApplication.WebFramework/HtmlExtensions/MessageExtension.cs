using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.WebFramework.HtmlExtensions
{
    public static class MessageExtension
    {
        public static MvcHtmlString ShowMessages<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            List<ResultMessage> Messages = htmlHelper.ViewBag.Messages;
            if (Messages != null && Messages.Count > 0)
            {
                var appended =
                    "<script> $('#ViewMessages').click(function(){ " +
                    "$.alert({" +
                    "title: '{title}'," +
                    "content: '{content}'" +
                    "});}); $('#ViewMessages').click(); </script>";
                var messages = "<ul>";
                foreach (var message in Messages)
                {
                    if (!message.Code.StartsWith("S") && !message.Code.StartsWith("E1"))
                        messages += "<li class='ErrorMessage'>" + htmlHelper.GetWord(message.Description) + "</li>";
                    else if (!message.Code.StartsWith("E1"))
                        messages += "<li class='SuccessMessage'>" + htmlHelper.GetWord(message.Description) + "</li>";
                    else
                        messages += "<li class='SuccessMessage'>" + message.Description.Replace("\n", "").Replace("\r", "") + "</li>";
                }
                messages += "</ul>";
                messages = messages.Replace("'", "\"");
                appended = appended.Replace("{content}", messages);
                appended = appended.Replace("{title}", htmlHelper.GetWord("Info").ToString());

                return new MvcHtmlString(appended);
            }
            return new MvcHtmlString("");
        }

        public static MvcHtmlString DateFormat<TModel>(this HtmlHelper<TModel> htmlHelper, string dateTime)
        {
            if (!string.IsNullOrEmpty(dateTime))
                return new MvcHtmlString(DateTime.Parse(dateTime).ToString("dd-MM-yyyy"));
            return new MvcHtmlString("");
        }
    }
}