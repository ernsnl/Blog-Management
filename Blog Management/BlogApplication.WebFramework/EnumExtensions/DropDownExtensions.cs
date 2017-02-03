using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BlogApplication.Framework.Attribute;
using BlogApplication.WebFramework.HtmlExtensions;

namespace BlogApplication.WebFramework.EnumExtensions
{
    public static class DropDownExtensions
    {
        public static MvcHtmlString TranslatedDropDownList<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object modelData,
            Type typeToDrawEnum, bool isUserType = false , object htmlAttributes = null, bool useBlank = false, string blankText = "",
            string blankValue = "")
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            var source = Enum.GetValues(typeToDrawEnum);

            var displayAttributeType = typeof(AttributeHelper);

            var items = new List<SelectListItem>();

            if (useBlank)
            {
                items.Add(new SelectListItem()
                {
                    Text = blankText,
                    Value = blankValue,
                    Selected = Convert.ToString(modelData).Equals(blankValue)
                });
            }

            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                var attrs = (AttributeHelper)field.GetCustomAttributes(displayAttributeType, false).FirstOrDefault();
                object underlyingValue = Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));
                if (isUserType)
                {
                    if (Convert.ToInt32(underlyingValue) >= Controller.Client.Services.CurrentUser.UserType)
                        items.Add(new SelectListItem()
                        {
                            Selected = underlyingValue.ToString().Equals(modelData.ToString()),
                            Text =
                                (attrs != null
                                    ? htmlHelper.GetWord(attrs.AttributeValue.ToString()).ToString()
                                    : htmlHelper.GetWord(value.ToString()).ToString()),
                            Value = Convert.ToString(underlyingValue)
                        });
                }
                else
                {
                    items.Add(new SelectListItem()
                    {
                        Selected = underlyingValue.ToString().Equals(modelData.ToString()),
                        Text =
                                (attrs != null
                                    ? htmlHelper.GetWord(attrs.AttributeValue.ToString()).ToString()
                                    : htmlHelper.GetWord(value.ToString()).ToString()),
                        Value = Convert.ToString(underlyingValue)
                    });
                }
            }
            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        public static MvcHtmlString LanguageDropDownList<TModel>(this HtmlHelper<TModel> htmlHelper, string name,
            object htmlAttributes = null)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            var languageList = Controller.Client.Services.ServiceController.General.Language.GetLanguageList(1, Int32.MaxValue);

            var items = new List<SelectListItem>();

            foreach (var language in languageList.Data)
            {
                items.Add(
                new SelectListItem()
                {
                    Text = language.Name,
                    Selected = Controller.Client.CurrentLanguageID == language.ID,
                    Value = Convert.ToString(language.ID)
                }
            );
            }

            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        public static MvcHtmlString CategoryDropDownList<TModel>(this HtmlHelper<TModel> htmlHelper, string name, long modelData,
            object htmlAttributes = null)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            var categoryList = Controller.Client.Services.ServiceController.BlogContent.Category.GetCategoryList(1, Int32.MaxValue);
            var items = new List<SelectListItem>();
            foreach (var category in categoryList.Data)
            {
                if (category.CategoryTranslations.Count > 0)
                {
                    if (category.CategoryTranslations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID)
                            .Any())
                    {
                        var translation =
                            category.CategoryTranslations.Where(
                                op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault();
                        items.Add(new SelectListItem()
                        {
                            Text = translation.Translation,
                            Selected = modelData == category.ID,
                            Value = Convert.ToString(category.ID)
                        });
                    }
                    else
                    {
                        items.Add(new SelectListItem()
                        {
                            Text = category.Name,
                            Selected = modelData == category.ID,
                            Value = Convert.ToString(category.ID)
                        });
                    }
                }
                else
                {
                    items.Add(new SelectListItem()
                    {
                        Text = category.Name,
                        Selected = modelData == category.ID,
                        Value = Convert.ToString(category.ID)
                    });
                }
            }

            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }
    }
}