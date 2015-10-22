﻿using System;
using System.Collections.Generic;

namespace GameStore.Web.Models.Comment
{
    public class CommentViewModel
    {
        public IEnumerable<DisplayCommentViewModel> Comments { get; set; }
        public CreateCommentViewModel CreateModel { get; set; }
    }
}