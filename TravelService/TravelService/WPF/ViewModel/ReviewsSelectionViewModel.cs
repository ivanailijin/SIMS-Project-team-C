using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using TravelService.Application.UseCases;
using TravelService.Application.Utils;
using TravelService.Commands;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;
using TravelService.Repository;
using TravelService.WPF.View;
using Document = iTextSharp.text.Document;
using Paragraph = iTextSharp.text.Paragraph;

namespace TravelService.WPF.ViewModel
{
    public class ReviewsSelectionViewModel : ViewModelBase
    {
        public AccommodationService _accommodationService;

        public LocationService _locationService;

        public OwnerRatingService _ownerRatingService;
        public ReviewsSelectionView ReviewsSelectionView { get; set; }
        public Action CloseAction { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ShowReviewCommand { get; set; }
        public RelayCommand GenerateReportCommand { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static List<Location> Locations { get; set; }
        public Owner Owner { get; set; }

        public ReviewsSelectionViewModel(Owner owner, ReviewsSelectionView reviewsSelectionView)
        {
            InitializeCommands();
            this.Owner = owner;
            _accommodationService = new AccommodationService(Injector.CreateInstance<IAccommodationRepository>());
            _locationService = new LocationService(Injector.CreateInstance<ILocationRepository>());
            _ownerRatingService = new OwnerRatingService(Injector.CreateInstance<IOwnerRatingRepository>());
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetOwnersAccommodations(Owner.Id));
            Locations = new List<Location>(_locationService.GetAll());

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = Locations.Find(l => l.Id == accommodation.LocationId);
            }
            ReviewsSelectionView = reviewsSelectionView;
        }
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            ShowReviewCommand = new RelayCommand(Execute_ShowReviewCommand, CanExecute_Command);
            GenerateReportCommand = new RelayCommand(Execute_GenerateReportCommand, CanExecute_Command);
        }
        private void Execute_GenerateReportCommand(object obj)
        {
            GenerateAverageAccommodationRatingsPDF();
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
        public void GenerateAverageAccommodationRatingsPDF()
        {
            try
            {
                string filePath = OpenFilePicker();

                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.SetFullCompression();

                document.Open();

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("C:/Users/hp/Desktop/slike_za_projekat/downloadTravel.jpg");
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
                document.Add(new Paragraph(" "));

                Paragraph heading = new(
                $"Izveštaj o prosečnim ocenama za svaki smeštaj vlasnika {Owner.Username}:",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    SpacingAfter = 15f
                };
                document.Add(heading);

                string reportText = @"Postovani,

            U prilogu Vam saljemo pregled prosecnih ocena koje ste dobili od gostiju smestaja. 
            Ove ocene odražavaju njihovo misljenje o vama kao vlasniku, vasem smestaju i
            opstom iskustvu koje ste imali.
            Za svaki od vasih smestaja je izdvojena prosecna ocena po svakoj od kategorija.";

                Paragraph reportParagraph = new Paragraph(reportText);
                document.Add(reportParagraph);
                document.Add(new Paragraph(" "));

                PdfPTable table = new(6);
                table.AddCell("Smeštaj");
                table.AddCell("Cistoca");
                table.AddCell("Korektnost vlasnika");
                table.AddCell("Lokacija");
                table.AddCell("Udobnost");
                table.AddCell("Sadržaji");

                foreach (Accommodation accommodation in Accommodations)
                {
                    table.AddCell(accommodation.Name);
                    table.AddCell(Math.Round(_ownerRatingService.GetAverageCleanliness(accommodation), 2).ToString());
                    table.AddCell(Math.Round(_ownerRatingService.GetAverageCorrectness(accommodation), 2).ToString());
                    table.AddCell(Math.Round(_ownerRatingService.GetAverageLocation(accommodation), 2).ToString());
                    table.AddCell(Math.Round(_ownerRatingService.GetAverageComfort(accommodation), 2).ToString());
                    table.AddCell(Math.Round(_ownerRatingService.GetAverageContent(accommodation), 2).ToString());
                }

                document.Add(table);

                document.Add(new Paragraph(" "));
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
        private void Execute_ShowReviewCommand(object obj)
        {
            if (SelectedAccommodation != null && Owner != null)
            {
                AccommodationReview accommodationReview = new AccommodationReview(SelectedAccommodation, Owner);
                OwnerWindow ownerWindow = Window.GetWindow(ReviewsSelectionView) as OwnerWindow;
                ownerWindow?.SwitchToPage(accommodationReview);
            }
            else
            {
                MessageBox.Show("Please select accommodation to show the review.");
            }
        }
        private void Execute_CancelCommand(object obj)
        {
            ReviewsSelectionView.GoBack();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
