using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface IImageRepository
    {
        Image Create(Image image);
        IEnumerable<Image> GetAll();
        Image GetById(int imageId);
        void Delete(int imageId);
    }
}
