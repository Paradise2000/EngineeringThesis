﻿namespace EngineeringThesisAPI.Services.UserIdProvider
{
    public interface IUserIdProvider
    {
        public int GetUserId();
        public bool IsUserLogged();
    }
}
