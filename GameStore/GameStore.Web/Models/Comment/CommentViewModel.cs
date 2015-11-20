using System;
using System.Collections.Generic;

namespace GameStore.Web.Models.Comment
{
    public class CommentViewModel
    {
        public int GameId { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<DisplayCommentViewModel> Comments { get; set; }

        public CreateCommentViewModel CreateModel { get; set; }
    }
}