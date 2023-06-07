using System.Collections.Generic;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.Applications.UseCases
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IForumRepository _forumRepository;
        private readonly UserService _userService;
        private readonly AccommodationService _accommodationService;
        private readonly GuestService _guestService;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _forumRepository = Injector.CreateInstance<IForumRepository>();
            _userService = new UserService(Injector.CreateInstance<IUserRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _guestService = new GuestService(Injector.CreateInstance<IGuestRepository>());
        }
        public void Delete(Comment comment)
        {
            _commentRepository.Delete(comment);
        }

        public List<Comment> GetAll()
        {
            List<Comment> comments = _commentRepository.GetAll();
            comments = GetForumData(comments);
            comments = GetUserData(comments);
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

        public List<Comment> GetUserData(List<Comment> comments)
        {
            List<User> users = _userService.GetAll();
            foreach (Comment comment in comments)
            {
                comment.User = users.Find(u => u.Id == comment.User.Id);
            }
            return comments;
        }

        public List<Comment> FindByForumId(int id)
        {
            List<Comment> comments = GetAll();
            comments = GetForumData(comments);
            comments = GetUserData(comments);
            comments = GetOwnerData(comments);
            comments = GetMarkedCommentData(comments);
            List<Comment> foundedComments = new List<Comment>();
            foreach (Comment comment in comments)
            {
                if (comment.Forum.Id == id)
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

        public List<Comment> GetOwnerData(List<Comment> comments)
        {
            foreach (Comment comment in comments)
            {
                if (comment.User.UserType == "Owner" && IsOwnersAccommodationOnLocation(comment.User, comment.Forum))
                {
                    comment.IsOwnersAccommodationOnLocation = true;
                }
                else
                {
                    comment.IsOwnersAccommodationOnLocation = false;
                }
            }
            return comments;
        }

        public List<Comment> GetMarkedCommentData(List<Comment> comments)
        {
            foreach (Comment comment in comments)
            {
                if ((comment.User.UserType == "Guest1" && _userService.CheckGuestsPresence(comment.User.Id, comment.Forum.Location)) ||
                    (comment.User.UserType == "Guest2" && _guestService.CheckGuestsPresence(comment.User.Username, comment.Forum.Location)))
                {
                    comment.IsMarkedComment = true;
                }
                else
                {
                    comment.IsMarkedComment = false;
                }
            }
            return comments;
        }

        public bool IsOwnersAccommodationOnLocation(User user, Forum forum)
        {
            List<Accommodation> ownersAccommodation = _accommodationService.GetOwnersAccommodations(user.Id);

            foreach (Accommodation accommodation in ownersAccommodation)
            {
                if (accommodation.Location.Id == forum.Location.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}




