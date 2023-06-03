using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class ForumService
    {
        private readonly IForumRepository _forumRepository;
        private readonly LocationService _locationService;
        private readonly CommentService _commentService;

        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
        }
        public void Delete(Forum forum)
        {
            _forumRepository.Delete(forum);
        }

        public List<Forum> GetAll()
        {
            List<Forum> forums = _forumRepository.GetAll();
            forums = GetLocationData(forums);
            forums = GetCommentsData(forums);
            forums = GetNumberOfComments(forums);
            return forums;
        }
        public List<Forum> GetNumberOfComments(List<Forum> forums)
        {
            foreach (Forum forum in forums)
            {
                int count = 0;
                foreach(Comment comment in forum.Comments)
                {
                    count++;
                }
                forum.NumberOfComments = count;
            }
            return forums;
        }
        public List<Forum> GetCommentsData(List<Forum> forums)
        {
            List<Comment> comments = _commentService.GetAll();
            foreach(Forum forum in forums)
            {
                foreach(Comment comment in comments)
                {
                    if(comment.Forum.Id == forum.Id)
                    {
                        forum.Comments.Add(comment);
                    }
                }
            }
            return forums;
        }
        public List<Forum> GetLocationData(List<Forum> forums)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (Forum forum in forums)
            {
                forum.Location = locations.Find(l => l.Id == forum.Location.Id);
            }
            return forums;
        }

        public Forum Save(Forum forum)
        {
            return _forumRepository.Save(forum);
        }

        public void Update(Forum forum)
        {
            _forumRepository.Update(forum);
        }

        public List<IGrouping<Location, Forum>> GetForumsByLocation()
        {
            List<Forum> forums = GetAll();
            var forumsByLocation = forums.GroupBy(f => f.Location).ToList();
            return forumsByLocation;
        }
    }
}