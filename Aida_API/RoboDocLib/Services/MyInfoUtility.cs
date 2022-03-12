using Newtonsoft.Json;

using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using RoboDocCore.Models;
using Jose;


using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;
using System.Configuration;

namespace RoboDocLib.Services
{
    public class MyInfoUtility
    {
        private string myInfoAttributes = "uinfin,name,aliasname,sex,race,nationality,dob,birthcountry,residentialstatus,regadd,email,mobileno,passtype";
        //private string myInfoAttributes = "uinfin,name,sex,race,nationality,dob,email,mobileno,regadd,housingtype,hdbtype,marital,edulevel,noa-basic,ownerprivate,cpfcontributions,cpfbalances";

        ////Test
        //private string myInfoClientId = "STG2-MYINFO-SELF-TEST";
        //private string myInfoClientSecret = "44d953c796cccebcec9bdc826852857ab412fbe2";

        //private string myInfoAPIAuthorise = "https://test.api.myinfo.gov.sg/com/v3/authorise";
        //private string myInfoAuthoriseCallback = "http://localhost:3001/callback";

        //private string myInfoAPIToken = "https://test.api.myinfo.gov.sg/com/v3/token";
        //private string myInfoTokenCallback = "http://localhost:3001/callback";

        //private string myInfoAPIPerson = "https://test.api.myinfo.gov.sg/com/v3/person";

        //private string myPrivateKeyXML = @"<RSAKeyValue><Modulus>xgUXbIg6ijxsh4Dqc8ZklNhEJJvs2GS75T5cJMMnxVi53RS2d7GsSUviyIsbBYbp661E/WWdMfmwejFeeaO9M+AATiURTVsdcGzBqnYgtBcn6cGnd4mKUVY2TvuXrRwPSRKGkz6Bcb/pLXfJFQC/T+SO/z4uoNUfgdrdpbGhNmzEsLWaoLOeYG+37Akd2eJ0IuwXXWEezGdpQDFn+4ew5JurkZRoZdkb7ZmGunVxZyt0YSKXUDlE96rsocHjBrKaMAv7WEhvgTYctVrpG8ISD+b0wL79Vmx04PlmlFFfqT9E4z0HGO93JR8TaSOc6zag5sA9q6IDX3jykVCOt8gVHw==</Modulus><Exponent>AQAB</Exponent><P>5BwM6NX6MESNfbZO1Qj59MzHRf1E0YIbS8sHP+MZ8NTXdd5vqKwKznVcz5UKJC1TS/NXSFSl5yHdB8P5F/AkckWM/LZC/NbKXN/Q32pWVu0COxbHIX6tR7YJMXf0BIwZSGqo5Doxkm1/cWI2FNQ2/D7hqBl1jrGIr/TwrCkdOes=</P><Q>3js32UqVeZF/EhAiV0QPnpsgpzvUtvadFvuQQk0Kdzj55my+2/5hqgVSdAwviIjeuy7vMcc3o7MKi2amkb5kLoqKh/vJjhqV2GcIUoQIn9gLoscby+ziunbw5jKqCoQSPZ2fflyOtAnfy451oMhTtOLquKLZ07pJmLEj/n6esJ0=</Q><DP>SilgfRaMV8MB6VwrNxHLCi7Fntif1imhrDue6MNx/J2GS1j9Hm6IuOTpPsfq2yYtTaUYvHhg08trrAmn44N8pfY8xzCOiNtgQV+27dwJpAs8TW+zYA1qUsU5Ke8g05Wk5qlerYzJ7xxQcdGHWbBBDDN5dAiK8tB/aw2MbbyUNu8=</DP><DQ>Csh80UJZNmjk7Y9y2yEmUN/eGb9Bdw9IWBEk0tLCKz7MgW3NZQdW3dUcRx1AQTPC+vowCQ5NmNfbLyBv/KpsWgXG6wpAoXCQzMtTEA3wDTGCfweCRcbcyYdz8PeMYK4/5FV9o7gCBKJmBY6IDqEpzqEkGolsYGWtpIcT5Alo0dE=</DQ><InverseQ>3hzC9NLwumi/1JWm+ASSADTO3rrGo9hicG/WKGzSHD5l1f+IO1SfmUN/6i2JjcnE07eYArNrCfbMgkFavj502ne2fSaYM4p0o147O9Ty8jCyY9vuh/ZGid6qUe3TBI6/okWfmYw6FVbRpNfVEeG7kPfkDW/JdH7qkWTFbh3eH1k=</InverseQ><D>Gy/7xVT25J/jLr+OcRLeIGmJAZW+8P7zpUfoksuQnFHQQwBjBRAJ3Y5jtrESprGdUFRb0oavDHuBtWUt2XmXspWgtRn1xC8sXZExDdxmJRPA0SFbgtgJe51gm3uDmarullPK0lCUqS92Ll3x58ZQfgGdeIHrGP3p84Q/Rk6bGcObcPhDYWSOYKm4i2DPM01bnZG2z4BcrWSseOmeWUxqZcMlGz9GAyepUU/EoqRIHxw/2Y+TGus1JSy5DdhPE0HAEWKZH729ZdoyikOZCMxApQglUkRwkwhtXzVAemm6OSoy3BEWvSEJh/F82tFrmquUoe/xd5JastlBHyD78RAakQ==</D></RSAKeyValue>";
        //private string myInfoCertificate = @"staging_myinfo_public_cert.cer";
        //private string myInfoCertificatePFX = @"stgdemoappclientpubliccert2018.pfx";
        //private string myInfoValidIssuer = @"https://test.api.myinfo.gov.sg/serviceauth/myinfo-com";

        //SIT
        private string myInfoClientId = "STG-201415822C-ACHIBIZ-BIZOFFICERREG";
        private string myInfoClientSecret = "m5ab2f2GYnmNrNDDVPydwJv3MSVhnQOZ";

        private string myInfoAPIAuthorise = "https://sandbox.api.myinfo.gov.sg/com/v3/authorise";
        private string myInfoAuthoriseCallback = "https://aidasit.achibiz.com/myinfo-callback";

        private string myInfoAPIToken = "https://sandbox.api.myinfo.gov.sg/com/v3/token";
        private string myInfoTokenCallback = "https://aidasit.achibiz.com/myinfo-callback";

        private string myInfoAPIPerson = "https://sandbox.api.myinfo.gov.sg/com/v3/person";

        private string myPrivateKeyXML = @"<RSAKeyValue><Modulus>s6cVPisAo1ww/3jxhyC8qJp+udYnj9fhPMmYxR6pr/YeEgNJrf+jrm8pfZdJuVgJ0me4w2YsbGKAI3bC4lTS7jI8XBCOwO8Xl1HluOLql8VQsxdJIrnP5w16qHPdCbDxIq7prvp9cmML9mMBSSYAqHDLQMgawg2FXmxi7AG31T7dF83+ggxvikeH4WN5HV4kDNVO1uP3XOKvrp9/Fyj9VvQD9UR0l9PPdMpz1Kb/3ZoWyliL/vElrPrMAzUC3vXsETz1UrxjzXXeXKJxrLNqueG1ya504W1RiH1ytGTMS+XvLPl0fyXhvKAufNW69NJmUUpAAT364HrzqwgTStNxtw==</Modulus><Exponent>AQAB</Exponent><P>6yI/sk0lpVOdotD3seKMBKeQq7dzn5an5TSlQPzPcd+Q9HuX8zl2+jleozJYM/IH+s5yWiknM+vUvAaXPECu2Cr1E2kY7pLpGrzpJnUR0pm/5B5MhuzKT6ianoyH2wo9FBNnr1AziGYV7+r+fMNMG2ecw3csWc7BDKY2ypbVtqU=</P><Q>w5hrHrjMMe6JZZsYKqEyprS7VEtDf1AVEA8ZsQavQyW2JWgODBZgcIbkgrwqmygzbno8TGDLZugnGdsFinZXzi95gLnMssgVue4lRXelgoxQ6+BVUHSD5AieCiZQbMtZkfcfI1lmm1xglon5u8uCCSWIgwVSzdTepr/8hjiPdCs=</Q><DP>IOSZpIfGTGp2EuDIxcm7bKtEQcFYG2sn8Zh7xauYGYX88TaY+x1+12JPKuVQEXQ6SYu304UORuUmzWbeoT8rNnJL9nstUbKbiXID661U4PFBNthGPFnDs4ESoDtohevYv4Y3av79NoeRag6lqy5m5y6Hb5CsU6PovaxC3fRb0UE=</DP><DQ>hSE2MAf6PDYu4LCK52Emt58mLj64J2ybmX2dsAdrvVlqu9UyUnnclFrMKEvo+AiJ0TVz20y6xUVYb9nK0K6yHw3jNoHAWSOk3hPA3KKOTegxoArmJsfXiZtv7lqbvSE/ywEX+Zh9JVvgBjbNe0wOKymX3A4AiUbNcVQVjknrIlc=</DQ><InverseQ>IdrFresLgcmyvv580oTNoaO58FafxjTkYX7OOWgDjlbJ9MJv6xi447FzWqHn9gpDQ5RnBZfhfxgviL19DXYqxL+fADYYFPUmmZ8lT/Zc34mvQB/SNuuR1vU36bJT36Srd18NOcoNrkvcKS8Csc4UtewNTjPBpuRwsidHYqQxrtM=</InverseQ><D>DTtEQRi+tR7ATG3rxWA1Cj/yuceFkL5oIZIsKnHTMplbEUg6YCfWC4NhbWq77HwzoP0Fqn03JAvEf0LVy1X5q78kcJgW7ivN3isTKHZXVofwT82/vmCBaPWLr/t0mgGfs5u+rKYDS5jy6RnVSZNpyf4gt4E6w0m7+xPa3KIAbaKnbFxVExu2/9zw4O3LDgok7ppLpYHgtsxEOhnsGg4ubGGNYxsmF9Va1MGrDEUPGFyRRTMDqjBofonstoBgBzBEFK4yPJPsggIKwzrxNM4hXV/YhNzFft8L026GvZghVhYJ1LvXooLmd5XOK+IgeANelU6EgsaL3U1n+DR+rG5B/Q==</D></RSAKeyValue>";
        private string myInfoCertificate = @"aidasit.cer";
        private string myInfoCertificatePFX = @"aidasit.pfx";
        private string myInfoValidIssuer = @"https://sandbox.api.myinfo.gov.sg/serviceauth/myinfo-com";

        ////Live
        //private string myInfoClientId = "PROD-201415822C-ACHIBIZ-BIZOFFICERREG";
        //private string myInfoClientSecret = "6zvHfzPNkQzNn6YG0134wOweMwKXdfH8";

        //private string myInfoAPIAuthorise = "https://api.myinfo.gov.sg/com/v3/authorise";
        //private string myInfoAuthoriseCallback = "https://aida.achibiz.com/myinfo-callback";

        //private string myInfoAPIToken = "https://api.myinfo.gov.sg/com/v3/token";
        //private string myInfoTokenCallback = "https://aida.achibiz.com/myinfo-callback";

        //private string myInfoAPIPerson = "https://api.myinfo.gov.sg/com/v3/person";

        //private string myPrivateKeyXML = @"<RSAKeyValue><Modulus>v2PJh+JP51EuAJV0yRJAAqNHF6WIYYi9DPg7t8/GOwuSMM7xIV870KWz+NTHSyM/pFLvDJri634P3VSTklnuqdbNxT0NdflMzw1nmz4J0mDVWr1TvVZPGR5X/aDOcgrqmzNB8wWtK4vgmQz21J+cUBcl6dqt9Kf7uiPBQ1l803JErJBzQ3VFudXTO07yxI88gTTPrDJn8SPXgliFcjxnIrWV4dCUeOQpHYvwsjBzeMU99k0/LNpXPIcXBKDsWEyyzKLWkIxtWuMOjjmbQ9M5XPQde/2RgCuOjiD9Bua6/WVxEHJCWqAjrInOOsSba271JwPteiTuGa2x/ZtLNhda5w==</Modulus><Exponent>AQAB</Exponent><P>82dlxRL/5sLrR8H8j3Yaf/ubo4luCA/tgH5h81Fj+3iyPb7lgN8i6SWCENlNiA/PgYoB1VuRTRu8YT1aod9AsVcSp3aQR/EhM/49Cy7BSBQ0d4zMgieNSNa7rTO1RMpMI/N1WyJAklcYzKExqEqU5nwNaW0HvnguF9aAoMJSk58=</P><Q>yUtPO1jKkXdy4znRt3C4O0SxFIK2TCeRrM03lmGmmQywtzaElUpohVhCTkhjQOmCXKQlboT+95McOYXenNqJCXmmBWSyfOp59QbMfAfJOE3nYJecN98RzabkcgojoGeygrTi+fPqCdbTcOankCbd5QT8TV/xQtmzddILUxu+M7k=</Q><DP>eDydg0qtatriyuHC10NqN8qcyZ474nOryQula0LLdw/aTLIeQRj47979b0FJEWtt//miijjfgA2xcv+R4+Ca3n+60Uq6Xy94TWzLzthb1jVU7bF53NmY62BX+WlPpBminyfG0ywKZb/smjkOhKnDAFBLd8bKSoanjlMB22fiztU=</DP><DQ>mbbWtF0jo13x767xhQGgzjGwgBcpGpKE3c6AimfbjrhwIhiRumdt1ZYGXD1O/fzTPedjZjhNc8V1IkAmNOV1DDVHIIs6+cUciI4+jOBtOn/TFqHv+4Ju1/qALM2tUwqc+M8lHPa0W/sez/JzNtNXU1IxUOzZEdGpB2n0l+xxnJE=</DQ><InverseQ>KUGq/1zVw4OPi7Q1e6+sQxbjVdpK7zD6eMqlC7w2hi90165lybjvKnaPqORvHW25HlZKPXjYRz4SLJn1NDQSEvXA2vvII9Hj9TmbfardSiUdz51BjDVJXP5eVTuRLudwJ7JPwGhLKHTWcbrTuWXmwY9CeBiq5nLdrE6rUXuUBHE=</InverseQ><D>MDb1SDQtKPr8ec0s9JKSaZe0izJ6XwfRiHjoom/HYoyRsZYQjPWu3etAoYkJA9bHa6Aw9J34WJcTbBCodJAPXd3tTKkKwROi0+5ukV9ZnhQi648Sj3o0xwEr7YsbNwOlAqAxFwXxEC1W0XfAnuPuvwZ12KnsdNv0wHnNcCgVGDcbKkisb8RprJj6W7UQlvWsIivV985UDcp7zvrh9NKaWmO8gpF3dInHygS+mQZwb1cjZAqdeiSW8/Zm+xshU/NugZoGE9OQjTARlTP1eTs+zoK5RU0L02UdwhqUxNelam5kw0D00vBAyBbtowVFk/CAlX2pVIriRZsZ9Au/Vz+g4Q==</D></RSAKeyValue>";
        //private string myInfoCertificate= @"aida_myinfo.cer";
        //private string myInfoCertificatePFX= @"aida_myinfo.pfx";

        string connectionString = "";
        int UserId = 99;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public MyInfoUtility(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

            if(util.isLive)
            {
                myInfoClientId = "PROD-201415822C-ACHIBIZ-BIZOFFICERREG";
                myInfoClientSecret = "6zvHfzPNkQzNn6YG0134wOweMwKXdfH8";

                myInfoAPIAuthorise = "https://api.myinfo.gov.sg/com/v3/authorise";
                myInfoAuthoriseCallback = "https://aida.achibiz.com/myinfo-callback";

                myInfoAPIToken = "https://api.myinfo.gov.sg/com/v3/token";
                myInfoTokenCallback = "https://aida.achibiz.com/myinfo-callback";

                myInfoAPIPerson = "https://api.myinfo.gov.sg/com/v3/person";

                myPrivateKeyXML = @"<RSAKeyValue><Modulus>v2PJh+JP51EuAJV0yRJAAqNHF6WIYYi9DPg7t8/GOwuSMM7xIV870KWz+NTHSyM/pFLvDJri634P3VSTklnuqdbNxT0NdflMzw1nmz4J0mDVWr1TvVZPGR5X/aDOcgrqmzNB8wWtK4vgmQz21J+cUBcl6dqt9Kf7uiPBQ1l803JErJBzQ3VFudXTO07yxI88gTTPrDJn8SPXgliFcjxnIrWV4dCUeOQpHYvwsjBzeMU99k0/LNpXPIcXBKDsWEyyzKLWkIxtWuMOjjmbQ9M5XPQde/2RgCuOjiD9Bua6/WVxEHJCWqAjrInOOsSba271JwPteiTuGa2x/ZtLNhda5w==</Modulus><Exponent>AQAB</Exponent><P>82dlxRL/5sLrR8H8j3Yaf/ubo4luCA/tgH5h81Fj+3iyPb7lgN8i6SWCENlNiA/PgYoB1VuRTRu8YT1aod9AsVcSp3aQR/EhM/49Cy7BSBQ0d4zMgieNSNa7rTO1RMpMI/N1WyJAklcYzKExqEqU5nwNaW0HvnguF9aAoMJSk58=</P><Q>yUtPO1jKkXdy4znRt3C4O0SxFIK2TCeRrM03lmGmmQywtzaElUpohVhCTkhjQOmCXKQlboT+95McOYXenNqJCXmmBWSyfOp59QbMfAfJOE3nYJecN98RzabkcgojoGeygrTi+fPqCdbTcOankCbd5QT8TV/xQtmzddILUxu+M7k=</Q><DP>eDydg0qtatriyuHC10NqN8qcyZ474nOryQula0LLdw/aTLIeQRj47979b0FJEWtt//miijjfgA2xcv+R4+Ca3n+60Uq6Xy94TWzLzthb1jVU7bF53NmY62BX+WlPpBminyfG0ywKZb/smjkOhKnDAFBLd8bKSoanjlMB22fiztU=</DP><DQ>mbbWtF0jo13x767xhQGgzjGwgBcpGpKE3c6AimfbjrhwIhiRumdt1ZYGXD1O/fzTPedjZjhNc8V1IkAmNOV1DDVHIIs6+cUciI4+jOBtOn/TFqHv+4Ju1/qALM2tUwqc+M8lHPa0W/sez/JzNtNXU1IxUOzZEdGpB2n0l+xxnJE=</DQ><InverseQ>KUGq/1zVw4OPi7Q1e6+sQxbjVdpK7zD6eMqlC7w2hi90165lybjvKnaPqORvHW25HlZKPXjYRz4SLJn1NDQSEvXA2vvII9Hj9TmbfardSiUdz51BjDVJXP5eVTuRLudwJ7JPwGhLKHTWcbrTuWXmwY9CeBiq5nLdrE6rUXuUBHE=</InverseQ><D>MDb1SDQtKPr8ec0s9JKSaZe0izJ6XwfRiHjoom/HYoyRsZYQjPWu3etAoYkJA9bHa6Aw9J34WJcTbBCodJAPXd3tTKkKwROi0+5ukV9ZnhQi648Sj3o0xwEr7YsbNwOlAqAxFwXxEC1W0XfAnuPuvwZ12KnsdNv0wHnNcCgVGDcbKkisb8RprJj6W7UQlvWsIivV985UDcp7zvrh9NKaWmO8gpF3dInHygS+mQZwb1cjZAqdeiSW8/Zm+xshU/NugZoGE9OQjTARlTP1eTs+zoK5RU0L02UdwhqUxNelam5kw0D00vBAyBbtowVFk/CAlX2pVIriRZsZ9Au/Vz+g4Q==</D></RSAKeyValue>";
                myInfoCertificate = @"aida_myinfo.cer";
                myInfoCertificatePFX = @"aida_myinfo.pfx";
                myInfoValidIssuer = @"https://api.myinfo.gov.sg/serviceauth/myinfo-com";
            }    

        }

        public string GetAuthorise(int BusinessProfileId)
        {
            Random rnd = new Random();
            int RequestId = rnd.Next(100000, 999900);
            int state = 0;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"insert into MyInfoRequest(RequestId,CreatedDate,BusinessProfileId)  " +
                               " values (@RequestId,Getdate(),@BusinessProfileId) ";

                var result = db.Execute(sqlQuery, new
                {
                    RequestId,
                    BusinessProfileId
                });
                sqlQuery = @"select State from MyInfoRequest where RequestId =@RequestId";
                state = db.QuerySingle<int>(sqlQuery, new { RequestId });
               

            }

            logger.Info(Util.ClientIP + "|" + "My infor Request generated|"+ myInfoAPIAuthorise + "?client_id=" + myInfoClientId +
                "&attributes=" + myInfoAttributes +
                "&purpose=Incorporation of any Business.&state=" + state +
                "&redirect_uri=" + myInfoAuthoriseCallback);
            
            return myInfoAPIAuthorise + "?client_id=" + myInfoClientId +
                "&attributes=" + myInfoAttributes +
                "&purpose=Incorporation of any Business.&state=" + state +
                "&redirect_uri=" + myInfoAuthoriseCallback;
        }


        public ResponseModel GetAuthoriseInvite(int businessProfileId,string email)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            Random rnd = new Random();
            int RequestId = rnd.Next(100000, 999900);
            //int RequestId1 = rnd.Next(100000, 999900);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                email = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(email));
                string sqlQuery = @"insert into MyInfoRequest(RequestId,CreatedDate,BusinessProfileId,email)  " +
                               " values (@RequestId,Getdate(),@BusinessProfileId,@email) ";

                var result = db.Execute(sqlQuery, new
                {
                    RequestId,
                    businessProfileId,
                    email
                });

                response = SendMyInfoInvite(email, RequestId);
            }

            logger.Info(Util.ClientIP + "|" + "My infor Request invite set to |" + email);

            return response;
        }


        public ResponseModel SendMyInfoInvite(string email, int requestId)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (email != null)
                {
                    string emailEncrypted = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(email));

                    string body = "	<head><title>Corporate Secretarial Services in Singapore | Achi Biz</title></head>" + Environment.NewLine +
"	<body style='font-family:Calibri;font-size:16px;width:188mm;'>" + Environment.NewLine +
"		<p><br>Hi, Greetings! <br><br><a href='" + ConfigurationManager.AppSettings["MyPath"] + "/myinfo-filling/invite/" + requestId + "/" + emailEncrypted + "' >Click this link to login.</a><br><br>Steps to follow:</p>" + Environment.NewLine +
"		<UL><li>Click the above link to login MyInfo</li>" + Environment.NewLine +
"		<li>You need OTP for login.</li>" + Environment.NewLine +
"		<li>Check your email again for OTP.</li>" + Environment.NewLine +
"		<li>Enter OTP to Login.</li>" + Environment.NewLine +
"		<li>Click: Continue to MyInfo.</li>" + Environment.NewLine +
"		<li>Enter your SingPass credential.</li>" + Environment.NewLine +
"		<li>Ensure the information for sharing.</li>" + Environment.NewLine +
"		<li>Click: I Agree. </li>" + Environment.NewLine +
"		<li>Click: Submit to fetch the data.</li>" + Environment.NewLine +
"		<li>Close after seeing capture message.</li>" + Environment.NewLine +
"		<li>Exit from MyInfo.</li>" + Environment.NewLine +
"		<li>Close the Browser & clear the cache.</li>" + Environment.NewLine +
"		<p> Thank You! Have A Nice Day! <br> <br> " + Environment.NewLine +
"		ACHI KUMAR, Director <br>" + Environment.NewLine +
"		ACHI BIZ SERVICES PTE. LTD. <br>" + Environment.NewLine +
"		ACRA UEN: 201415822C & MOM EAL: 18C9185 <br>" + Environment.NewLine +
"		<a href='https://achibiz.com'>https://achibiz.com</a> " + Environment.NewLine +
"		</p>" + Environment.NewLine +
"</body>";

                        /*
                        "<head>" +
                    "</head>" +
                    "<body>" +
                        "<table width='80%' align='center'>" + Environment.NewLine +
                        "<tr><td><a href='" + ConfigurationManager.AppSettings["MyPath"] + "/myinfo-filling/invite/" + requestId + "/" + emailEncrypted + "' ><img alt='' src='https://aida.achibiz.com/images/myinfo-request-header.png' ></a></td></tr>" + Environment.NewLine +
                        "<tr><td><a href='https://achibiz.com'><img alt='' src='https://aida.achibiz.com/images/myinfo-request-body.png' ></a></td></tr>" + Environment.NewLine +
                        "</table></body></html>";
                    /*
                        "<h1>Fill the details using MyInfo.</h1>" + Environment.NewLine +
                        "<p>Dear User, </p>" + Environment.NewLine +
                        "<p>Pleas follow this hyperlink to fill your detail using MyInfo.</p>" + Environment.NewLine +
                        "<a href='" + ConfigurationManager.AppSettings["MyPath"] + "/myinfo-filling/invite/" + requestId + "/" + emailEncrypted + "' > Click here to continue</a>" + "</body>";
                    */
                    MailClient mail = new MailClient();


                    string emailResponse = mail.SendMail("Mail from AIDA", email, "Form filling using MyInfo ", body);
                    if (emailResponse.Equals("OKAY"))
                    {
                        response.IsSuccess = true;
                        response.Message = "Email sent to " + email;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = emailResponse;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Mail Not sent problem in email";
                }

                logger.Info(Util.ClientIP + "|" + "Form filling using MyInfo for email id " + email + " and response is " + response.Message);

            }

            return response;
        }

        public ResponseModel GetAuthoriseOTP(int requestId, string email)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Login failed" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //email = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(email));

                string sqlQuery = @"select isnull(email,'') " +
                            " from MyInfoRequest where requestId=@requestId and otp is null";

                string Email = db.QuerySingleOrDefault<string>(sqlQuery, new { requestId});
                if (Email != null && Email.Length > 0)
                {
                    sqlQuery = @"update MyInfoRequest set OTP=@OTP,OTPDate = getdate() where requestId=@requestId and otp is null";
                    string OTP = Guid.NewGuid().ToString();
                    OTP = OTP.Replace("-", "");
                    if (OTP.Length > 8)
                        OTP = OTP.Substring(0, 8);

                    db.Execute(sqlQuery, new { OTP , requestId});
                    response = SendLoginOTP(Email, OTP);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not a valid credential or already used";

                }

                logger.Info(Util.ClientIP + "|" + "OTP send and response is " + response.Message);

            }

            if (response == null)
                response = new LoginResponseModel() { IsSuccess = false, Message = "Login failed" };

            logger.Info(Util.ClientIP + "|" + "Login status " + response.Message);

            return response;
        }

        public ResponseModel SendLoginOTP(string email, string otp)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (email != null)
                {

                    string body = "<head>" +
                    "</head>" +
                    "<body>" +

                        "<h1>Form filling OTP.</h1>" + Environment.NewLine +
                        "<p>Dear User, </p>" + Environment.NewLine +
                        "<p>Your OTP is <b>" + otp + "</b>. It will expire in 10 minutes. Please use this for form filling.</p>" + Environment.NewLine +

                        "</body>";

                    MailClient mail = new MailClient();


                    string emailResponse = mail.SendMail("Mail from AIDA", email, "OTP for form filling", body);
                    if (emailResponse.Equals("OKAY"))
                    {
                        response.IsSuccess = true;
                        response.Message = "Email sent to " + email;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = emailResponse;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Mail Not sent problem in email";
                }

                logger.Info(Util.ClientIP + "|" + "Login OTP sent for email id " + email + " and response is " + response.Message);

            }

            return response;
        }

        public string GetAuthoriseWithOTP(int requestId, string email,string OTP)
        {
            string state = "Request failed or expired";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //email = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(email));

                string sqlQuery = @"select State from MyInfoRequest " +
                                    " where requestId=@requestId and " +
                                    " otp=@otp and datediff(n,otpdate,getdate()) <=10 ";
                state = db.QueryFirstOrDefault<string>(sqlQuery, new { OTP, requestId });
                if (state != null)
                {
                    logger.Info(Util.ClientIP + "|" + "My infor Request generated|" + myInfoAPIAuthorise + "?client_id=" + myInfoClientId +
                    "&attributes=" + myInfoAttributes +
                    "&purpose=Incorporation of any Business.&state=" + state +
                    "&redirect_uri=" + myInfoAuthoriseCallback);

                    state= myInfoAPIAuthorise + "?client_id=" + myInfoClientId +
                        "&attributes=" + myInfoAttributes +
                        "&purpose=Incorporation of any Business.&state=" + state +
                        "&redirect_uri=" + myInfoAuthoriseCallback;
                }

            }

            logger.Info(Util.ClientIP + "|" + "Status " + state);

            return state;
        }


        public List<DropDownModel> GetPerson(string path, string code, string state)
        {
            try
            {
                logger.Info(Util.ClientIP + "|" + "GetPerson| code " + code + ", state" + state);

                string tstamp = ((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds * 1000) + "";
                tstamp = tstamp.Split('.')[0];

                DateTime now = DateTime.UtcNow;
                long unixTimeMilliseconds = new DateTimeOffset(now).ToUnixTimeMilliseconds();

                DefaultApexHeaders requestHeader = new DefaultApexHeaders()
                {
                    code = code,
                    client_id = myInfoClientId,
                    client_secret = myInfoClientSecret,
                    grant_type = "authorization_code",
                    redirect_uri = myInfoTokenCallback,
                    app_id = myInfoClientId,
                    nonce = tstamp + "00",
                    signature_method = "RS256",
                    timestamp = tstamp + ""
                };

                string jsonString = JsonConvert.SerializeObject(requestHeader, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                jsonString = jsonString.Replace("\":\"", "=").Replace("\",\"", "&").Replace("{\"", "").Replace("\"}", "");

                string baseString = "POST&" + myInfoAPIToken + "&" + jsonString;

                RSA rsa = RSA.Create();
                
                rsa.FromXmlString(myPrivateKeyXML);
                byte[] Sign = rsa.SignData(Encoding.ASCII.GetBytes(baseString), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                string signed = System.Convert.ToBase64String(Sign);
                logger.Info(Util.ClientIP + "|" + "Signing done ");

                var strApexHeader = "PKI_SIGN timestamp=\"" + requestHeader.timestamp +
                    "\",nonce=\"" + requestHeader.nonce +
                    "\",app_id=\"" + requestHeader.app_id +
                    "\",signature_method=\"RS256\"" +
                    ",signature=\"" + signed +
                    "\"";

                logger.Info(Util.ClientIP + "|" + "GetPerson| ApexHeader " + strApexHeader);

                /*
                ApexParams apex = new ApexParams()
                {
                    grant_type = requestHeader.grant_type,
                    code = requestHeader.code,
                    redirect_uri = requestHeader.redirect_uri,
                    client_id = requestHeader.client_id,
                    client_secret = requestHeader.client_secret
                };

                Random rnd = new Random();
                int myRandomNo = rnd.Next(1000, 9999);

                jsonString = "grant_type=" + requestHeader.grant_type +
                    "&code=" + requestHeader.code +
                    "&redirect_uri=" + requestHeader.redirect_uri +
                    "&client_id=" + requestHeader.client_id +
                    "&client_secret=" + requestHeader.client_secret;
                */

                var client = new RestClient(myInfoAPIToken);

                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", strApexHeader);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                request.AddParameter("grant_type", requestHeader.grant_type);
                request.AddParameter("code", requestHeader.code);
                request.AddParameter("redirect_uri", requestHeader.redirect_uri);
                request.AddParameter("client_id", requestHeader.client_id);
                request.AddParameter("client_secret", requestHeader.client_secret);

                IRestResponse response = client.Execute(request);

                jsonString = response.Content;

                logger.Info(Util.ClientIP + "|" + "Response | " + jsonString);

                //string jsonString = "{\"access_token\":\"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MTUyNjdmMC01OTM5LTAyMzAtNzhlNy1iOGNkYmFhYjg1MTgiLCJqdGkiOiJMdEQ5U3F0eUZvMFQ1UW9OSHVVZ1JSSUM4a3p0aUpEUk1Ucl9XT1dYIiwic2NvcGUiOlsibmFtZSIsInNleCIsIm5hdGlvbmFsaXR5IiwiZG9iIiwiYmlydGhjb3VudHJ5IiwidWluZmluIiwicGFzc3BvcnRudW1iZXIiLCJwYXNzcG9ydGV4cGlyeWRhdGUiLCJyZWdhZGQiLCJtb2JpbGVubyIsIm1haWxhZGQiLCJob21lbm8iLCJlbWFpbCJdLCJ0b2tlbk5hbWUiOiJhY2Nlc3NfdG9rZW4iLCJ0b2tlbl90eXBlIjoiQmVhcmVyIiwiZ3JhbnRfdHlwZSI6ImF1dGhvcml6YXRpb25fY29kZSIsImV4cGlyZXNfaW4iOjE4MDAsImF1ZCI6IlNURzItTVlJTkZPLVNFTEYtVEVTVCIsInJlYWxtIjoibXlpbmZvLWNvbSIsImlzcyI6Imh0dHBzOi8vdGVzdC5hcGkubXlpbmZvLmdvdi5zZy9zZXJ2aWNlYXV0aC9teWluZm8tY29tIiwiaWF0IjoxNTg1NTAxNDAxLCJuYmYiOjE1ODU1MDE0MDEsImV4cCI6MTU4NTUwMzIwMX0.OP-yPD9t3NOPGLBRSSQNIfwwSPRFMxvoiJ8twH8YZQRrR6JCAgQzEc-Ob2LjKkQjDG_EgpNg8UVaP574OZ-uNuHuHLfuz4JOGeNtEsyjf0q4lIsCw3pSqLispfeHcrPvvsmjILA4Vnrbfn0BhC91ESoJsSR7H4uh6Av2irE2gLwhfMy2bxbffbKuvPon4qDLSwOcg7Wk9MjCJ9oHDUeTeZOpLc2k-FdL4bzlQzkwTQpwMwVqHWUnkpwWxFI9G5gdhOo_w_lf_NpvT1Z8zwRiQIY3eAicMTBGLisW4UytHOMt8fOLShHRvb8ncOH1BwMv81xJ5fzrKq07gK3gvkKt-Q\",\"token_type\":\"Bearer\",\"expires_in\":1799,\"scope\":\"name sex nationality dob birthcountry uinfin passportnumber passportexpirydate regadd mobileno mailadd homeno email\"}";

                AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(jsonString);
                JwtSecurityToken tokenReceived = new JwtSecurityToken(accessToken.access_token);

                X509Certificate2 certificate2 = new X509Certificate2(path + @"\ssl\" + myInfoCertificate);
                X509SecurityKey securityKey = new X509SecurityKey(certificate2);
                RSACryptoServiceProvider publicOnly = (RSACryptoServiceProvider)certificate2.PublicKey.Key;

                SecurityToken validatedToken;
                TokenValidationParameters validationParameters = new TokenValidationParameters();
                validationParameters.IssuerSigningKey = securityKey;
                validationParameters.ValidAudience = myInfoClientId;
                //validationParameters.ValidIssuer = myInfoValidIssuer;

                /*
                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(accessToken.access_token, validationParameters, out validatedToken);
                string sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                */
                var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken.access_token);

                string personAPI = myInfoAPIPerson + "/" + token.Subject + "/";


                tstamp = ((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds * 1000) + "";
                tstamp = tstamp.Split('.')[0];

                baseString = "GET&" + personAPI +
                    "&app_id=" + myInfoClientId +
                    "&attributes=" + myInfoAttributes +
                    "&client_id=" + myInfoClientId +
                    "&nonce=" + tstamp + "00" +
                    "&signature_method=RS256" +
                    "&timestamp=" + tstamp;

                Sign = rsa.SignData(Encoding.ASCII.GetBytes(baseString), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                signed = System.Convert.ToBase64String(Sign);


                strApexHeader = "PKI_SIGN timestamp=\"" + tstamp +
                    "\",nonce=\"" + tstamp + "00" +
                    "\",app_id=\"" + myInfoClientId +
                    "\",signature_method=\"RS256\"" +
                    ",signature=\"" + signed + "\"" +
                    ",Bearer " + accessToken.access_token;

                logger.Info(Util.ClientIP + "|" + "new request | " + personAPI);
                client = new RestClient(personAPI);

                request = new RestRequest(Method.GET);

                request.AddHeader("Authorization", strApexHeader);
                request.AddParameter("client_id", myInfoClientId);
                request.AddParameter("attributes", myInfoAttributes);

                response = client.Execute(request);

                jsonString = response.Content;

                logger.Info(Util.ClientIP + "|" + "new response | " + jsonString);

                //string jsonString = "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZHQ00iLCJraWQiOiJzdGctbXlpbmZvIn0.XUGnb3FlKNP_Yh-9y2VpdruUJgp_valzjpZCWoXyKf6v6fR1oDQEJuLUiaVCQTYyYCzrNvtTIemJRv1uerlvsrDHXs5J7XiYnbylMHNBUKdFP2NwExuON3UEa_5JcdXgEalpCCXvAE9eNwLQSd5CNF5XdeJpdEh9CRzUppzaCvAyzWz80roVaDeRnwx0c5uaf7-iQIWGS7uRtZ8mnZ6iLN3NT4gc9RUdAVQr6iHC0Wfv50yc1xiluiryw1AFKm8z1aL1o-OE38kzJDoKgoVLjFunrSpAoK1PNTwTX_9557lDbhQQwfp8qylHjH5SRxJTGRmBUxheaqWxr5NyoFE_Dg.IGHB6aIkEpZBUHPq.vSpCedkyPbqc47-kNv-nALuDaaxht3TucJVlb6xy296CLQtbWtT-4dWmcSCwhi-794s1uSwTlZDsMjCnp_9CnIofMzAE6SsPcNsXKEtKpK3Kijuauq82VeQpLq_m22IoGk2riEUlJxxQkAQqfT1NUCZbDA7jqkXwNqdd9W2yNilCSOYYMP-KcrgmQUw8FYnnMyNtZBesHJ2QjToczC1p0u-zjcRZirKut5bHY5Tv6d6VBdW-xmRnMjZBTgAQDoo-qjccxRaoG1KqrJ6JdA2AXHJAtobEynyBK-MS0HaCwGzzHFb09RuLPaE51n6PRvkyvq8NaQNXb78VI4BtPrcDkAQ046RFeZj_J_D24wrT-DDREngilSk-5RQ6zzUFMmvsg-IwnZ7EL2Y6OdxHtnt5NnXVc2bLnxEdOn0tBuUt8zDzkTThPAGIZKEJ9fND9RtaEZlB2eZadrri8XY7sk1WJCJlPorvUVN4aRmUA2L1B8uHfsU33NTxTNzu0RcRc5fiTe7RgBHWxxZJVlWAaEeRRMhoGuMB4te-4lWwcUH089XUYCMTf0VS4BORYwCCyvm172Ozt3whW6drXcgE-ifcgIFnnBQBRR8PvoUQucjTdt0nkr3xJ77YgzyKMS2aG_rVKPM84-6UjKQZWmknZTZTvvYOCqILZAXEDTuM37Wea6XFZ32ei4BDJSK-onmCf5lbtDJ4qm-SaS2KGeuGnBFVHo_qWI1zc98NUMYoJu38oOPrVr0aHSc3eteaEosd49fBnX3SFUu5xhKW5zYIL211KMAh1FnmVngL-kt2r89R7rxXiFxrhWlkD4rkperti0ILpzvzjuc2whPPKtJiDrKzb-tgb7VaYBY_EOH_yfMKXt-jwmPW8XtBVOdTM09AwN1JUJc9c0F7HhAKX097w0428NM9GE3jJ1fNRNZ9MeTtsxWyoe5lossS2W81tiCnijKtACW0BkejfC8oHdeXdKI8ClLR6__2pfVQbTHb5FcaVm4E_MZq8sD0uis2gA0CW3i5TSEiP1o7TO7gz0DbfgwPZ3I0sqTORwAQMN4bcXTzYIGdxciH_cN4Rg4jiLIC_8TiQxU-q-pSA8bGfKTBmBImS97uxm-bk63CmsZdFR4WGu42yA6VnDT-pktYmNZUzFHFta2PkF5NFdbG68kyTwXMnpQSUNXMhUBUkGTFEGSU5LWMP4umGCrDIrQHwuJHnIrV7q_tT87yu6JkxRrvyHp8kt57YWdjXQy0id_gCtSNOjFrMASzn22t3_KQQbhC-1OgKiOqAMRFUtlcC3JYb0FAEZ3pkf33PFVLCP1JtBEwA1Tg8jvOiQ35puRVmtc54JveserzfByvur-IfQYQgjVw0vsmpq_bNbLxJP1Zxze-Bd0KMmpiYUbjJY3_ebYAhslhJbh-nn17PVNdEi90_7MHiWrsiAFK5XrZc00S4pk_-Ui-aJMyERYyPwllM_qbdd2_FN829fBxMIP_FLjryUEyKQvYLWfkF8MJKo0jPhl8OCbmXR3QP_H7xoQvdMMhYoK3ilZ8Nu32AuSa3Ye8OC0-BY8aMCONetGFEOO1hnMUUZN2ADQVlci-rSq_GwfAY1u99tMgr0Tru5RJ8kGnGrvRVoQDHt5HUpuT1atkLkBW_WMLWiOAMgvVpG5ltpbmDcyOjh4KliVDd00gdq6vsnIFYGEFZRCLon81ORyGJXrNm0jRo_GouDSczAQpFWG1qDGjYlyjFA3c-S8OYXMMtgGdMJ1WKPWfA62pa9kes3WMGYb_Wz8uJntv_rSRt8FAj2H9uL8-c0CxlATes9IMzsEc_5oiky88J3lxSyEpYohc01bSzr57l211KCsUT7uuV6Jfi8UoO4gO5e5xr6L-W1-w9K3XlKaZdMbJECNqMmCONr84MhpE9YFNrY9FPaCz-eDVLo83ng568NKIK8qMEWFNtlglVc8yjqqsjj-Fc02i2RlWzOeuI23o1Z7pbbkMrDtwQ_XBqh7n7mVQVGCH_8TV9G8koQYmQfbgQhHo9IXKjE5MVaTHaL3ZqDOLTZEer5JUthV6fLefb1BMxRHztA0qWyCFjcSezAEutjiZW0pItiSqznuRzXdQLzYpqflerE_xrmFvv3kFS8CpJjU64wZ-bzmn2fEvQhTMh672SzQhL35mFbg-HuCV7-Qx3KqNnVtgyzHDOqWwMrrjHJjtVA1XYT65jdJnjoPGymzOflPqZ-YHeSD9pg6XfhcVfB14-r8o-JIIG8xBdsVeV1Qbp5oqBZu5CcbLFzYyuJ2qhat3HE63ckOCbiamc1t7CMkwiskw5uJosuHcBgcb0dTausVB4qoF21iu_yw-8_n1YJoLkM3gmVDKx_rPnWR-jM_IW_lCiRVQK-T4nSubDVEc2-FgP0z9D4imGitzYAJrvnyn1mmmwtnYtkkxmKZhm-vgkAqJNLJCCd-El1LgzLnpPES8ShXwOuwzY6v2MXW0TPUwWkLhVopJLc7kVQhH5VLj_KttcN5CpIFX9zR5QowKPupfP7hvQq8GkPs3VQkkQHhgBSaHxSiAmZJdoYXWlmLNz5Tuew2TE8HxKVwPfjWBOllymEwjC70Ef3P1PO2yu1C_rXrw_G077to9bMBz-xo2RIbg4QLSfry3Fa0HzFLfAuQ5c481DrHkQf0ThYTmF3sm87ZkGa7xX4fQ7YR9TlrcJt61S3TfnTL2fpbea1A6ZxjxuFuUf8u4WJp4bklDRji_GLxX4KYHawSmoaTg5hEf_TbqKc0BmPqxiohwI6NnktqiviyqnkKlcsb22mIeu-gL3punS6NPB7Dz2JaRjvtSuIG7TWhetlWCaxILAQJcx7Vrkxi0xMxO1pCMKpA8_jC-XyjExCMD4PCTFpsECVQhLlSH-AlrMjXR2tPDkxa6q2bE0RU96Rahvx9NH2D3zfXjZJI-JZlGYU57ZVOFlGbaM1wvivIsdNpnu1mOoKQbU3moJgwFf0Ea0YeucninxLOaWuWdPSPrvhmltig7cdl4830ugu4kGDFF4rQTNUfSjG4kkHeJWczLd2tpVCW8SSl1mwV2PSBspQ2QPOdj-LMaKxrX8K1VT74TIAQn7HXfDl4OTnInCgZgQnA_OlJLVpN3Ce_oVL1dNPGdEV8ZGfl_egVYf6--jfdN-dLxDKrk2qF8t4gV7AhxF2ySj5pr4MfJaKv7FIRTNO2Klc7NS6fZelX5rHYNUyQ-2qqK94Yas2nMbewJemK218FlYYnevaOAvwId5CuwDzWPxsVAyNzCJS6l2Be9YrAaSJRrZ5qL9ZRulTGISy24IiIAWIsUBxxsdJVsd3az_rEnqoKlOMytMGQc7th59D755ko0URJpdtyDcz0Yg8-u-IhCU7qtmFTo1avJFsQMlHApbDZh28mv1pqHvomQL22fUHBa6ElSXU5t-rK2GCg3IKcEURnciRStkfAYjKIDUNGulFfagpcQqE9fNu2KyfdS33hhwWeMvIbZaNQiy-c6TTAsBdET_mlyq58AhmHaIV0g6PcEYiQpliGv3F1KNMj8QZrA5vktYVaCONkQNvAcRzqjpPx-e_IXpCr2fhjkg4tLLR418hy2uiNhePmuB5fp_TnmU7zImQYAcW385XFgQuVPv6kVOLorENxZP_vkIvXuhr4H9oFtEuNsQPai4ynkyplpeHaJnts9bd8DdqBb1FPmxdaDyxRddlmp7Z1ATkH3m95so5R0sukV1tHDsbl8ZgmWWstpNyWYYDbmvcngOZA1qCbAIPmZ5_ksdsEWo9bkGHw2Di2LwHr4S3hUfL1LxmdNL4o9kPXT0mw09a5HcgqihiqVLhw6l9mnY74heaqDHNVVT81zSF-Jn6DkZV2IBeAGMpjuHmVCFCRmwWIWNsBHvUUYzn0ygxZLjwpUSVIj_s04xwW1VpvF63C1YPyb0uB-Bsik6e_vZwKzvgbUbSx6ujvOypHYo5WyirU9WVOdrb2v685fCeRU6k7CG75iK10K03Be_5pz8A1pJDUFvqR2dTUXdEi19wM-H26PxBUrG7v8uPC7C2jFo31AXlzfMBbo-nvDTdWqzmDl19_wFtXsLWMb61i5Eo-q3461HkqZfKqe2fgYb4pnReM-0g1GYXrcPKmwO68Vb2srbw_GIKkci1Rsr5Y6zRDPUKdxshJ0MKNYmniwoNQj0QZ7Js9_aKiMizrzUgks9hWZ7RfK6b7xT9a-ZRG_Q4KRpn_9pyXFCmqdFzX7BysVN6LNcvl9CStWwuwJoR6SJlisnMIra2ygOPh-5MiPILQQb907bsH4XFAkRcqOYyBo7b3LhWdaTqdqW8gj7gh-YxrQtr3wJf97ohsDCmHSKpXhw8Ct-pYLVVS2dWWH6UptFe3_aUX6Icv07O-It_90iTf-TfnScZpdBLzLY652tUji-mpSBFJ8vfPzp6oXEURiYRDSvmm8pm4Mh7afMBcpJJsX93WtOkOz8CU7qMezSnsscrn4rko_ZXpgzgCKFDFvfvjNA-0DBRC7mrD1kF9cjPpJYI56pcbyQxzo3aNF7bwfiDxxBAlwVv_05A8LcyTnHKCpPP9kbJ7U1y56RnyZwsq16JEjF-DBdv1xZvM25GNNDgX4ZTHeIbqT5JTJzjL_QtUPRlnZroSTrQV2WVxElF0pFsc5gDaUdG7NaMVhGVURbf0GozlEYLsE-qiipHBeMi2Q54KcDkKppmlSS_lG0h_jN9NU9_ZaIg4s01a3WBbHtVwCxFk8kOQKGtm4ruDs15QHvdNvpXjY1W95ye6G7MPUNuM1qJZqnBk532hVHgEBNhnW3G1qQp3DqYJ8ayOI9ABKccr0b5fhy22fi_kcLrgXcbxlIX1aIEkiid1U_8Tkr3ecJmnxl1NHtJF5NvKWZOpy6yhfbpfLzaRxDvooHcwvyu5mp6mrDr2R8vp0dyNmj8MUnx7nACiXq7FvIygJcMvuNUtk9tCi_56QTiqBSexBTROiuUyZe4khsMq1z9INCPdSkfJqOs0nkoHbEyKAEZBYVO2AN7tO7ai80UjteegUbGMR4F61JIx81xUXYQt0hcGqRcP2Puzx6jI-LIZWIT4R9ORYZnypepySMW7BTKqC2f3MkY0QCgaMasHVZTsdqERdH1QTF_3Vzg9eJ5K1TTuOCC7JX4oNs40DjT5aCZohDwJM5M9WRcb4ePyj9j-WJC61xYbzj-O_JBNduzxnVO3QfDNPEXpa-P8xx5dr4GiCr1Rry_X_nND0uo4doovfWDmZskcxhVNzseJlqUIAKQ3gaovz9C-yMHeZSFDH1MSxC2bvvnThfuul5mSFvg2r-98kSIn_bAnWvKoeGPcNVfBAAy2LxcB5BB-YQAKCj1Wr8BV508Dc8NJPqCYwzPOaNlZmZYgyfoUh4mwkGjN_jKAcCezizxLCow3FHc7tyZqMKFz89Pb1gfBgvPmudL7m9iMSxk7yY0C2lktKm9ji9uDC_e3W91KOgIkEWm56Fv9hpnS2dvmlfbf_ueejtD3kuD1pj8Wj8985XPfJHbl7lsOj7NYYeHRYtZlpNpMa8VS7_ZT56-nYCjp32oLPAH_B_sI38yUFwnDK2qI2WhazMcgME2aTq2iYJGthzKAlcHGUTYk2LFs31y-qI_k-k9ZCXwJDkMtH6yQWEj4699qc3eIOB5WtrP5ggBvEo3YFflWiNzYyzciKKrN6c1vHMruKFq8fmX0eIepzp1D89jKt3TtsJS_PNA5a9HLYYvTbD8QlFsG6-Q-IEc4WEHMWTomvkjbt9_UuL1GFYYmGue1HHoy_Q1iz2Co1jjIP8DmCOrtuOqLpFB6W5tPKcRR0kibTISm5-VbO91uLl2T43WePK-ri9DAnxrrtwLrY2OflBHPU-ZeNA1rQQTwaAvc8hNo8IRQwgSlSCjZdcVgNP8Lb9_DYiiG4YVJf5bkC_ENxOAlFiEk9IJGpV0hjdyxEumm1m5iTXF-w5j75BE03Dmnp54YEfrxy_qRhvzYig-8qnbNggrPPVRR3HwXfJYb8NFlpd-M3L72enhFrI9ZaX8iaUX70UFsk0Ga7VAiNqmKzLV0TomuSSxgBrgLsU0WengI0exXvseT8iZIzzLc3SPkvjgy69RPesDLuVCqsn-3XXH_i3dxwCg1ZdaqV90T8cKIQxqi1GHGZ1KjTlHTRAhSojuO7oWPr5hiZyjhTqzCDgHRc5eSGTZ5LDKvk9o5Mu3Bcbwg-1fP8_ohVvVyOGBSpyCT820D6Y0g0lTHYKWAvXwQkYeAPquo-waDl46NS18S_93gtk7yoNtSUJ7IzLqyydhIc_QIoDr-G0nWaaSXaZbcXLay9CIv5SReMgudE5MSSQaezImBRWUJ9nGk2lMsb0H50MFijdCceGTcKHvggqyn3frlWGs2SMcC2bOeyDU5B7UKaD20seXuD1_Ed-W_t3Xu3dLPDz_uqGDvL0icJxjFJ7tJsHfTB6NUJ_F26X2Ph32unUJslHfxQkdSfvecXwX-HlMsVVPedGmOmd1L8uEHoePi9E7xOt5YfUaenSGSgrH8TenqFbyEqsGRv8lvV9NZKeBISqVp9Z3EiZX4jDSzjKDTk0CcPiRrd-alZL4MaNj0sid3wVqmAW8RnTKn6MWS8MhCUPgv4YyPX8iIY_XDbAYx7jpQXp6q0woDiYDVR4gN24JWq5zOl_G5s0-npT9j-f8iNL5iPnExBlBerj35lpILpqNb8o20jtxLdDVVdRxHQwTNuJWSkSU7V3WKiYr8b5i-rfhAsAfreOT31WPe9V5ArbcnxAt4kgA_oUS0gRic1qwUCF47hfcGirW-7kuCaM2NgQTrV_tGrP529Ar_rnZxU2pojvDJBmU5Mq_bGm_GEpzMujf5BuUrGSV2rdbjQRXqaPw0Hku7XIJkBcem7BjyqDvvJ2O4Vh3hXxEiDrvSocfZTt3aqDdmb_nHecNr1YkKb6u1bYIfbLBCIRBY_xTlJf9knBUVZFX4BJ38DGp6p0Xn15Oafvd7170xv673FasppPDA8eXNN8kf9iI77cNavCrLkvgxDpUn_AcoQ_t3XxUUGT18mUoKMrTvyG_lyiPnkZfHMW6eEh-cifwoFYwMzr-FKspGfSykrlj1eMWTsaCdU0FZpt5E2RXLfKZ-8-JMAWIXX8uwONxnzZpo8F5ay0i7RKc9wl1BXTEX8Gd4fUS6uuXJG3m_VPQiNsgFp1vu0DKTyz_SrMD-SFKlsoF0C53IODpXuqWmvO-NlQ6OHeN5Y0cn_EA.neFueASh0HRgrYAfZBZd5w";
                //string jsonString = "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZHQ00iLCJraWQiOiJzdGctbXlpbmZvIn0.gJMo6fgr-EedXaofdzz8N64ejBDI8nY8CeHhhvJoWcqxR4AZn-e22HCys-M4HSGvh45z5Z62nlpCsNCLZNu9y371RNUs4Npvsj-1-OYCjo4hkd2VHDiQmqjWjnTHpqHTGzcv7p_4war8f59ET80BotS68W7DbXY9dJRs-rpw0tjC6g5N_6Hp4JTX5dHdwGvWAyaXrt2CMBeHTlz9ziuWd6nsobucKDum_Rc_yWkPapbwFNnNeV9PWdCs7RCxRmK8gMebyrl3beHCd7_d-ZcqkvoWysXxBANLemLufLl_49G6FyE9UOZ2DdBvdo0T88m8fb5lcWcBcccsAQNBxr2fag.0ZBSxGYkuj9fHI5Y.-Qi4QWbh-rakr5HdUIotJMWYhOfe-CviKOHPncFlkA_PGkD-eBQU-wAXlCGcJXhBQkpBUZXFKGs8EW5AWdV7LNP7yLVER-NfPG8ULX86zJk1hELPD8pp9EUnhTJaXfvMtGx6zsKF-4UDuVBKPYIXmQ_oomTH0wZczatczFlosfQvT_PJkwspnp6LLifJzaeQICmIpo_Yuz8mKTJqlBfWySty9b870pRlZZfZ1OpEq5Di-W0d8iMI2_Ec1E6afOfcJ49pIoAVuK36ZGxR79vhpUyrdvE5CSPQkKldDOeOHkjz3FQw4-knwFCD4tnAkFDQxveC67NdY2ks0n0JBFbKSHUSnTYWgKRN9cBDCVtGs9HMEafmJik71EDUiSOv46rrJpwyIoQ01_g1cQerXoW3Zz614qa0viH5OfoaVjqXpPRPEm71jQvYRuDi1VKEN1J_-F6kl-H4okw0qeunuFg8UOutiIixjIwsKeYCndobGeaH1fhwI9-_ZmUaVGJ8LHeP47u5fIg-S1crQ27QmELF00xfeIyNpwkOBWJ0JDlLB59Oc03A5zcdDqNuT1wG9uUh3MJUUsqZv2nWJfE5Ay0vD5WFdatqRc4T0tHAl8fBfECz6QTSOjn7GCq4k_bgIBQ8eseXXgq_J1N7GF2nTOEVegnjiKNjm6i-K2XXIYhomaqfdoFFf07eov_vbLSLYYRQnBtBuYalUSeOSdZU70t37cD9dd6k_L6-STXtnoNPfnaaP5jeHBsm6Gb07ep-G8965mnbe5OY-o6UQwhIEH5CFTsXtYh-G63QbTwrgJkLu9OvShvRGso-owhbH0xfNz1pE8GS98KJ6-EghDq40wE1xazfFtDVnzecq6V4kjZyayZ9zSEXwCCAjVrtoMsvsGMcFHdXAYZI2jamYVNNI3Pa9h2G1a1nxNFn1nVMfihG99JYVfCYFFS2kmiCVXvNnv2ScaoMLBCGhqhAF4rwauJ8_pWUrOB4ab4DndJ9HLJR7pe5tltkEIblUGkVId4d1ueNG4CwpYfCsPBQ67SN_U1bToe9_uoJX9a-2dcYsPfNtM4-xumX_ge7VeFTTZyvuWHXO2OrAvyftuwdXP8qGrmhXPvoz9X2erz3UgQjkNZi0cQqDO_PCNDtmUanm2ohmDONU3--rf2NUh87nlyfVntEh2EHDVHvHIgKklzVHht_HHOP_g_sjy_FQa77UbL9O8HdtaVWqrIL8jmUqrvNIWNkyZuU6Cpf7Tuek5m3bLjeesq7NBXYxkqU6-wF0fxco8TM8GmACciA2lx7RYp8OevoEW5fJUahhb9GxNgDiFjSpki--s_7fUYx9scNoro2r1MCgnR0vVvs55WN-w9HyO1louMAcx6IYZO96D-4sCI3WlqvADdVnp72SncgVe6KHIifpQLte2F7yKzylifK4NFyxgqEkoAR64-l0qg5Kr8qGurOwZmFGajP5ymRlv_Cfj7ugOqMId1EeG7JKMT-0ljIcaHARjBc2Wff5K32ZsrBhpAjdJpMPKCNfTXINyzrZ7HbleKFsFyEG6mWi5l2hMK4N4C-aHXAwt7IkoXgBKhFA-6DmPc_oeWGleDqBKoddhr08QIOwcefeVMP1eu7h5Yof002d6tYnPxZqh_8H3RTXBaGyeXQvqH43v4_MlL-lrOv3pDy2hFA-nCY7DyV0n5c1LqlKJupbHjQHRTI_wO2J23tvWn0UqNLgKZ0J8_CQEOI3C2gaC42pq44Juo5kQrmPYqtmIEIsWs4D6ZF4WH9Z6u6gaSBfI3YpdYNr_69bLbhIQ0HS04tYjic1XlYdJc8OubO2GQRsRiC4_wXU7BWH3AexGRuGHeZO_6LLd3x8MDzqvrtD024m26n8PtZ-VVLp-CkS9_weNKUyyjTQv7pYivaQQyLQduhsFT92wosm1Xb6ELDq0S2j9p8LsOOVcAq7G8N_k3m0xpDswGbjgeUWNy4VT-clXo4HUgVfVtaJuSI249K74jMEzSxV6WkECVMEaGfT_H9-GqyXbyfpKPNg2dq45ivwcid10ebmsIgjBficwvS-mnpUq10oa7i-Zq9NdPu8t-AEKcb8Qj3qABGmUHrui-3lEPbZsHPHLj3Rf2tatuaqQ_Hdci65UQQdOGk14g3oymR0JuT6CIJnwWsLxwzPn8gkylkU2V9bo5m7jiBHQt_swa2Xan8UM1wsw1yk5iHlkHG-KOprtn3qcol88DIeV-viDAGuDGhGncZMajXcMYLmrkILkUhjuPDBk_blgXTkLTzCTiLaTEQrs1PijohGOgNYBne7DRS0d8dwoZ78jQz0FiTphVLWS9lCgxQUQ7HwLUr1YmG6IL31TSckEPmLz44F39ZF9ZtYlFsIwSzMsZQ-zzK0T4le9Jw64CAOwcVw-nItGm6g7OPbhBT4UVXip1YgWlyQ7arUURaePCWIaMT4aovAR9ql5_szUCvJt05rKGb3JSvWschrPUq5SpwtdknXK3AKHyC2iMTHvdnqgTRk9sobI88F5E-9sOEJojRrB3INvuLroqoZfuJkkNiiB_KDYAnJA0H8ycO8tS7q_dFdjToBkNQIagF203VMwxNCk2DO_BUO9rULWtbzhzVygxACefxw-A6FoDc85cGScmJ12bMruBrhobtW-Z8l2T5OKatEAE5tG8dhul7iGSwstZBEwXwMjiDYBx_A8OihqDc3DOUFhw6n1w8KUn4MsoQShOHm30cXxSkIwh39q6wS9pXeOozlTF05l5c5c2hE85PGxd6yqeyDPkJDMpZmxy8mZNVIWAl65ndteA0Js0o3MMp70F1Csl1jdpq2fidS7GmEaGvs8_PDEKUwIJ8sOwnySbKNZ7TK5DDCoQk46NBaL4uUgI4KLbVPENSmEOUHttt-Y8haCGCxZs1YHzrL56VhqPRG-O8hvRulMrL5SI1PROk_X2N4S511eVF2mTysmGYHnHJzAf4m8SAcRT7hh77up4i0EypyrygGMndNV2ZnB4fROrd9qgcfEzUByxM-IBdcnwA7zLUwJsC5SUgXp2xUgaBW-79X2aNOFwoTqY7IoIwg1WgzysfOkMGuB0240QVXMN3nUQXSFxS4LATFfmn09pb_rTSqmubr7Ay9v4Gbj1CL_TnyzGNHgo6KKzrnehS9RPXh4jYjosYFDc_htmqAApHQE-eUXJnic8amA7B_EXKZRz-D9eXUbPywws90By33B0uvR-2GZzwGvbcDD9EauaUGUsINDLYQihyI4RV8kp1EFS1Tw1Se5i5LUDWfDoBZeioJUQQlQrAWyqta5TCKHhV0bHuupc_cHM8L4PzY4sWfPdRZaNj4PJLQsn_6ZpPUQ0RbmA5LAm0ZoYNZQ9mKy5voTZJVOA_AXstAvZT_tQvUM07qikEjJiIYSPKW1GwVv9o2mIf7CTcu8ES76_0wQvfutTN2m229OcLIaUAdLJRenF8U8Iadd9tHBg1ZWjwyYq25JK8i2HR2TAEvhVcVw1-nMjfJII8oaoc9dbzEGItWxnPnmHTY7sFkOUsBa3C6XAgEX9jXDBK1QicsQd5c1pNutJbejN8kIMvzaU0vcNlr4OHzJuUle0LBBOOb3D-M0xe0QyOZejAiCorffFe.zVjgh7wxtGIuSp8Kj9nP1w";

                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"update MyInfoRequest set Person= @JsonString " +
                                   " where state= @state";

                    var result = db.Execute(sqlQuery, new
                    {
                        jsonString,
                        state
                    });
                }

                logger.Info(Util.ClientIP + "|" + "will parse now | " + jsonString);

                
                if (Util.isLive)
                    return ParsePerson(path, state, jsonString, null);
                else
                {
                   
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);

                    logger.Info(Util.ClientIP + "|" + "uinfin | " + myDeserializedClass.uinfin.value
                            + "name| " + myDeserializedClass.name.value);

                    return ParsePersonWithJSON(path, state, myDeserializedClass, null);
                                    
                }
            }
            catch (Exception ex)
            {
                List<DropDownModel> fieldModels = new List<DropDownModel>();
                fieldModels.Add(new DropDownModel() { Text = "Error", Value = "Error occured while parsing MyInfo callback"});

                logger.Info(Util.ClientIP + "|" + "GetPerson| Error " + ex.StackTrace);

                return fieldModels;
            }
            
        }

        public List<DropDownModel> InsertPerson(string path,string state)
        {
            List<DropDownModel> fieldModels = new List<DropDownModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"select BusinessProfileId,Person,RequestId from MyInfoRequest where state =@state";
                    PersonDB person = db.QuerySingle<PersonDB>(sqlQuery, new { state });
                    logger.Info(Util.ClientIP + "|" + person.Person);
                    BusinessOfficerModel officerModel = new BusinessOfficerModel();
    
                    if (Util.isLive)
                        ParsePerson(path, state, person.Person, officerModel);
                    else
                    {
                        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(person.Person);
                        List<DropDownModel> persondDetail = ParsePersonWithJSON(path, state, myDeserializedClass, officerModel);
                    }
                    officerModel.BusinessProfileId = person.BusinessProfileId;
                    officerModel.MyInfoRequestId = person.RequestId;
                    new BusinessOfficerMaster(Util).PostBusinessOfficer(officerModel);
                    fieldModels.Add(new DropDownModel() { Text = "Success", Value = "Detail for " + officerModel.Name + " is captured" });

                    logger.Info(Util.ClientIP + "|" + "New Officer added from MyInfor");
                    logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(officerModel));

                    return fieldModels;
                }

            }
            catch (Exception ex)
            {
                fieldModels.Add(new DropDownModel() { Text = "Error", Value = "Some error occured" });

                logger.Info(Util.ClientIP + "|" + "GetPerson| Error " + ex.StackTrace);

                return fieldModels;
            }
        }

        
        public BusinessOfficerModel GetBusinessOfficerWithMyInfo (string state)
        {
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select BusinessProfileId,Person,RequestId from MyInfoRequest where state =@state";
                PersonDB person = db.QuerySingle<PersonDB>(sqlQuery, new { state });

                return new BusinessOfficerMaster(Util).GetBusinessOfficerWithMyInfo(person.BusinessProfileId, person.RequestId);
            }
            return null;
        }


        private List<DropDownModel> ParsePersonWithJSON(string path, string state, Root myDeserializedClass, BusinessOfficerModel officerModel)
        {
            List<DropDownModel> fieldModels = new List<DropDownModel>();
            
            fieldModels.Add(GetPersonDetail("NRIC/FIN", myDeserializedClass.uinfin.value));
            if (officerModel != null) officerModel.NricNo = myDeserializedClass.uinfin.value;

            fieldModels.Add(GetPersonDetail("Name", myDeserializedClass.name.value));
            if (officerModel != null) officerModel.Name = myDeserializedClass.name.value;

            fieldModels.Add(GetPersonDetail("Alias Name", myDeserializedClass.aliasname.value));
            if (officerModel != null) officerModel.AliasName = myDeserializedClass.aliasname.value;

            fieldModels.Add(GetPersonDetail("Sex", myDeserializedClass.sex.desc));
            if (officerModel != null) officerModel.Sex= myDeserializedClass.sex.desc;

            fieldModels.Add(GetPersonDetail("Race", myDeserializedClass.race.desc));
            if (officerModel != null) officerModel.Race = myDeserializedClass.race.desc;

            fieldModels.Add(GetPersonDetail("Nationality", myDeserializedClass.nationality.desc));
            if (officerModel != null) officerModel.Nationality = myDeserializedClass.nationality.desc;

            fieldModels.Add(GetPersonDetail("Date of Birth", myDeserializedClass.dob.value));
            if (officerModel != null) officerModel.BirthDate = myDeserializedClass.dob.value;

            fieldModels.Add(GetPersonDetail("Place of Country", myDeserializedClass.birthcountry.desc));
            if (officerModel != null) officerModel.BirthCountry = myDeserializedClass.birthcountry.desc;

            fieldModels.Add(GetPersonDetail("Residential Status", myDeserializedClass.residentialstatus.desc));
            if (officerModel != null) officerModel.ResidentialStatus = myDeserializedClass.residentialstatus.desc;

            fieldModels.Add(GetPersonDetail("Pass Type", myDeserializedClass.passtype.desc));
            if (officerModel != null) officerModel.PassType= myDeserializedClass.passtype.desc;


            string personValueString = (myDeserializedClass.regadd.block.value.Length > 0 ? GetPersonDetailString("Block", myDeserializedClass.regadd.block.value) : "") +
                            (myDeserializedClass.regadd.building.value.Length > 0 ? GetPersonDetailString("Building", myDeserializedClass.regadd.building.value) : "") +
                            (myDeserializedClass.regadd.floor.value.Length > 0 ? GetPersonDetailString("Floor", myDeserializedClass.regadd.floor.value) : "") +
                            (myDeserializedClass.regadd.unit.value.Length > 0 ? GetPersonDetailString("Unit", myDeserializedClass.regadd.unit.value) : "") +

                            (myDeserializedClass.regadd.street.value.Length > 0 ? GetPersonDetailString("Street", myDeserializedClass.regadd.street.value) : "") +
                            (myDeserializedClass.regadd.postal.value.Length > 0 ? GetPersonDetailString("Postal", myDeserializedClass.regadd.postal.value) : "") +
                            (myDeserializedClass.regadd.country.desc.Length > 0 ? GetPersonDetailString("Country", myDeserializedClass.regadd.country.desc) : "");


            if (officerModel != null) officerModel.Address = personValueString;
            fieldModels.Add(new DropDownModel()
            {
                Text = "Address",
                Value = personValueString
            });

            fieldModels.Add(GetPersonDetail("Email", myDeserializedClass.email.value));
            if (officerModel != null) officerModel.Email = myDeserializedClass.email.value;

            personValueString = (myDeserializedClass.mobileno.prefix.value.Length > 0 ? GetPersonDetailString("", myDeserializedClass.mobileno.prefix.value) : "") +
                    (myDeserializedClass.mobileno.areacode.value.Length > 0 ? GetPersonDetailString("", myDeserializedClass.mobileno.areacode.value) : "") +
                    (myDeserializedClass.mobileno.nbr.value.Length > 0 ? GetPersonDetailString("", myDeserializedClass.mobileno.nbr.value) : "");

            if (officerModel != null) officerModel.Mobile = personValueString;
            fieldModels.Add(GetPersonDetail("Mobile", personValueString));

            return fieldModels;

        }



        public List<DropDownModel> ParsePerson(string path, string state, string personString, BusinessOfficerModel officerModel)
        {
            List<DropDownModel> fieldModels = new List<DropDownModel>();
            //fieldModels.Add(new DropDownModel() { Text = "####", Value = state });


            //X509Certificate2 certificate2 = new X509Certificate2( (path + @"\ssl\" + myInfoCertificate);
            X509Certificate2 certificate2 = new X509Certificate2(path + @"\ssl\" + myInfoCertificatePFX, "test");
            X509SecurityKey securityKey = new X509SecurityKey  (certificate2, "AIDA @ ACHIBIZ");

            logger.Info(Util.ClientIP + "|" + "Path| " + path + @"\ssl\" + myInfoCertificatePFX);

            //var privateKey = new X509Certificate2(path + @"\ssl\"+myInfoCertificatePFX, "test").GetRSAPrivateKey();

            RSA rsa = RSA.Create();

            rsa.FromXmlString(myPrivateKeyXML);
           
            var payload = Jose.JWT.Decode (personString, rsa, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
            payload = payload.Replace("\"", "");
            /*
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(payload, new TokenValidationParameters()
            {
                IssuerSigningKey = securityKey,
                ValidAudience = myInfoClientId,
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                //ValidIssuer = "https://test.api.myinfo.gov.sg/serviceauth/myinfo-com",
            }, out SecurityToken personToken);
            */
  
            var person = new JwtSecurityTokenHandler().ReadJwtToken(payload);
            var personpayload = person.Payload;


            MyInfoPersonBase personbase;
            MyInfoPersonWithDesc personDesc;
            MyInfoPersonAddress personAddress;
            MyInfoPersonPhone personPhone;
            object personvalue;
            string personValueString;
            if (personpayload.TryGetValue("uinfin", out personvalue))
            {
                personbase = JsonConvert.DeserializeObject<MyInfoPersonBase>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("NRIC/FIN", personbase.value));
                if (officerModel != null) officerModel.NricNo = personbase.value;
            }

            if (personpayload.TryGetValue("name", out personvalue))
            {
                personbase = JsonConvert.DeserializeObject<MyInfoPersonBase>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Name", personbase.value));
                if (officerModel != null) officerModel.Name = personbase.value;
            }
            if (personpayload.TryGetValue("aliasname", out personvalue))
            {
                personbase = JsonConvert.DeserializeObject<MyInfoPersonBase>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Alias Name", personbase.value));
                if (officerModel != null) officerModel.AliasName = personbase.value;
            }
            if (personpayload.TryGetValue("sex", out personvalue))
            {
                personDesc = JsonConvert.DeserializeObject<MyInfoPersonWithDesc>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Sex", personDesc.desc));
                if (officerModel != null) officerModel.Sex = personDesc.desc;
            }
            if (personpayload.TryGetValue("race", out personvalue))
            {
                personDesc = JsonConvert.DeserializeObject<MyInfoPersonWithDesc>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Race", personDesc.desc));
                if (officerModel != null) officerModel.Race = personDesc.desc;
            }
            if (personpayload.TryGetValue("nationality", out personvalue))
            {
                personDesc = JsonConvert.DeserializeObject<MyInfoPersonWithDesc>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Nationality", personDesc.desc));
                if (officerModel != null) officerModel.Nationality= personDesc.desc;
            }

            if (personpayload.TryGetValue("dob", out personvalue))
            {
                personbase = JsonConvert.DeserializeObject<MyInfoPersonBase>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Date of Birth", personbase.value));
                if (officerModel != null) officerModel.BirthDate = personbase.value;
            }
            if (personpayload.TryGetValue("birthcountry", out personvalue))
            {
                personDesc = JsonConvert.DeserializeObject<MyInfoPersonWithDesc>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Place of Country", personDesc.desc));
                if (officerModel != null) officerModel.BirthCountry = personDesc.desc;
            }
            if (personpayload.TryGetValue("residentialstatus", out personvalue))
            {
                personDesc = JsonConvert.DeserializeObject<MyInfoPersonWithDesc>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Residential Status", personDesc.desc));
                if (officerModel != null) officerModel.ResidentialStatus = personDesc.desc;
            }
            if (personpayload.TryGetValue("passtype", out personvalue))
            {
                personbase = JsonConvert.DeserializeObject<MyInfoPersonWithDesc>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Pass Type", personbase.value));
                if (officerModel != null) officerModel.PassType = personbase.value;
            }
            if (personpayload.TryGetValue("regadd", out personvalue))
            {
                personAddress = JsonConvert.DeserializeObject<MyInfoPersonAddress>(personvalue.ToString());
                personValueString = (personAddress.block.value.Length > 0 ? GetPersonDetailString("Block", personAddress.block.value) : "") +
                            (personAddress.building.value.Length > 0 ? GetPersonDetailString("Building", personAddress.building.value) : "") +
                            (personAddress.floor.value.Length > 0 ? GetPersonDetailString("Floor", personAddress.floor.value) : "") +
                            (personAddress.unit.value.Length > 0 ? GetPersonDetailString("Unit", personAddress.unit.value) : "") +
                            
                            (personAddress.street.value.Length > 0 ? GetPersonDetailString("Street", personAddress.street.value) : "") +
                            (personAddress.postal.value.Length > 0 ? GetPersonDetailString("Postal", personAddress.postal.value) : "") +
                            (personAddress.country.desc.Length > 0 ? GetPersonDetailString("Country", personAddress.country.desc) : "");


                if (officerModel != null) officerModel.Address= personValueString;
                fieldModels.Add(new DropDownModel()
                {
                    Text = "Address",
                    Value = personValueString
                });

                /*
                fieldModels.Add(GetPersonDetail("Street", personAddress.street.value));
                fieldModels.Add(GetPersonDetail("Postal", personAddress.postal.value));
                fieldModels.Add(GetPersonDetail("Country", personAddress.country.desc));
                */
            }
            if (personpayload.TryGetValue("email", out personvalue))
            {
                personbase = JsonConvert.DeserializeObject<MyInfoPersonBase>(personvalue.ToString());
                fieldModels.Add(GetPersonDetail("Email", personbase.value));
                if (officerModel != null) officerModel.Email = personbase.value;
            }
            /*
            if (personpayload.TryGetValue("homeno", out personvalue))
            {
                personPhone = JsonConvert.DeserializeObject<MyInfoPersonPhone>(personvalue.ToString());
                personValueString = (personPhone.prefix.value.Length > 0 ? GetPersonDetailString("", personPhone.prefix.value) : "") +
                    (personPhone.areacode.value.Length > 0 ? GetPersonDetailString("", personPhone.areacode.value) : "") +
                    (personPhone.nbr.value.Length > 0 ? GetPersonDetailString("", personPhone.nbr.value) : "");
                if (officerModel != null) officerModel.Mobile = personValueString;
                fieldModels.Add(GetPersonDetail("Phone", personValueString));
            }
            */
            if (personpayload.TryGetValue("mobileno", out personvalue))
            {
                personPhone = JsonConvert.DeserializeObject<MyInfoPersonPhone>(personvalue.ToString());
                personValueString = (personPhone.prefix.value.Length > 0 ? GetPersonDetailString("", personPhone.prefix.value) : "") +
                    (personPhone.areacode.value.Length > 0 ? GetPersonDetailString("", personPhone.areacode.value) : "") +
                    (personPhone.nbr.value.Length > 0 ? GetPersonDetailString("", personPhone.nbr.value) : "");
                    

                if (officerModel != null) officerModel.Mobile = personValueString;
                fieldModels.Add(GetPersonDetail("Mobile", personValueString));
            }
            return fieldModels;

        }

        
        private string GetPersonDetailString(string text, string value)
        {
            return text + " : " + value + ", ";
        }
        private DropDownModel GetPersonDetail(string text, string value)
        {
            return new DropDownModel()
            {
                Text = text,
                Value = value
            };
        }



        private string CalculateNonce()
        {
            ////Allocate a buffer
            //var ByteArray = new byte[20];
            ////Generate a cryptographically random set of bytes
            //using (var Rnd = RandomNumberGenerator.Create())
            //{
            //    Rnd.GetBytes(ByteArray);
            //}
            ////Base64 encode and then return
            //return Convert.ToBase64String(ByteArray);

            Random rnd = new Random();
            int myRandomNo = rnd.Next(1000000000, 2147483644);
            return "432" + myRandomNo + "12";
        }
    }

    public class RequestParams
    {
        
    }

    public class ApexHeader
    {
        [JsonProperty("Content-Type")]
        public string Content { get; set; }
        [JsonProperty("Cache-Control")]
        public string Cache { get; set; }
        [JsonProperty("Authorization")]
        public string Authorization { get; set; }
    }
    public class ApexParams
    {
        [JsonProperty]
        public string client_id { get; set; }
        [JsonProperty]
        public string client_secret { get; set; }
        [JsonProperty]
        public string code { get; set; }
        [JsonProperty]
        public string grant_type { get; set; }
        [JsonProperty]
        public string redirect_uri { get; set; }
    }
    public class AccessToken
    {
        [JsonProperty]
        public string access_token { get; set; }
        [JsonProperty]
        public string token_type { get; set; }
        [JsonProperty]
        public string expires_in { get; set; }
        [JsonProperty]
        public string scope { get; set; }
        
    }
    public class DefaultApexHeaders
    {
        public string app_id { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }
        public string grant_type { get; set; }
        public string nonce { get; set; }
        public string redirect_uri { get; set; }
        //        public string state { get; set; }
        public string signature_method { get; set; }
        public string timestamp { get; set; }
    }
    public class MyInfoPersonValue
    {
        public string value { get; set; }
    }
    public class MyInfoPersonBase
    {
        public string value { get; set; }
        public string classification { get; set; }
        public string source { get; set; }
        public string lastupdated { get; set; }
    }
    //uinfin,passportnumber,passportexpirydate,regadd,mobileno,mailadd,homeno,email
    public class MyInfoPersonWithDesc: MyInfoPersonBase
    {
        public string code { get; set; }
        public string desc { get; set; }
    }
    public class MyInfoPersonAddress: MyInfoPersonBase
    {
        public string type { get; set; }
        public MyInfoPersonValue block { get; set; }
        public MyInfoPersonValue building { get; set; }
        public MyInfoPersonValue floor { get; set; }
        public MyInfoPersonValue unit { get; set; }
        public MyInfoPersonValue street { get; set; }
        public MyInfoPersonValue postal { get; set; }
        public MyInfoPersonWithDesc country { get; set; }
    }
    public class MyInfoPersonPhone: MyInfoPersonBase
    {
        public MyInfoPersonValue prefix { get; set; }
        public MyInfoPersonValue areacode { get; set; }
        public MyInfoPersonValue nbr { get; set; }
    }

    public class PersonDB 
    {
        public int BusinessProfileId { get; set; }
        public int RequestId { get; set; }
        public string Person { get; set; }
    }


    /////////////////////////////
    

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Uinfin
{
    public string lastupdated { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string value { get; set; }
}

public class Name
{
    public string lastupdated { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string value { get; set; }
}

public class Aliasname
{
    public string lastupdated { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string value { get; set; }
}

public class Sex
{
    public string lastupdated { get; set; }
    public string code { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string desc { get; set; }
}

public class Race
{
    public string lastupdated { get; set; }
    public string code { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string desc { get; set; }
}

public class Nationality
{
    public string lastupdated { get; set; }
    public string code { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string desc { get; set; }
}

public class Dob
{
    public string lastupdated { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string value { get; set; }
}

public class Birthcountry
{
    public string lastupdated { get; set; }
    public string code { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string desc { get; set; }
}

public class Residentialstatus
{
    public string lastupdated { get; set; }
    public string code { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string desc { get; set; }
}

public class Country
{
    public string code { get; set; }
    public string desc { get; set; }
}

public class Unit
{
    public string value { get; set; }
}

public class Street
{
    public string value { get; set; }
}

public class Block
{
    public string value { get; set; }
}

public class Postal
{
    public string value { get; set; }
}

public class Floor
{
    public string value { get; set; }
}

public class Building
{
    public string value { get; set; }
}

public class Regadd
{
    public Country country { get; set; }
    public Unit unit { get; set; }
    public Street street { get; set; }
    public string lastupdated { get; set; }
    public Block block { get; set; }
    public string source { get; set; }
    public Postal postal { get; set; }
    public string classification { get; set; }
    public Floor floor { get; set; }
    public string type { get; set; }
    public Building building { get; set; }
}

public class Email
{
    public string lastupdated { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string value { get; set; }
}

public class Areacode
{
    public string value { get; set; }
}

public class Prefix
{
    public string value { get; set; }
}

public class Nbr
{
    public string value { get; set; }
}

public class Mobileno
{
    public string lastupdated { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public Areacode areacode { get; set; }
    public Prefix prefix { get; set; }
    public Nbr nbr { get; set; }
}

public class Passtype
{
    public string lastupdated { get; set; }
    public string code { get; set; }
    public string source { get; set; }
    public string classification { get; set; }
    public string desc { get; set; }
}

public class Root
{
    public Uinfin uinfin { get; set; }
    public Name name { get; set; }
    public Aliasname aliasname { get; set; }
    public Sex sex { get; set; }
    public Race race { get; set; }
    public Nationality nationality { get; set; }
    public Dob dob { get; set; }
    public Birthcountry birthcountry { get; set; }
    public Residentialstatus residentialstatus { get; set; }
    public Regadd regadd { get; set; }
    public Email email { get; set; }
    public Mobileno mobileno { get; set; }
    public Passtype passtype { get; set; }
}




}
