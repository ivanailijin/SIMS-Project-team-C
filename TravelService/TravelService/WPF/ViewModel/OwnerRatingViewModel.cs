﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using TravelService.Domain.Model;
using TravelService.Repository;
using TravelService.WPF.View;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.RepositoryInterface;
using System.Collections.ObjectModel;

namespace TravelService.WPF.ViewModel
{
    public class OwnerRatingViewModel : ViewModelBase
    {
        private RatingViewModel _ratingViewModel;
        private AccommodationReservationService _reservationService;
        private OwnerService _ownerService;
        private OwnerRatingService _ownerRatingService;
        public AccommodationReservation SelectedUnratedOwner { get; set; }
        public Action CloseAction { get; set; }

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _ownerName;
        public string OwnerName
        {
            get => _ownerName;
            set
            {
                if (value != _ownerName)
                {
                    _ownerName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _correctness;
        public int Correctness
        {
            get => _correctness;
            set
            {
                if (value != _correctness)
                {
                    _correctness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _cleanliness;
        public int Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _location;
        public int Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _comfort;
        public int Comfort
        {
            get => _comfort;
            set
            {
                if (value != _comfort)
                {
                    _comfort = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _content;
        public int Contents
        {
            get => _content;
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _pictures;

        public string Pictures
        {
            get => _pictures;
            set
            {
                if (value != _pictures)
                {
                    _pictures = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _addOwnerRatingCommand;
        public RelayCommand AddOwnerRatingCommand
        {
            get => _addOwnerRatingCommand;
            set
            {
                if (value != _addOwnerRatingCommand)
                {
                    _addOwnerRatingCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand _addPicturesCommand;
        public RelayCommand AddPicturesCommand
        {
            get => _addPicturesCommand;
            set
            {
                if (value != _addPicturesCommand)
                {
                    _addPicturesCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand _cancelOwnerRatingCommand;
        public RelayCommand CancelOwnerRatingCommand
        {
            get => _cancelOwnerRatingCommand;
            set
            {
                if (value != _cancelOwnerRatingCommand)
                {
                    _cancelOwnerRatingCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private ObservableCollection<string> _picturesList;
        public ObservableCollection<string> PicturesList
        {
            get => _picturesList;
            set
            {
                if (value != _picturesList)
                {
                    _picturesList = value;
                    OnPropertyChanged();
                }
            }
        }

        public OwnerRatingViewModel(RatingViewModel ratingViewModel, AccommodationReservation selectedUnratedOwner)
        {
            _ratingViewModel = ratingViewModel;
            SelectedUnratedOwner = selectedUnratedOwner;
            PicturesList = new ObservableCollection<string>();
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _ownerService = new OwnerService(Injector.CreateInstance<IOwnerRepository>());
            _ownerRatingService = new OwnerRatingService(Injector.CreateInstance<IOwnerRatingRepository>());

            AccommodationName = selectedUnratedOwner.Accommodation.Name;
            Owner owner = _ownerService.FindById(selectedUnratedOwner.OwnerId);
            OwnerName = owner.Username;

            AddOwnerRatingCommand = new RelayCommand(Execute_AddOwnerRating, CanExecute_Command);
            AddPicturesCommand = new RelayCommand(Execute_AddPictures, CanExecute_Command);
            CancelOwnerRatingCommand = new RelayCommand(Execute_CancelOwnerRating, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_AddOwnerRating(object sender)
        {
            List<string> formattedPictures = new List<string>();
            string[] delimitedPictures = Pictures.Split(new char[] { '|' });

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }

            OwnerRating ownerRating = new OwnerRating(SelectedUnratedOwner.Id, SelectedUnratedOwner.AccommodationId, SelectedUnratedOwner.GuestId, SelectedUnratedOwner.OwnerId, Correctness, Cleanliness, Location, Comfort, Contents, Comment, formattedPictures);
            _ownerRatingService.Save(ownerRating);

            AccommodationReservation ratedOwner = _reservationService.FindById(SelectedUnratedOwner.Id);
            ratedOwner.IsOwnerRated = true;
            _reservationService.Update(ratedOwner);

            _ratingViewModel.UnratedOwners.Remove(ratedOwner);

            CloseAction();
        }

        private void Execute_AddPictures(object sender)
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
                    PicturesList.Add(file);
                }
                Pictures = Pictures.Substring(0, Pictures.Length - 1);
            }
        }

        private void Execute_CancelOwnerRating(object sender)
        {
            CloseAction();
        }
    }
}