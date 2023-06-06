using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;

namespace TravelService.Applications.UseCases
{
    public class TourReviewService
    {
        private readonly ITourReviewRepository _tourReviewRepository;

        public readonly GuestRepository _guestReposiotry;

        public TourReviewService(ITourReviewRepository tourReviewRepository)
        {
            _tourReviewRepository = tourReviewRepository;
        }
        public void Delete(TourReview tourReview)
        {
            _tourReviewRepository.Delete(tourReview);
        }

        public List<TourReview> GetAll()
        {
            return _tourReviewRepository.GetAll();
        }

        public TourReview Save(TourReview tourReview)
        {
            TourReview savedTourReview = _tourReviewRepository.Save(tourReview);
            return savedTourReview;
        }

        public void Update(TourReview tourReview)
        {
            _tourReviewRepository.Update(tourReview);
        }


        public void addReview(int guideKnowledge, int guideLanguage, int tourEntertainment, string comment, string pictures, Tour selectedTour, Guest2 guest2)
        {

            List<string> formattedPictures = new List<string>();

            string[] delimitedPictures = pictures.Split(new char[] { '|' });

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }

            TourReview tourReview = new TourReview(guideKnowledge, guideLanguage, tourEntertainment, comment, formattedPictures, selectedTour.GuideId, guest2.Id, false);
            _tourReviewRepository.Save(tourReview);
        }

        public string addPictures(string Pictures)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "Image files (*.jpg;*.jpeg;*.png;*.jfif)|*.jpg;*.jpeg;*.png;*.jfif";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {
                string[] selectedFiles = dlg.FileNames;

                string destinationFolder = @"../../../Resources/Images/";

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                foreach (string file in selectedFiles)
                {
                    Pictures += file;
                    Pictures += "|";
                    string destinationFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(file));
                    File.Copy(file, destinationFilePath);
                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);
            }
            return Pictures;
        }

        public string addToPictureList(string newPictures, string Pictures)
        {

            if (!string.IsNullOrEmpty(newPictures))
            {
                if (!string.IsNullOrEmpty(Pictures))
                {
                    Pictures += "|";
                }
                Pictures += newPictures;
            }
            return Pictures;
        }

        public List<TourReview> FindGuestsTourReviews(List<TourReview> tourReviews, Guest guest)
        {
            List<TourReview> matchingTourReviews = new List<TourReview>();

            foreach (TourReview tourReview in tourReviews)
            {
                if (tourReview.GuestId == guest.Id)
                {
                    matchingTourReviews.Add(tourReview);
                }
            }

            return matchingTourReviews;
        }
    }
}
