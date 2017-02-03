using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using BlogApplication.Data.General;
using BlogApplication.Data.Translation;
using BlogApplication.Framework.Attribute;
using BlogApplication.Framework.Configuration;
using BlogApplication.Framework.Extensions.ListExtensions;
using BlogApplication.Framework.ResultHelper;


namespace BlogApplication.WebFramework.HtmlExtensions
{
    public static class TranslationExtension
    {

        public static MvcHtmlString GetWord<TModel>(this HtmlHelper<TModel> htmlHelper, string Key)
        {
            if (!string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Key.Trim()))
            {
                Controllers.BaseController Controller = (Controllers.BaseController) htmlHelper.ViewContext.Controller;
                CollectionResult<Common> Result = Controller.Client.Services.ServiceController.Translation.GetTranslationsOfTheKey(Key);
                if (Result.HasFailed)
                {
                    Controller.Client.Services.ServiceController.Translation.AddTranslation(Key);
                    var TranslationResult = Controller.Client.Services.ServiceController.Translation.GetTranslationsOfTheKey(Key)
                        .Data.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault();
                    return new MvcHtmlString(TranslationResult.Translation);
                }

                else
                {
                    if (Result.Data.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    {
                        var translation = Result.Data.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault();
                        return new MvcHtmlString(translation.Translation);
                    }
                    else
                    {
                        var addNewLanguage =
                            Controller.Client.Services.ServiceController.Translation.AddTranslationToOneLanguage(Key,
                                Controller.Client.CurrentLanguageID).Data;
                        return new MvcHtmlString(addNewLanguage.Translation);
                    }
                }

            }
            return new MvcHtmlString(Key);
        }

        public static MvcHtmlString GetTranslatedEnumName<TModel>(this HtmlHelper<TModel> htmlHelper, object modelData,
            Type typeToDrawEnum)
        {

            var mainResult = Enum.GetName(typeToDrawEnum, modelData);
            var memInfo = typeToDrawEnum.GetMember(mainResult);

            var displayAttributeType = typeof(AttributeHelper);

            var attributes =
                (AttributeHelper)
                    memInfo[0].GetCustomAttributes(typeof (AttributeHelper), false)
                        .FirstOrDefault();

            return htmlHelper.GetWord(attributes != null ? attributes.AttributeValue.ToString() : mainResult);
        }

        public static MvcHtmlString GetLanguageName<TModel>(this HtmlHelper<TModel> htmlHelper, long ID)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            ObjectResult<Language> Result = Controller.Client.Services.ServiceController.General.Language.GetLanguage(ID);

            if (Result.Data != null)
            {
                return new MvcHtmlString(Result.Data.Name);
            }

            return new MvcHtmlString("");
        }


    }
}