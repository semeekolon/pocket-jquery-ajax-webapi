using Microsoft.Owin.Security.OAuth;
using Pocket.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Pocket.WebAPI.Providers
{
    public class AuthProvider : OAuthAuthorizationServerProvider
    {
        PocketDb db;

        public AuthProvider()
        {
            db = new PocketDb();
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext Context)
        {
            int nStatus = 0;
            string sError = "";

            //
            // Any Logic goes here
            //

            // returning Auth type and claims
            if (nStatus == 0)
            {
                Context.Validated();
            }
            else if (nStatus == 1)
            {
                Context.SetError("invalid_grant", sError);
                return;
            }
            else
            {


                Context.SetError("Server Error", sError);
                return;
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext Context)
        {
            Context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            bool bOk = true;
            int nStatus = 0;
            string sError = "";

            Int64 nId;
            string sUsername;
            string sPasswrd;

            nId = -1;
            sUsername = "";
            sPasswrd = "";


            ClaimsIdentity ClaimsInToken;



            if (bOk)
            {
                try
                {
                    sUsername = Context.UserName;
                    sPasswrd = Context.Password;
                }
                catch (Exception ex)
                {
                    bOk = false;
                    sError = "Incomplete authentication or authorization data.| Message: " + ex.Message;
                }
            }

            // Finding user
            if (nStatus == 0)
            {

                //  nStatus = oBalMUsers.udfChkLoginDetails(out sError, out nId, sPhoneNum, sPasswrd);

                chkLoginDets(out sError, out bOk, out nId, sUsername, sPasswrd);
            }

            // Returning Auth type and claims
            if (bOk)
            {
                ClaimsInToken = new ClaimsIdentity(Context.Options.AuthenticationType);

                ClaimsInToken.AddClaim(new Claim("sUserId", nId.ToString()));
                ClaimsInToken.AddClaim(new Claim("sUsername", sUsername));

                Context.Validated(ClaimsInToken);
            }
            else if (bOk)
            {
                Context.SetError("invalid_grant", sError);
                return;
            }
            else
            {


                Context.SetError("Server Error", sError);
                return;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public void chkLoginDets(out string psError, out bool bOk, out Int64 nId, string sUsername, string sPassword)
        {
            psError = "";
            bOk = false;
            nId = 0;

            List<Login> lsLoginDet = new List<Login>();

            lsLoginDet = (from LGNDT in db.Logins
                          where LGNDT.Username == sUsername
                          && LGNDT.Password == sPassword
                          select LGNDT).ToList();

            if (lsLoginDet.Count > 1)
            {
                bOk = false;
                psError = "More than one entry found with same EmailId(" + sUsername + ").";
            }

            else if (lsLoginDet.Count == 0)
            {
                psError = "Invalid login credentials.";
                bOk = false;
            }
            else
            {
                bOk = true;
                nId = lsLoginDet[0].Id;
            }


        }
    }
}