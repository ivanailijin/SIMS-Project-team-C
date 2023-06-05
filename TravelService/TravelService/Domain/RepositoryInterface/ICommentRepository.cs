using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Domain.Model;

namespace TravelService.Domain.RepositoryInterface
{
    public interface ICommentRepository
    {
        public List<Comment> GetAll();
        public Comment Save(Comment comment);
        public int NextId();
        public void Delete(Comment comment);
        public Comment Update(Comment comment);
    }
}
