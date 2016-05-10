﻿using Brook.DuDuRiBao.Common;
using Brook.DuDuRiBao.Models;
using Brook.DuDuRiBao.Utils;
using System;
using WeiboSDKForWinRT;

namespace Brook.DuDuRiBao.Authorization
{
    [AuthoAttribution(LoginType.Sina)]
    public class SinaAuthorization : IAuthorize
    {
        public const string LoginDataKey = "LoginData";

        public readonly static SinaAuthorization Instance = new SinaAuthorization();

        private ClientOAuth _oauth = new ClientOAuth();

        static SinaAuthorization()
        {
            SdkData.AppKey = "2615126550";
            SdkData.AppSecret = "d8f2b0dc26390ddb844f45b5a6f69328";
            SdkData.RedirectUri = "http://sns.whalecloud.com/sina2/callback";
        }

        public SinaAuthorization()
        {
            LoginData loginData;
            if (StorageUtil.TryGetJsonObj(LoginDataKey, out loginData))
            {
                LoginData = loginData;
            }
        }

        public LoginData LoginData { get; private set; }

        private void UpdateLoginData(SdkAuth2Res res)
        {
            if (LoginData == null)
                LoginData = new LoginData();

            LoginData.access_token = res.AccessToken;
            LoginData.expires_in = int.Parse(res.ExpriesIn);
            LoginData.refresh_token = res.RefreshToken;
            LoginData.source = LoginType.Sina.Convert();
            LoginData.user = res.Uid;

            StorageUtil.AddObject(LoginDataKey, LoginData);
        }

        public bool IsAuthorized
        {
            get
            {
                return _oauth.IsAuthorized;
            }
        }

        public void Login(Action<bool, object> loginCallback)
        {
            _oauth.LoginCallback += (isSuccess, err, response) =>
            {
                if(isSuccess)
                {
                    UpdateLoginData(response);
                }
                loginCallback?.Invoke(isSuccess, response);
            };
            _oauth.BeginOAuth();
        }

        public void Logout()
        {
            StorageUtil.Remove(LoginDataKey);
        }
    }
}
