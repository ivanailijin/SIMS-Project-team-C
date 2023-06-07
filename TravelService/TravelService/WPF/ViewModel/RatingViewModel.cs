using iTextSharp.text.pdf;
using iTextSharp.text;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.WPF.View;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace TravelService.WPF.ViewModel
{
    public class RatingViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService;
        private GuestRatingService _guestRatingService;
        public AccommodationReservation SelectedUnratedOwner { get; set; }
        public Guest1 Guest1 { get; set; }
        public RatingView RatingView { get; set; }

        private ObservableCollection<AccommodationReservation> _unratedOwners;
        public ObservableCollection<AccommodationReservation> UnratedOwners
        {
            get => _unratedOwners;
            set
            {
                if (value != _unratedOwners)
                {
                    _unratedOwners = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<GuestRating> _guestRatings;
        public ObservableCollection<GuestRating> GuestRatings
        {
            get => _guestRatings;
            set
            {
                if (value != _guestRatings)
                {
                    _guestRatings = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _generatePDFCommand;
        public RelayCommand GeneratePDFCommand
        {
            get => _generatePDFCommand;
            set
            {
                if (value != _generatePDFCommand)
                {
                    _generatePDFCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _ownerRatingWindowCommand;
        public RelayCommand OwnerRatingWindowCommand
        {
            get => _ownerRatingWindowCommand;
            set
            {
                if (value != _ownerRatingWindowCommand)
                {
                    _ownerRatingWindowCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public RatingViewModel(RatingView ratingView, Guest1 guest1)
        {
            this.Guest1 = guest1;
            RatingView = ratingView;
            _reservationService = new AccommodationReservationService(Injector.CreateInstance<IAccommodationReservationRepository>());
            _guestRatingService = new GuestRatingService(Injector.CreateInstance<IGuestRatingRepository>());

            List<AccommodationReservation> reservations = new List<AccommodationReservation>(_reservationService.FindUnratedOwners(Guest1.Id));
            reservations = _reservationService.GetAccommodationData(reservations);
            reservations = _reservationService.GetLocationData(reservations);
            reservations = _reservationService.GetOwnerData(reservations);
            UnratedOwners = new ObservableCollection<AccommodationReservation>(reservations);

            List<GuestRating> guestRatings = new List<GuestRating>(_guestRatingService.FindCommonGuestRatings(Guest1.Id));
            guestRatings = _guestRatingService.GetOwnerData(guestRatings);
            guestRatings = _guestRatingService.GetReservationData(guestRatings);
            guestRatings = _guestRatingService.GetAccommodationData(guestRatings);
            guestRatings = _guestRatingService.GetLocationData(guestRatings);

            GuestRatings = new ObservableCollection<GuestRating>(guestRatings);

            GeneratePDFCommand = new RelayCommand(Execute_GeneratePDF, CanExecute_Command);
            OwnerRatingWindowCommand = new RelayCommand(Execute_OwnerRatingWindow, CanExecute_Command);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_GeneratePDF(object sender)
        {
            GenerateGuestAverageRatingPDF(Guest1);
        }

            private void Execute_OwnerRatingWindow(object sender)
        {
            if (SelectedUnratedOwner != null)
            {
                OwnerRatingView ownerRatingView = new OwnerRatingView(this, Guest1, SelectedUnratedOwner);
                FirstGuestWindow firstGuestWindow = Window.GetWindow(RatingView) as FirstGuestWindow ?? new(Guest1);
                firstGuestWindow?.SwitchToPage(ownerRatingView);
            }
            else
            {
                MessageBox.Show("Odaberite smestaj za ocenjivanje!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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

        public void GenerateGuestAverageRatingPDF(Guest1 guest)
        {
            try
            {
                string filePath = OpenFilePicker();

                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.SetFullCompression();

                document.Open();

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("C:/Users/ivana/Desktop/accommodations/travel.jpg");
                logo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                float width = PageSize.A4.Width / 4;
                float height = width * (logo.Height / logo.Width);
                logo.ScaleAbsolute(width, height);

                float X = 0;
                float Y = PageSize.A4.Height - height;
                logo.SetAbsolutePosition(X, Y);
                document.Add(logo);

                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                Paragraph heading = new Paragraph(
                    $"Prikaz prosečnih ocena po kategorijama za gosta {guest.Username}:",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    SpacingAfter = 15f
                };
                document.Add(heading);

                string reportText = @"Postovani goste,

U prilogu Vam saljemo pregled prosecnih ocena koje ste dobili od vlasnika smestaja. Ove ocene odražavaju njihovo misljenje o vama kao gostu, vasem ponasanju tokom boravka i opstom iskustvu koje ste imali.

Cistoca:  Prosecna ocena koju ste dobili za cistocu smestaja ukazuje na vasu paznju prema odrzavanju cistoce smestaja tokom boravka. Nastojimo pruziti visok standard cistoce kako bismo svim gostima omogućili ugodan boravak.

Poštovanje pravila ukazuje na vasu odgovornost i postovanje kucnog reda koji je vazan za sigurnost i ugodnost svih gostiju.

Komunikacija: Vasa komunikacija sa vlasnicima smestaja odrazava vasu sposobnost da jasno izrazavate svoje potrebe, pitanja i komentare, te da uspesno komunicirate sa vlasnicima smestaja.

Postovanje imovine: Prosecna ocena koju se dobili za postovanje imovine ukazuje na vase ophodjenje prema imovini smestaja.";

                Paragraph reportParagraph = new Paragraph(reportText);
                document.Add(reportParagraph);
                document.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(2);
                table.AddCell("Kategorija");
                table.AddCell("Prosecna ocena");

                double cleanness = _guestRatingService.GetAverageCleannes(guest);
                double ruleCompliance = _guestRatingService.GetAverageRulesFollowing(guest);
                double communication = _guestRatingService.GetAverageCommunication(guest);
                double noiseLevel = _guestRatingService.GetAverageNoiseLevel(guest);
                double propertyRespect = _guestRatingService.GetAverageRulesFollowing(guest);

                table.AddCell("Cistoca");
                table.AddCell(cleanness.ToString());
                table.AddCell("Postovanje pravila");
                table.AddCell(ruleCompliance.ToString());
                table.AddCell("Komunikacija");
                table.AddCell(communication.ToString());
                table.AddCell("Nivo bucnosti");
                table.AddCell(noiseLevel.ToString());
                table.AddCell("Postovanje imovine");
                table.AddCell(propertyRespect.ToString());

                document.Add(table);

                document.Add(new Paragraph(" "));

                string text = @"Hvala vam sto ste koristite nasu aplikaciju za rezervaciju smestaja. Vase prosecne ocene nam pomazu da unapredimo nasu ponudu i pruzimo bolje iskustvo svim nasim korisnicima. Ako imate bilo kakva pitanja ili povratne informacije, slobodno nas kontaktirajte.

Srdacan pozdrav,
Vas Travel Service.";
                Paragraph paragraph = new Paragraph(text);
                document.Add(paragraph);
                document.Close();

                MessageBox.Show("PDF file generated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF file: " + ex.Message);
            }
        }

    }
}
