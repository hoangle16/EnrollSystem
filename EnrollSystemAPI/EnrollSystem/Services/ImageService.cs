using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class ImageService : IImageRepository
    {
        private EnrollContext _context;
        public ImageService(EnrollContext context)
        {
            _context = context;
        }
        public Image Create(Image image)
        {
            //check path

            _context.Images.Add(image);
            _context.SaveChanges();
            return image;
        }

        public void Delete(int imageId)
        {
            var image = _context.Images.Find(imageId);
            _context.Images.Remove(image);
            _context.SaveChanges();
        }

        public IEnumerable<Image> GetAll()
        {
            return _context.Images;
        }

        public Image GetById(int imageId)
        {
            return _context.Images.Find(imageId);
        }
    }
}
