using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Application.UseCases
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IForumRepository _forumRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _forumRepository = Injector.CreateInstance<IForumRepository>();
        }
        public void Delete(Comment comment)
        {
            _commentRepository.Delete(comment);
        }

        public List<Comment> GetAll()
        {
            List<Comment> comments = _commentRepository.GetAll();
            comments = GetForumData(comments);
            return comments;
        }
        public List<Comment> GetForumData(List<Comment> comments)
        {
            List<Forum> forums = _forumRepository.GetAll();
            foreach (Comment comment in comments)
            {
                comment.Forum = forums.Find(f => f.Id == comment.Forum.Id);
            }
            return comments;
        }

        public List<Comment> FindByForumId(int id)
        {
            List<Comment> comments = GetAll();
            comments = GetForumData(comments);
            List<Comment> foundedComments = new List<Comment>();
            foreach(Comment comment in comments)
            {
                if(comment.Forum.Id == id)
                {
                    foundedComments.Add(comment);
                }
            }
            return foundedComments;
        }

        public Comment Save(Comment comment)
        {
            return _commentRepository.Save(comment);
        }

        public void Update(Comment comment)
        {
            _commentRepository.Update(comment);
        }
    }
}