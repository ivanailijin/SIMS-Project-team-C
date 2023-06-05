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
        private readonly UserService _userService;

        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
            _userService = new UserService(Injector.CreateInstance<IUserRepository>());
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
            forums = GetUserData(forums);
            forums = GetNumberOfComments(forums);
            return forums;
        }
        public List<Forum> GetNumberOfComments(List<Forum> forums)
        {
            foreach (Forum forum in forums)
            {
                int count = 0;
                foreach (Comment comment in forum.Comments)
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
            foreach (Forum forum in forums)
            {
                foreach (Comment comment in comments)
                {
                    if (comment.Forum.Id == forum.Id)
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

        public List<Forum> GetUserData(List<Forum> forums)
        {
            List<User> users = _userService.GetAll();
            foreach (Forum forum in forums)
            {
                forum.User = users.Find(u => u.Id == forum.User.Id);
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

        public List<Forum> FindByGuestId(int guestId)
        {
            List<Forum> foundForums = new List<Forum>();
            List<Forum> forums = GetAll();

            foreach (Forum forum in forums)
            {
                if (forum.User.Id == guestId)
                {
                    foundForums.Add(forum);
                }
            }
            return foundForums;
        }

        public Forum FindById(int forumId)
        {
            List<Forum> forums = GetAll();
            foreach (Forum forum in forums)
            {
                if (forum.Id == forumId)
                {
                    return forum;
                }
            }
            return null;
        }

        public Forum CloseForum(int forumId)
        {
            Forum forum = FindById(forumId);
            forum.Status = FORUMSTATUS.Closed;
            _forumRepository.Update(forum);
            return forum;
        }

        public bool IsUserForumOwner(int userId,  Forum forum) 
        {
            bool result = false;
            if (userId == forum.User.Id)
            {
                result = true;
            }
            return result;
        }
    }
}