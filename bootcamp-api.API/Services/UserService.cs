using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Domain;
using bootcamp_api.Data;
using bootcamp_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace bootcamp_api.Services
{
    public class UserService : IUserService
    {

        private readonly PawssierContext _context;
        private readonly IMapper _mapper;

        public UserService(PawssierContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Dto.User Authenticate(ApiVersion version, Dto.Credentials credentials)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == credentials.Email);
            if (user == null || user.Password != credentials.Password)
                throw new UnauthorizedException();

            var dto = _mapper.Map<User, Dto.User>(user);
            dto.Link = LinkService.GenerateUserLink(version, dto.Id);
            return dto;
        }

        public Dto.User Get(ApiVersion version, int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                throw new UserNotFoundException(id);

            var dto = _mapper.Map<User, Dto.User>(user);
            dto.Link = LinkService.GenerateUserLink(version, dto.Id);
            return dto;
        }

        public Dto.User Add(ApiVersion version, Dto.User user)
        {
            var dupe = _context.Users.SingleOrDefault(u => u.Email == user.Email);
            if (dupe is not null)
                throw new DuplicateUserException(user.Email);

            DateTime now = DateTime.Now;
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Email = user.Email,
                Password = user.Password,
                DateAdded = now,
                DateModified = now
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var dto = _mapper.Map<User, Dto.User>(newUser);
            dto.Link = LinkService.GenerateUserLink(version, dto.Id);
            return dto;
        }

        public void Delete(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                throw new UserNotFoundException(id);

            _context.Remove(user);
            _context.SaveChanges();
        }

        public Dto.User Update(ApiVersion version, int id, Dto.User updatedUser)
        {
            if (id != updatedUser.Id)
                throw new Exception();

            var currentUser = _context.Users.SingleOrDefault(u => u.Id == id);
            if (currentUser == null)
                throw new UserNotFoundException(id);

            var dupe = _context.Users.SingleOrDefault(u => u.Email == updatedUser.Email);
            if (dupe is not null && dupe != currentUser)
                throw new DuplicateUserException(updatedUser.Email);

            currentUser.FirstName = updatedUser.FirstName;
            currentUser.LastName = updatedUser.LastName;
            currentUser.Birthday = updatedUser.Birthday;
            currentUser.Email = updatedUser.Email;
            currentUser.Password = updatedUser.Password;
            currentUser.DateModified = DateTime.Now;

            _context.SaveChanges();

            var dto = _mapper.Map<User, Dto.User>(currentUser);
            dto.Link = LinkService.GenerateUserLink(version, dto.Id);
            return dto;
        }


    }
}