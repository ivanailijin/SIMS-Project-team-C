
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelService.Domain.Model;
using TravelService.Repository;
using Paragraph = iTextSharp.text.Paragraph;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        public GuideHomePageView GuideHomePageView { get; set; }
        private bool _isSuperGuide;

        public bool IsSuperGuide
        {
            get { return _isSuperGuide; }
            set
            {
                _isSuperGuide = value;
                OnPropertyChanged(); // Ovdje pozovite OnPropertyChanged da biste obavijestili sustav o promjeni vrijednosti svojstva
            }
        }

        private string _confirmationMessage;
        public string ConfirmationMessage
        {
            get { return _confirmationMessage; }
            set
            {
                _confirmationMessage = value;
                OnPropertyChanged(nameof(ConfirmationMessage));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public NavigationService NavigationService;
        public readonly TourRepository _tourRepository;


        public readonly TourReviewRepository _tourReviewRepository;
        public readonly TourRequestRepository _tourRequestREpo;

        public static List<Location> Locations { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<Tour> FutureTours { get; set; }
        public static List<TourReservation> TourReservations { get; set; }
        public static List<TourReview> TourReviews { get; set; }
        public static List<Tour> Tourlist { get; set; }
        public static List<TourRequest> TourRequests { get; set; }

        public static ObservableCollection<Tour> Tours { get; set; }


        public Tour Tour { get; set; }
        public Guide Guide { get; set; }
        public Language Language { get; set; }


        public ProfileView(Guide guide, NavigationService navigationService, GuideHomePageView guideHomePageView)
        {

            NavigationService = navigationService;
            GuideHomePageView = guideHomePageView;


            this.Guide = guide;
            _tourRepository = new TourRepository();


            _tourReviewRepository = new TourReviewRepository();
            _tourRequestREpo = new TourRequestRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());

            TourReviews = new List<TourReview>(_tourReviewRepository.GetAll());
            Tourlist = new List<Tour>(_tourRepository.GetAll());
            FutureTours = new List<Tour>();
            List<Tour> TourList = _tourRepository.GetAll();
            TourRequests = new List<TourRequest>(_tourRequestREpo.GetAll());


            List<Tour> GuideTours = _tourRepository.FindGuidesTours(Guide.Id);

            IsSuperGuide = _tourReviewRepository.CalculateSuperGuideStatus(Guide, TourReviews, TourList);
            InitializeComponent();
            DataContext = this;


        }

        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OtkazTour_Click(object sender, RoutedEventArgs e)
        {

            bool tourCancelled = _tourRepository.Otkaz(Guide.Id);
            if (tourCancelled)
            {
                Tours.Remove(Tour);

                // Display message box with option to send vouchers
                ConfirmationMessage = "Tour cancelled successfully! Vouchers Sent!";

                _tourRepository.PoslatiVaucer();

            }
            else
            {
                ErrorMessage = "You cannot cancel this tour as it starts within 48 hours.";
            }

        }

        public void Log_Out(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            GuideHomePageView.Close();


        }
        public static void GenerateTourRequestStatsPDF(List<TourRequest> tourRequests, string title)
        {
            
                string filePath = OpenFilePicker();

                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.SetFullCompression();

                document.Open();

                // Statistics for All Tour Requests
                Paragraph allRequestsHeading = new Paragraph($"Statistics for All {title} Tour Requests:", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                allRequestsHeading.SpacingAfter = 15f;
                document.Add(allRequestsHeading);

                PdfPTable allRequestsTable = new PdfPTable(5);
                allRequestsTable.AddCell("Request ID");
                allRequestsTable.AddCell("Description");
                allRequestsTable.AddCell("Approval");
                allRequestsTable.AddCell("Start Date");
                allRequestsTable.AddCell("End Date");

                foreach (var request in tourRequests)
                {
                    allRequestsTable.AddCell(request.Id.ToString());
                    allRequestsTable.AddCell(request.Description);
                    allRequestsTable.AddCell(request.RequestApproved.ToString());
                    allRequestsTable.AddCell(request.TourStart.ToString("dd/MM/yyyy"));
                    allRequestsTable.AddCell(request.TourEnd.ToString("dd/MM/yyyy"));
                }

                document.Add(allRequestsTable);

                // Separator
                document.Add(new Paragraph("\n"));

                // Statistics for Accepted Tour Requests
                List<TourRequest> acceptedRequests = tourRequests.Where(request => request.RequestApproved == APPROVAL.ACCEPTED).ToList();

                Paragraph acceptedRequestsHeading = new Paragraph($"Statistics for Accepted {title} Tour Requests:", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                acceptedRequestsHeading.SpacingAfter = 15f;
                document.Add(acceptedRequestsHeading);

                PdfPTable acceptedRequestsTable = new PdfPTable(5);
                acceptedRequestsTable.AddCell("Request ID");
                acceptedRequestsTable.AddCell("Description");
                acceptedRequestsTable.AddCell("Approval");
                acceptedRequestsTable.AddCell("Start Date");
                acceptedRequestsTable.AddCell("End Date");

                foreach (var request in acceptedRequests)
                {
                    acceptedRequestsTable.AddCell(request.Id.ToString());
                    acceptedRequestsTable.AddCell(request.Description);
                    acceptedRequestsTable.AddCell(request.RequestApproved.ToString());
                    acceptedRequestsTable.AddCell(request.TourStart.ToString("dd/MM/yyyy"));
                    acceptedRequestsTable.AddCell(request.TourEnd.ToString("dd/MM/yyyy"));
                }

                document.Add(acceptedRequestsTable);

                // Separator
                document.Add(new Paragraph("\n"));
                List<TourRequest> denied = tourRequests.Where(request => request.RequestApproved == APPROVAL.INVALID).ToList();

                Paragraph deniedRequestsHeading = new Paragraph($"Statistics for Denied {title} Tour Requests:", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                deniedRequestsHeading.SpacingAfter = 15f;
                document.Add(deniedRequestsHeading);

                PdfPTable deniedRequestsTable = new PdfPTable(5);
                deniedRequestsTable.AddCell("Request ID");
                deniedRequestsTable.AddCell("Description");
                deniedRequestsTable.AddCell("Approval");
                deniedRequestsTable.AddCell("Start Date");
                deniedRequestsTable.AddCell("End Date");

                foreach (var request in denied)
                {
                    deniedRequestsTable.AddCell(request.Id.ToString());
                    deniedRequestsTable.AddCell(request.Description);
                    deniedRequestsTable.AddCell(request.RequestApproved.ToString());
                    deniedRequestsTable.AddCell(request.TourStart.ToString("dd/MM/yyyy"));
                    deniedRequestsTable.AddCell(request.TourEnd.ToString("dd/MM/yyyy"));
                }

                document.Add(deniedRequestsTable);

                // Separator
                document.Add(new Paragraph("\n"));

                // Additional Statistics Table
                // Add your additional statistics table here

                document.Close();

      
           
              
            
        }
        private void GenerateStatistics_Click(object sender, RoutedEventArgs e)
        {
            // Pozovite metodu za generiranje statistike PDF-a
            GenerateTourRequestStatsPDF(TourRequests, "Tour Requests");
        }


        private static string OpenFilePicker()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            if (saveFileDialog.ShowDialog() == true)
                return saveFileDialog.FileName;
            throw new Exception("Save file dialog returned error!");
        }



    }
}
