﻿using Microsoft.AspNetCore.Identity;
using Projekt.Models.DTOs.Responses;

namespace Projekt.Models
{
    public class AuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Result { get; set; }
        public UserInfoResponseDTO? UserInfo { get; set; }
        public List<string>? Errors { get; set; }
    }
}
