using System;
using System.Collections.Generic;
using TravelService.Domain.RepositoryInterface;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Repository;
using TravelService.Applications;
using TravelService.Repository;
using TravelService.Applications.UseCases;

namespace TravelService.Applications.Utils
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
        { typeof(IReservationRequestRepository), new ReservationRequestRepository() },
        { typeof(ILocationRepository), new LocationRepository() },
        { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
        { typeof(IAccommodationRepository), new AccommodationRepository() },
        { typeof(IGuest1Repository), new Guest1Repository() },
        { typeof(IOwnerRepository), new OwnerRepository() },
        { typeof(IOwnerRatingRepository), new OwnerRatingRepository() },
        { typeof(IUserRepository), new UserRepository() },
        { typeof(ITourReviewRepository), new TourReviewRepository() },
        { typeof(ITourRepository), new TourRepository() },
        { typeof(IGuest2Repository), new Guest2Repository() },
        { typeof(ICheckPointRepository), new CheckPointRepository() },
        { typeof(IGuestRepository), new GuestRepository() },
        { typeof(IGuideRepository), new GuideRepository() },
        { typeof(ILanguageRepository), new LanguageRepository() },
        { typeof(IInvitationRepository), new InvitationRepository() },
        { typeof(IGuestRatingRepository), new GuestRatingRepository() },
        { typeof(IVoucherRepository), new GuestVoucherRepository() },
        { typeof(ITourReservationRepository), new TourReservationRepository() },
        { typeof(IAccommodationRenovationRepository), new AccommodationRenovationRepository() },
        { typeof(ITourRequestRepository), new TourRequestRepository() },
        { typeof(INewTourNotificationRepository), new NewTourNotificationRepository() },
        { typeof(IRenovationRecommendationRepository), new RenovationRecommendationRepository() },
        { typeof(IForumRepository), new ForumRepository() },
        { typeof(ICommentRepository), new CommentRepository() },
        { typeof(IComplexTourRequestRepository), new ComplexTourRequestRepository() },

       

    };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}