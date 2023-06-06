using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelService.Applications.UseCases;
using TravelService.Applications.Utils;
using TravelService.Domain.Model;
using TravelService.Domain.RepositoryInterface;

namespace TravelService.WPF.View
{
    /// <summary>
    /// Interaction logic for AddingForumComment.xaml
    /// </summary>
    public partial class AddingForumComment : Window, INotifyPropertyChanged
    {
        private CommentService _commentService;
        public Owner Owner { get; set; }
        public Forum Forum { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        private string _commentContent;
        public string CommentContent
        {
            get => _commentContent;
            set
            {
                if (value != _commentContent)
                {
                    _commentContent = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddingForumComment(Owner owner, Forum selectedForum, ObservableCollection<Comment> comments)
        {
            InitializeComponent();
            _commentService = new CommentService(Injector.CreateInstance<ICommentRepository>());
            Owner = owner;
            Forum = selectedForum;
            Comments = comments;
            DataContext = this;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AddComment_Click(object sender, RoutedEventArgs e)
        {
            bool IsMarkedComment = false;
            Comment comment = new Comment(Owner, Forum, CommentContent, DateTime.Now, IsMarkedComment);
            Comments.Add(comment);
            _commentService.Save(comment);
            Close();
        }
    }
}
