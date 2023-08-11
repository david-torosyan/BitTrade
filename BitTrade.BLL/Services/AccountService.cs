﻿using BitTrade.BLL.Configuration;
using BitTrade.BLL.Extensions;
using BitTrade.Common.Models;
using BitTrade.DAL;
using BitTrade.DAL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BitTrade.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityService _securityService;
        public AccountService(IUnitOfWork unitOfWork, ISecurityService securityService)
        {
            _unitOfWork = unitOfWork;
            _securityService = securityService;
        }

        public LoginResultModel LogIn(LoginModel model)
        {

            var user = _unitOfWork._userRepository
                .Get(u => u.Email == model.Email).FirstOrDefault();

            if (user == null)
            {
                return new LoginResultModel
                {
                    IsSuccessful = false,
                    EmailErrorMessage = "User is not found."
                };
            }

            if (!_securityService.ValidatePassword(model.Password, user.Password, user.Salt))
            {
                return new LoginResultModel
                {
                    IsSuccessful = false,
                    PasswordErrorMessage = "Password is not correct."
                };
            }

            var userData = JsonConvert.SerializeObject(user.MapTo<AccountModel>());
            var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(10080), model.RememberMe, userData);
            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            HttpContext.Current.Response.Cookies.Add(authCookie);

            return new LoginResultModel
            {
                IsSuccessful = true
            };
        }

        public RegisterResultModel Register(RegisterModel model)
        {


            if (_unitOfWork._userRepository.Any(u => u.Email == model.Email))
            {
                return new RegisterResultModel
                {
                    IsSuccessful = false,
                    EmailErrorMessage = "User is already registered."
                };
            }

            string salt = _securityService.GetRandomString();
            string passwordHash = _securityService.GetSHA256($"{model.Password}{salt}");

            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                IsActive = model.IsActive,
                Email = model.Email,
                Gender = model.Gender,
                ImageURL = model.ImageURL,
                Password = passwordHash,
                Role = model.Role,
                Salt = salt,
            };

            _unitOfWork._userRepository.Insert(user);
            _unitOfWork.Commit();

            return new RegisterResultModel
            {
                IsSuccessful = true,
                ID = user.ID
            };
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
