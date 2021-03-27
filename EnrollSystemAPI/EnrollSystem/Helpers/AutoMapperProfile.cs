using AutoMapper;
using EnrollSystem.Entities;
using EnrollSystem.Models.Course;
using EnrollSystem.Models.Room;
using EnrollSystem.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //user
            CreateMap<User, UserModel>()
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(src => src.Avatar.Path));
            CreateMap<UserRegisterModel, User>();
            CreateMap<UserUpdateModel, User>();

            //room
            CreateMap<Room, RoomModel>();
            CreateMap<RoomInputModel, Room>();
            //course
            CreateMap<Course, CourseModel>();
            CreateMap<CourseInputModel, Course>();
        }
    }
}
