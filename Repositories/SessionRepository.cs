﻿using MasteryTest3.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MasteryTest3.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SessionRepository(IHttpContextAccessor contextAccessor) { 
            _contextAccessor = contextAccessor;
        }
        public int? GetInt(string key)
        {
            return _contextAccessor.HttpContext?.Session.GetInt32(key);
        }

        public string? GetString(string key)
        {
            return _contextAccessor.HttpContext?.Session.GetString(key);
        }

        public void SetInt(string key, int value)
        {
            _contextAccessor.HttpContext?.Session.SetInt32(key, value);
        }

        public void SetString(string key, string value)
        {
            _contextAccessor.HttpContext?.Session.SetString(key, value);
        }
    }
}
