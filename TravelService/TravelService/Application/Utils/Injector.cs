﻿using System;
using System.Collections.Generic;
using TravelService.Domain.RepositoryInterface;
using TravelService.Application;
using TravelService.Repository;
using TravelService.Application.ServiceInterface;
using TravelService.Application.UseCases;

namespace TravelService.Application.Utils
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
        { typeof(IReservationRequestRepository), new ReservationRequestRepository() },
        { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
        //{ typeof(IReservationRequestService), new ReservationRequestService(new ReservationRequestRepository())},
        // Add more implementations here
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
