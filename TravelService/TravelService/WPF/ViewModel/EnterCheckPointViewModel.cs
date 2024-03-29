﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;

namespace TravelService.WPF.ViewModel
{
    public class EnterCheckPointViewModel : ViewModelBase
    {
        public int TourId;
        private readonly CheckPointService _checkPointService;
        private readonly TourService _tourService;
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public Action CloseAction { get; set; }
        


        private string _checkPoint;
        public string CheckPoint
        {
            get => _checkPoint;
            set
            {
                if (value != _checkPoint)
                {
                    _checkPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        public EnterCheckPointViewModel(int Id)
        {
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>());
            _checkPointService = new CheckPointService(Injector.CreateInstance<ICheckPointRepository>());
            TourId = Id;

            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            CreateCommand = new RelayCommand(Execute_CreateCommand, CanExecute_Command);
        }

      
        
        private void Execute_CreateCommand(object obj)
        {
            
            if (CheckPoint == null )
            {
                MessageBox.Show("Please enter checkpoints first.");
                return;
            }
            CheckPoint checkPoint = new CheckPoint();
            checkPoint.Name = CheckPoint;
            checkPoint.TourId = TourId;

            CheckPoint savedCheckPoint = _checkPointService.Save(checkPoint);
        }
        private void Execute_CancelCommand(object obj)
        {
            CloseAction();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
