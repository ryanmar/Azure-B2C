﻿using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace B2C_WebAPI
{
    public partial class Startup
    {
        // Get the B2C configuration values from web.config
        public static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        public static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        public static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string signUpPolicy = ConfigurationManager.AppSettings["ida:SignUpPolicyId"];
        public static string signInPolicy = ConfigurationManager.AppSettings["ida:SignInPolicyId"];
        public static string editProfilePolicy = ConfigurationManager.AppSettings["ida:EditProfilePolicyId"];
        public static string passwordResetPolicy = ConfigurationManager.AppSettings["ida:ResetPasswordPolicyId"];
        public static string signInAndSignUpPolicy = ConfigurationManager.AppSettings["ida:SignInAndSignUpPolicyId"];

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(signUpPolicy));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(signInPolicy));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(editProfilePolicy));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(passwordResetPolicy));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(signInAndSignUpPolicy));

            //Test B2C App: Application Policies 
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy("B2C_1_sign_up"));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy("B2C_1_sign_in"));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy("B2C_1_edit_profile"));
        }

        public OAuthBearerAuthenticationOptions CreateBearerOptionsFromPolicy(string policy)
        {
            TokenValidationParameters tvps = new TokenValidationParameters
            {
                // This is where you specify that your API only accepts tokens from its own clients
                ValidAudience = clientId,
                AuthenticationType = policy
            };

            return new OAuthBearerAuthenticationOptions
            {
                // This SecurityTokenProvider fetches the Azure AD B2C metadata & signing keys from the OpenIDConnect metadata endpoint
                AccessTokenFormat = new JwtFormat(tvps, new OpenIdConnectCachingSecurityTokenProvider(String.Format(aadInstance, tenant, policy))),
            };
        }
    }
}