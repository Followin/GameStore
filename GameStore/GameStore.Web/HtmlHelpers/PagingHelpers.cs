﻿using System;
using System.Text;
using System.Web.Mvc;
using GameStore.Web.Models;

namespace GameStore.Web.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(
            this HtmlHelper html,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl)
        {
            var result = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");
                result.Append(tag);
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString PageLinks(
             this HtmlHelper html,
             PagingInfo pagingInfo,
             Func<int, MvcHtmlString> pageUrl)
        {
            var result = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                result.Append(pageUrl(i));
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }

}