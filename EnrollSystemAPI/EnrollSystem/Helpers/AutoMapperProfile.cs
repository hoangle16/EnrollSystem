using AutoMapper;
using EnrollSystem.Entities;
using EnrollSystem.Models.Attendance;
using EnrollSystem.Models.Course;
using EnrollSystem.Models.Room;
using EnrollSystem.Models.Section;
using EnrollSystem.Models.Student;
using EnrollSystem.Models.StudentSectionRegistration;
using EnrollSystem.Models.Teacher;
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
            CreateMap<Room, RoomSectionsModel>();
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

            //mapper section => room
            CreateMap<Section, Room>()
                .ForMember(dst => dst.Id, src => src.MapFrom(src => src.RoomId))
                .ForMember(dst => dst.Name, src => src.MapFrom(src => src.Room.Name));
            //Student
            CreateMap<Student, StudentModel>()
                .ForMember(dst => dst.UserId, src => src.MapFrom(src => src.UserId))
                .ForMember(dst => dst.UserName, s => s.MapFrom(s => s.User.UserName))
                .ForMember(d => d.Avatar, s => s.MapFrom(s => s.User.Avatar.Path))
                .ForMember(d => d.Name, s => s.MapFrom(s => s.User.Name))
                .ForMember(d => d.Gender, s => s.MapFrom(s => s.User.Gender))
                .ForMember(d => d.IdNumber, s => s.MapFrom(s => s.User.IdNumber))
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(s => s.User.PhoneNumber))
                .ForMember(d => d.Address, s => s.MapFrom(s => s.User.Address))
                .ForMember(d => d.IsActive, s => s.MapFrom(s => s.User.IsActive))
                .ForMember(d => d.StudentId, s => s.MapFrom(s => s.Id));
            //Section => Teacher
            CreateMap<Section, Teacher>()
                .ForMember(d => d.Id, s => s.MapFrom(s => s.TeacherId))
                .ForMember(d => d.UserId, s => s.MapFrom(s => s.Teacher.UserId));
            //Teacher
            CreateMap<Teacher, TeacherModel>()
                .ForMember(dst => dst.UserId, src => src.MapFrom(src => src.UserId))
                .ForMember(dst => dst.UserName, s => s.MapFrom(s => s.User.UserName))
                .ForMember(d => d.Avatar, s => s.MapFrom(s => s.User.Avatar.Path))
                .ForMember(d => d.Name, s => s.MapFrom(s => s.User.Name))
                .ForMember(d => d.Gender, s => s.MapFrom(s => s.User.Gender))
                .ForMember(d => d.IdNumber, s => s.MapFrom(s => s.User.IdNumber))
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(s => s.User.PhoneNumber))
                .ForMember(d => d.Address, s => s.MapFrom(s => s.User.Address))
                .ForMember(d => d.IsActive, s => s.MapFrom(s => s.User.IsActive))
                .ForMember(d => d.TeacherId, s => s.MapFrom(s => s.Id));
            //StudentSectionRegistration
            CreateMap<StudentSectionRegistration, StudentSectionRegModel>()
                .ForMember(d => d.StudentUserName, s => s.MapFrom(s => s.Student.User.UserName))
                .ForMember(d => d.StudentName, s => s.MapFrom(s => s.Student.User.Name))
                .ForMember(d => d.StudentPhoneNumber, s => s.MapFrom(s => s.Student.User.PhoneNumber))
                .ForMember(d => d.CourseName, s => s.MapFrom(s => s.Section.Course.Name))
                .ForMember(d => d.SectionTeacherName, s => s.MapFrom(s => s.Section.Teacher.User.Name))
                .ForMember(d => d.StartDay, s => s.MapFrom(s => s.Section.StartDay))
                .ForMember(d => d.EndDay, s => s.MapFrom(s => s.Section.EndDay))
                .ForMember(d => d.StartTime, s => s.MapFrom(s => s.Section.StartTime))
                .ForMember(d => d.EndTime, s => s.MapFrom(s => s.Section.EndTime))
                .ForMember(d => d.Schedule, s => s.MapFrom(s => s.Section.Schedule))
                .ForMember(d => d.Slot, s => s.MapFrom(s => s.Section.Slot))
                .ForMember(d => d.MaxSlot, s => s.MapFrom(s => s.Section.MaxSlot))
                .ForMember(d => d.Room, s => s.MapFrom(s => s.Section.Room.Name));
            //Attendance
            CreateMap<Attendance, AttendanceModel>()
                .ForMember(d => d.SectionId, s => s.MapFrom(s => s.StudentSection.SectionId))
                .ForMember(d => d.StudentId, s => s.MapFrom(s => s.StudentSection.StudentId))
                .ForMember(d => d.UserId, s => s.MapFrom(s => s.StudentSection.Student.UserId))
                .ForMember(d => d.Name, s => s.MapFrom(s => s.StudentSection.Student.User.Name));
            CreateMap<TrainingImage, TrainingImageModel>()
                .ForMember(d => d.Path, s => s.MapFrom(s => s.Image.Path))
                .ForMember(d => d.StudentUserName, s => s.MapFrom(s => s.Student.User.UserName));
            CreateMap<AttendanceImage, AttendanceImageModel>()
                .ForMember(d => d.Path, s => s.MapFrom(s => s.Image.Path));
            //Calendar item
            CreateMap<Section, CalendarModel>()
                .ForMember(d => d.CourseName, s => s.MapFrom(s => s.Course.Name))
                .ForMember(d => d.RoomName, s => s.MapFrom(s => s.Room.Name));
        }
    }
}
