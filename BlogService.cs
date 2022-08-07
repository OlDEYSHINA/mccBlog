using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    static class BlogService
    {
        public static string NumberOfCommentsPerUser(MyDbContext context)
        {
            var resultList = context.BlogComments.GroupBy(x => x.UserName)
                .Select(g => new {Name = g.Key, Count = g.Count()})
                .OrderBy(j => j.Name).ToList();

            var sb = new StringBuilder(64);

            foreach (var user in resultList)
            {
                sb.Append(user.Name);
                sb.Append(": ");
                sb.Append(user.Count);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string PostsOrderedByLastCommentDate(MyDbContext context)
        {
            var resultList = context.BlogPosts.Select(p => new
            {
                Title = p.Title,
                LastComment = p.Comments.OrderByDescending(c => c.CreatedDate).Select(c => new
                {
                    CreatedDate = c.CreatedDate,
                    Text = c.Text
                }).FirstOrDefault()
            }).OrderByDescending(p => p.LastComment.CreatedDate).ToList();

            var sb = new StringBuilder(64);

            foreach (var user in resultList)
            {
                sb.Append(user.Title);
                sb.Append(": '");
                sb.Append(user.LastComment.Text);
                sb.Append("', ");
                sb.Append(user.LastComment.CreatedDate);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string NumberOfLastCommentsLeftByUser(MyDbContext context)
        {
            var lastComments = context.BlogPosts
                .Select(p => p.Comments.OrderByDescending(c => c.CreatedDate).FirstOrDefault()).ToList();

            var countLastCommentsOfUser = lastComments.GroupBy(c => c.UserName)
                .Select(g => new
                {
                    UserName = g.Key,
                    Count = g.Count()
                }).ToList();

            var sb = new StringBuilder(64);

            foreach (var user in countLastCommentsOfUser)
            {
                sb.Append(user.UserName);
                sb.Append(": ");
                sb.Append(user.Count);
                sb.AppendLine();
            }
            
            return sb.ToString();
        }
    }
}
