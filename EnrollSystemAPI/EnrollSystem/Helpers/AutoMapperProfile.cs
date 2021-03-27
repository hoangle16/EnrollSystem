using AutoMapper;
using EnrollSystem.Entities;
using EnrollSystem.Models.Course;
using EnrollSystem.Models.Room;
using EnrollSystem.Models.Section;
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
            //Section
            CreateMap<Section, SectionModel>()
                .ForMember(dst => dst.TeacherName, src => src.MapFrom(src => src.Teacher.User.Name))
                .ForMember(dst => dst.CourseName, src => src.MapFrom(src => src.Course.Name))
                .ForMember(dst => dst.RoomName, src => src.MapFrom(src => src.Room.Name));
            CreateMap<SectionRegisterModel, Section>();
            CreateMap<SectionUpdateModel, Section>();
            //Schedule
            CreateMap<Section, ScheduleModel>();
        }
    }
}
