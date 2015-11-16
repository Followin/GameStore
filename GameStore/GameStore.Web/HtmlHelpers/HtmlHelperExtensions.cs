using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GameStore.Static;
using GameStore.Web.Models;
using GameStore.Web.Models.Comment;
using GameStore.Web.Models.Genres;

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
                commentDiv.Attributes["data-id"] = comment.Id.ToString();
                
                var header = new TagBuilder("h3");
                header.AddCssClass("author-name");
                header.SetInnerText(comment.Name);

                

                var body = new TagBuilder("div");
                body.AddCssClass("comment-body");
                body.SetInnerText(comment.Body);
                body.InnerHtml = comment.Quotes + body.InnerHtml;

                var commentButtonsDiv = new TagBuilder("div");
                commentButtonsDiv.AddCssClass("comment-buttons");

                var answerLink = new TagBuilder("a");
                answerLink.SetInnerText("Answer");
                answerLink.AddCssClass("comment-button answer-to-comment");
                answerLink.Attributes["data-id"] = comment.Id.ToString();

                var quoteLink = new TagBuilder("a");
                quoteLink.SetInnerText("Quote");
                quoteLink.AddCssClass("comment-button quote-comment");
                quoteLink.Attributes["data-id"] = comment.Id.ToString();

                commentButtonsDiv.InnerHtml += answerLink;
                commentButtonsDiv.InnerHtml += quoteLink;

                var deleteButton = new TagBuilder("button");
                deleteButton.AddCssClass("delete-comment-button");
                deleteButton.Attributes["data-id"] = comment.Id.ToString();
                deleteButton.SetInnerText("x");

                if ((HttpContext.Current.User as ClaimsPrincipal).HasClaim(ClaimTypesExtensions.CommentPermission, Permissions.Delete))
                {
                    commentDiv.InnerHtml += deleteButton;
                }
                commentDiv.InnerHtml += header;
                commentDiv.InnerHtml += body;
                commentDiv.InnerHtml += commentButtonsDiv;
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