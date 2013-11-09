﻿using System;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.Authentication;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Token AuthenticationRequestAutenticationToken()
        {
            var request = new RestRequest("authentication/token/new")
            {
                DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
            };

            IRestResponse<Token> response = _client.Get<Token>(request);
            var token = response.Data;
            token.ExpiresAt = token.ExpiresAt.ToLocalTime();
            token.AuthenticationCallback =
                response.Headers.First(h => h.Name.Equals("Authentication-Callback")).Value.ToString();

            return token;
        }

        public UserSession AuthenticationGetUserSession(string initialRequestToken)
        {
            var request = new RestRequest("authentication/session/new");
            request.AddParameter("request_token", initialRequestToken);
            
            IRestResponse<UserSession> response = _client.Get<UserSession>(request);

            return response.Data;
        }

        public GuestSession AuthenticationCreateGuestSession()
        {
            var request = new RestRequest("authentication/guest_session/new")
            {
                DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
            };

            IRestResponse<GuestSession> response = _client.Get<GuestSession>(request);
            var guestSession = response.Data;
            guestSession.ExpiresAt = guestSession.ExpiresAt.ToLocalTime();

            return response.Data;
        }
    }
}