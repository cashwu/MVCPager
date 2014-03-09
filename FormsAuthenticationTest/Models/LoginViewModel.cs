using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace FormsAuthenticationTest.Models
{
    public class LoginViewModel : IValidatableObject
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string Password { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //將使用者輸入的字串轉成Base64String
            string base64Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(Password));
            //todo到DB抓使用者資料
            //假如抓不到系統使用者資料
            //※為了Demo用這種寫法，實際請換成判斷DB的資料存不存在
            if (!(Account == "cash" && base64Password == "Y2FzaDEyMzQ="))
            {
                yield return new ValidationResult("無此帳號或密碼", new string[] { "Account" });
            }
        }

    }
}