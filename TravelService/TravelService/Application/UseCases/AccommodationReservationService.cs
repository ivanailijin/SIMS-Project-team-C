﻿using System;
using System.Collections.Generic;
using TravelService.Application.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Application.UseCases
{
    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationService _accommodationService;
        private readonly LocationService _locationService;

        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository)
        {
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
        }
        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservationRepository.GetAll();
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            return _accommodationReservationRepository.Save(accommodationReservation);
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservationRepository.Delete(accommodationReservation);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            return _accommodationReservationRepository.Update(accommodationReservation);
        }

        public AccommodationReservation FindById(int id)
        {
            return _accommodationReservationRepository.FindById(id);
        }

        public List<AccommodationReservation> FindByGuestId(int guestId)
        {
            return _accommodationReservationRepository.FindReservationsByGuestId(guestId);
        }

        public void SetLocation(List<Location> locations)
        {
            _accommodationReservationRepository.SetLocation(locations);
        }

        public void CancelReservation(AccommodationReservation selectedReservation)
        {
            AccommodationReservation reservation = _accommodationReservationRepository.FindById(selectedReservation.Id);
            reservation.IsCancelled = true;
            _accommodationReservationRepository.Update(reservation);
        }

        public bool IsCancellingLimitFulfilled(int id)
        {
            AccommodationReservation accommodationReservation = _accommodationReservationRepository.FindById(id);
            TimeSpan dayDifference = accommodationReservation.CheckInDate - DateTime.Now;
            if (dayDifference.TotalHours > 24)
            {
                return true;
            }
            return false;
        }

        public bool IsMinimumDaysForCancellingFulfilled(AccommodationReservation reservation)
        {
            Accommodation accommodation = _accommodationService.FindById(reservation.AccommodationId);
            TimeSpan daydifference = reservation.CheckInDate - DateTime.Now;
            if (daydifference.TotalDays > accommodation.DaysBeforeCancellingReservation)
            {
                return true;
            }
            return false;
        }

            List<AccommodationReservation> FindReservationsByAccommodation(int accommodationId)
            {
                List<AccommodationReservation> filteredReservations = new List<AccommodationReservation>();
                List<AccommodationReservation> allReservations = GetAll();

                foreach (AccommodationReservation reservation in allReservations)
                {
                    if (reservation.AccommodationId == accommodationId)
                    {
                        filteredReservations.Add(reservation);
                    }
                }
                return filteredReservations;
            }
        

        public bool CheckMatchingDates(AccommodationReservation checkReservation, DateTime newStartDate, DateTime newEndDate)
        {
            if (checkReservation.CheckOutDate < newStartDate || newEndDate < checkReservation.CheckInDate)
            {
                return false;
            }
            return true;
        }

        public AVAILABILITY CheckAvailability(int reservationId, DateTime newStartDate, DateTime newEndDate)
        {
            AccommodationReservation reservation = FindById(reservationId);
            Accommodation accommodation = _accommodationService.FindById(reservation.AccommodationId);

            List<AccommodationReservation> reservationsForCheck = FindReservationsByAccommodation(accommodation.Id);
            reservationsForCheck.Remove(reservation);
            bool hasOverlap = false;

            foreach (AccommodationReservation checkReservation in reservationsForCheck)
            {
                bool matchingDates = CheckMatchingDates(checkReservation, newStartDate, newEndDate);
                if (matchingDates)
                {
                    hasOverlap = true;
                    break;
                }
            }

            if (hasOverlap)
            {
                return AVAILABILITY.Zauzet;
            }
            else
            {
                return AVAILABILITY.Slobodan;

            }
        }

        /*public void GetAccommodationData(List<AccommodationReservation> reservations)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodations.Find(a => a.Id == reservation.AccommodationId);
            }
        }*/

        public void GetLocationData(List<AccommodationReservation> reservations)
        {
            List<Location> locations = _locationService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Location = locations.Find(l => l.Id == reservation.LocationId);

            }
        }

        public List<AccommodationReservation> FindUnratedReservations(int OwnerId)
        {
            List<AccommodationReservation> UnratedReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> accommodationReservations = GetAll();

            foreach (AccommodationReservation reservation in accommodationReservations)
            {
                Accommodation reservedAccommodation = _accommodationService.FindById(reservation.AccommodationId);
                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                if (!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservedAccommodation.OwnerId == OwnerId)
                {
                    UnratedReservations.Add(reservation);
                }
            }

            return UnratedReservations;
        }
        public List<AccommodationReservation> GetAccommodationData(List<AccommodationReservation> reservations)
        {
            List<Accommodation> accommodations = _accommodationService.GetAll();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodations.Find(a => a.Id == reservation.AccommodationId);
            }
            return reservations;
        }
    }

}
