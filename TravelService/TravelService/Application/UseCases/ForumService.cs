﻿using System;
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
        private readonly AccommodationService _accommodationService;

        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
            _userService = new UserService(Injector.CreateInstance<IUserRepository>());
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
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
            forums = GetUserData(forums);
            forums = GetHelpfulData(forums);
            return forums;
        }
        public List<Forum> GetHelpfulData(List<Forum> forums)
        {
            foreach(Forum forum in forums)
            {
                if(GetNumberOfOwnerComments(forum) >= 10 && GetNumberOfGuestComments(forum) >= 20)
                {
                    forum.Helpful = true;
                }
                else
                {
                    forum.Helpful = false;
                }
            }
            return forums;
        }
        public bool GetOwnersAuthorization(Owner owner, Forum forum)
        {
            List<Accommodation> ownersAccommodation = _accommodationService.GetOwnersAccommodations(owner.Id);

            foreach(Accommodation accommodation in ownersAccommodation)
            {
                if(accommodation.Location.Id == forum.Location.Id)
                {
                    return true;
                }
            }
            return false;
        }
        public int GetNumberOfOwnerComments(Forum forum)
        {
            int count = 0;
            foreach(Comment comment in forum.Comments)
            {
                if(comment.User.UserType == "Owner")
                {
                    count++;
                }
            }
            return count;
        }
        public int GetNumberOfGuestComments(Forum forum)
        {
            int count = 0;
            foreach (Comment comment in forum.Comments)
            {
                if (comment.User.UserType == "Guest1")
                {
                    count++;
                }
            }
            return count;
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

        public Forum FindById(int id)
        {
            Forum forum = _forumRepository.FindById(id);
            return forum;
        }
        public List<IGrouping<Location, Forum>> GetForumsByLocation()
        {
            List<Forum> forums = GetAll();
            var forumsByLocation = forums.GroupBy(f => f.Location).ToList();
            return forumsByLocation;
        }
    }
}