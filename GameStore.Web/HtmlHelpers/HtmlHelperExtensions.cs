using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Web.Models;
using GameStore.Web.Models.Comment;

namespace GameStore.Web.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString CreateTree(this HtmlHelper helper, IEnumerable<GenreViewModel> genres, String propertyName)
        {
            var ulTag = new TagBuilder("ul");
            foreach (var genre in genres)
            {
                var liTag = new TagBuilder("li");

                var genreCheckBox = new TagBuilder("input");
                genreCheckBox.Attributes["type"] = "checkbox";
                genreCheckBox.Attributes["value"] = genre.Id.ToString();
                genreCheckBox.Attributes["id"] = "genre" + genre.Id;
                genreCheckBox.Attributes["name"] = propertyName;

                var labelFor = new TagBuilder("label");
                labelFor.Attributes["for"] = "genre" + genre.Id;
                labelFor.SetInnerText(genre.Name);

                liTag.InnerHtml += genreCheckBox;
                liTag.InnerHtml += labelFor;
                if (genre.ChildGenres != null && genre.ChildGenres.Any())
                {
                    liTag.InnerHtml += CreateTree(helper, genre.ChildGenres, propertyName);
                }

                ulTag.InnerHtml += liTag;
            }
            return new MvcHtmlString(ulTag.ToString());
        }

        public static MvcHtmlString CreateTree(this HtmlHelper helper, IEnumerable<DisplayCommentViewModel> comments)
        {
            var ulTag = new TagBuilder("ul");
            foreach (var comment in comments)
            {
                var liTag = new TagBuilder("li");

                var commentDiv = new TagBuilder("div");
                commentDiv.AddCssClass("game-comment");
                
                var header = new TagBuilder("h3");
                header.SetInnerText(comment.Name);

                var body = new TagBuilder("p");
                body.SetInnerText(comment.Body);

                commentDiv.InnerHtml += header;
                commentDiv.InnerHtml += body;
                liTag.InnerHtml += commentDiv;

                if (comment.ChildComments != null && comment.ChildComments.Any())
                {
                    liTag.InnerHtml += CreateTree(helper, comment.ChildComments);
                }

                ulTag.InnerHtml += liTag;
            }

            return new MvcHtmlString(ulTag.ToString());
        }
    }
}