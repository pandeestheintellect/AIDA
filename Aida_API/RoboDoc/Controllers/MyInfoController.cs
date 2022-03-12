using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using RoboDocLib.Services;
using System.Configuration;
using RoboDocCore.Models;



namespace RoboDoc.Controllers
{
    public class MyInfoController : APIController
    {

        [HttpGet]
        [Route("api/myinfo")]
        public List<DropDownModel> Test()
        {
            return new MyInfoUtility(Util).ParsePerson(
                ConfigurationManager.AppSettings["RoboDocPath"], "",
                @"eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZHQ00iLCJraWQiOiJBSURBIEAgQUNISUJJWiJ9.u-BucKiFpxXJw-bhp19USWL6RR8Ci8taMO2NwkD6mp9PYPEzOElsKLegH1ghc8jRdLz3hM5ZWBxMGrMD3B6rzwlysP-HybshgEIypZg1dmYtt2AmJzebTswnN9qagScurnB0116bK-hdinV5YeezrRZCUz07EKUOQf21Ml4_kEfbynCp3ZsKyDOx4ZZYMes3mre6zI-eLUMz_3pqLgTsqiMSiXtoPXwd8V2RWQdYXwktXqbMwVNet0m1GTVsUxsTf1UYIJvnRQ3xJrAKkpD_8V55rd1FRDyMbyhwR3Nqa-Es-hjlt0sGi_yGXattS-pAozE1eY515Rw0i-1ZfMu2ng.4TwtFcBwbcnV6NoL.ajeGj6cSyU7jfA8n7NfBT_OSUmsZnu_7p-2dgLOnqsmPBQO2fSjTexmDBGDEOCvADgD4ugCE_XVwT3nsQ_ZFnuqYuqX7lfdrv9XYCF0N_GeBMUayy-2sg5AtTZ84aXQx8N-L1rqLz-vtSv-njMQ66GbJC8ymP1K4Hf9rIRGgZH_JRcLSz_RuccMrxQaV9xWUoHbIYQyS7n1HsA7dxfawhhsvVy2OKQv6R53bMrTWlre3fXricUGwttWZbaGngRanZ0ZEYqOQAoowlvoDcP1bQNNrQk47G38Ou0pdYwHKPZkccz5fTZxmXt7zoc0vfRtFU8heO5GfgWtSDqwso2Mx2JUA4R-3seAQIBOj3QOutX4Sdtpy4u8txJJvAnRKaK_YlJ9jdjzz_i8kcacVhjnPKUPw0pXd3jusKuJE793zeh_HgSS7zU5FZRxE8VcpUIGVRAdSvMpocoKHZbMbpNNAByq5ozEYoSwDnmQeAVrpMA6Jmtvscy3jUzEZI2Ui2ZBNxIQqsi0CyqgoAGWdFy3eT5xaTqWQbybEoZMYrcLecoB_wmi2AON-BZxzQam4al0-6gIu_60_VxiMUZELTic6ATo5ypoxB3KXJmjlpUqbUCesJ6j8O-bDeWLDcU8vPyJgxQrnq7hSVijdeLWaWiDxFO_o3uniBycyen6RiIcGHKA7aaGXJosRr97Ql5OCBDd5bbY_pDL9KYcRdmH53A10NGtKTxzxRjjDIZC2uegp0wIo1imhHcbTWii76OBLkxKsibzZQrjITWVSxJvhiMwOFblePdxy-cTJMyGFqiXqBpf_mzgKaUwoqUMjGje_f3XE9BchSMXKhOpMdPDMdycrC4_2fkr_nvuVztXNfplqiHIwiwL2Kjrm9CPR7OPJRRkZolHT4PR4B4Rm1j9zdKyH70L6f6VsB0ksU82E1hbCxDOyeV3binEBrUwaAJihallMiHAyzDtEMtzzpK1lkkj8uZRM2SWkCSkafkDae2KrG-la9vqMEL4MaR3UOfVzG3xp1Bm6imO1rZy8AQ82zceG6XcFEXu3-wnCha4M6gF8YZJi5wOhmiTrShHYubPLaMe4JxNCjHdySR7VHsDk4v6_2gKClfg3P4Pg69VOnPiBOmEH8_S8Nu5DE-BFWeqlJDcj7RS3aQxdD3DuzXfwAc-6xzvxKIkKx00ChxtwUY6Kx0ORzUi6bXx8zMmk1GR7rXlEribU7GrgFaZ6YrChoxJnT4OrGUYJIp5aQINsm_-hZqgcwOdk4BQ8w3qreQA8DJGYcYezRuwYeau-1JHJpGVXdrEcFhj6FhjTjDdOrqH-sM3319-2cL1Gw3zaxh40_hM9FlJOY3LomQdljyOZ34803oFpPZq07T8gfgl58IyOnbjIwwKK1v7lsc3qiF848--ER4AdgCNGNgupX1aW_srpOZ1Dt3eJCzrUAvv4iMbJ5uOuOr_GC2CgTwUJgLI3y174Mth9UdG-qAb5KYAUzGIS4FaVmqlUAFui_ql9_vg7WoozdIMikdQgMebV2dQlM18wzGf-QrDnkVgxN6Flti1ZmMB2UWGv1wdFOunjVWiZbYJH75T86hEzijKSgUQ5aQf3UGReSR0Y51VmGkC9OuO_PJvFGyeyC6M3MoC1tmac0vKjvOzuAhN_1PtAHmXu-wa7H_8QDLUjOs02fyf6HZLJfbVCTNBDGd0TKWSnsIcYVLZq_DxoJpOoeOTvVO6ya0u1Nln8cdoAAUgT1jRZe9GZyBMAFJsEdNbA3JMdCdjx-IdBXh80WmbWuUTmZLAUgHNGccJ5BxwqkiLpf3Od86atjbjse48DEtp-47nN2Mca60aajLQZ4YumTka5XNgBf2OrHWnbryLSXhdtgoeAICH1fi8UnfXePZvhW2emTLCTadq3dQA3IvdqC_LIhXj_T9lWb4zMXLPdndg2koMpIg_1hUGiveynH2FctBqOttT1PhVljp_hcVf9M2MQg61TvHPzI41A_YAFmHYqwNPzo3pPnlkoKNZdwIaBvmNaBpDWBfVjueWvkeBWMxejOSdSb0jM4tAuzGFYhxrdMIDzdBccrj55y9si3FDldtzC1jqvHvrb64-OK7QiVgDFpvwJTts4bBOIeQ5HOhKh-Johom-jfQgdrFw9BJGlmQrcb8NK34ORYku1t46z8beLa36T8rH6k1_ShunR1nORsqgsOLMy1FkcjYeQ-tEbVi_coikxk9ZYKDXXUHVE1_izhnJWiPFgYTpgTiudnBAAguXWEuVjBQS2aUWz0vuCk61-_UIJ4BrZltaKZ8PzR5yD_GAngCUa8TIn6KOnyZUqeb5g3wDqAPRnLBamYt_KNtyFDtGoiFoGEcsABFaOUchcOt98Ju83ay4cb8VAoHBmd2f2HGTyqxjn_RaS0QhrtQ11rPfHeCOjQ5b_aEepat43sIUH9hMuzbwhJQwxWb5VG4KkGK6HEV1XNX4Hn4mmhMk4JevaI4GZfp6E03-uHcH_NCRVsZjLMaF2V49h6l_NS4Y2hoW6jZpr5cnkNRKOEtjXDeH3vJmaXsdb0_-EaeJCgwFOqHSJUKP9aHcBZCroke1TkqP-Yv94CqRbIZ-sMdCzw-UJvdt0ltUqC_mNB72j7YHyiWpg0uftlWf7VHdT84geuRgar-CAgY1E7lLBFBnAniOp3g51VpdnWtUZvq_PHsFUliIrf5fosqoubhHu_XxoWVJ-r5dRHwT9y5w3CMxOzJpx2QCrAafDNiRz2LZ_TUhvf_7QxG7vSHwdAFyt-NIjOFRwovHADG6wwo0IoDPj3UuThM02z16EbPKJPJF4STOqMD7uWKZckx1Y4QSdvg8IuIYCLUyK6cNoJthg9ME5BnHVufkZ83j2sAjlzxGS20DXRwW9H6x5sUCADuW6Itl96cDCLo6C-mdZ9dppy0hFIBYvuzF3CyF4aZL49XkAa8iTh2OCgMr_gBST0H9Oe19IN4c-4kuRNIc4_vXo7husrEoZVNMy1qMZROIj2Vdr_2saf8fN_vqwUdoVbztheteT4OWW_ALq_121nzCBTmEID4q3D0G5NpWeItD2k6w6YaehioKIwhRmSTQQ_AlvxrIk5tHbJ7PtpP7PyFovvtb3ziMqjNd4ZOURUjF0Q2vmIaP6JfpYpfAgi7TlM0RNMboXymTEVib_5JpgaHszPfXdinymCTlpoLPrJokewYzK4NkMV_NOex8.NYcQRWPlKV0Qi_XxCGvapA", null);
            
        }

        [HttpGet]
        [Route("api/myinfo-authorise/{companyId}")]
        public string GetAuthorise(int companyId)
        {
            return new MyInfoUtility(Util).GetAuthorise(companyId);
        }
        [HttpGet]
        [Route("api/myinfo-authorise/{companyId}/{email}")]
        public ResponseModel GetAuthoriseInvite(int companyId,string email)
        {
            return new MyInfoUtility(Util).GetAuthoriseInvite(companyId,email);
        }
        [HttpGet]
        [Route("api/myinfo-filling-otp/{requestId}/{email}")]
        public ResponseModel GetAuthoriseOTP(int requestId, string email)
        {
            return new MyInfoUtility(Util).GetAuthoriseOTP(requestId, email);
        }
        [HttpGet]
        [Route("api/myinfo-validate-otp/{requestId}/{email}/{otp}")]
        public string GetAuthoriseWithOTP(int requestId, string email,string otp)
        {
            return new MyInfoUtility(Util).GetAuthoriseWithOTP(requestId, email, otp);
        }

        [HttpGet]
        [Route("api/myinfo-person")]
        public List<DropDownModel> GetPerson(string code, string state)
        {

            return new MyInfoUtility(Util).GetPerson(
                ConfigurationManager.AppSettings["RoboDocPath"],code,state);
        }


        [HttpGet]
        [Route("api/myinfo-person/{state}")]
        public List<DropDownModel> InsertPerson(string state)
        {

            return new MyInfoUtility(Util).InsertPerson(
                            ConfigurationManager.AppSettings["RoboDocPath"], state);
        }

        [HttpGet]
        [Route("api/myinfo/{state}")]
        public BusinessOfficerModel GetBusinessOfficerWithMyInfo(string state)
        {
            return new MyInfoUtility(Util).GetBusinessOfficerWithMyInfo(state);
        }

    }


}
