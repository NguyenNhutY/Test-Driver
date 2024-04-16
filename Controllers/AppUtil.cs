using System.Text.RegularExpressions;

namespace DemoGPLX.Controllers
{
    public class AppUtil
    {
       public static string ToLowerCaseNonAccentVietnamese(string str)
        {
            str = str.ToLower();
            str = Regex.Replace(str, @"à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ", "a");
            str = Regex.Replace(str, @"è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ", "e");
            str = Regex.Replace(str, @"ì|í|ị|ỉ|ĩ", "i");
            str = Regex.Replace(str, @"ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ", "o");
            str = Regex.Replace(str, @"ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ", "u");
            str = Regex.Replace(str, @"ỳ|ý|ỵ|ỷ|ỹ", "y");
            str = Regex.Replace(str, @"đ", "d");
            str = Regex.Replace(str, @"\u0300|\u0301|\u0303|\u0309|\u0323", ""); // Huyền sắc hỏi ngã nặng 
            str = Regex.Replace(str, @"\u02C6|\u0306|\u031B", ""); // Â, Ê, Ă, Ơ, Ư
            return str;
        }

        public static string ToNonAccentVietnamese(string str)
        {
            str = Regex.Replace(str, @"A|Á|À|Ã|Ạ|Â|Ấ|Ầ|Ẫ|Ậ|Ă|Ắ|Ằ|Ẵ|Ặ", "A");
            str = Regex.Replace(str, @"à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ", "a");
            str = Regex.Replace(str, @"E|É|È|Ẽ|Ẹ|Ê|Ế|Ề|Ễ|Ệ", "E");
            str = Regex.Replace(str, @"è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ", "e");
            str = Regex.Replace(str, @"I|Í|Ì|Ĩ|Ị", "I");
            str = Regex.Replace(str, @"ì|í|ị|ỉ|ĩ", "i");
            str = Regex.Replace(str, @"O|Ó|Ò|Õ|Ọ|Ô|Ố|Ồ|Ỗ|Ộ|Ơ|Ớ|Ờ|Ỡ|Ợ", "O");
            str = Regex.Replace(str, @"ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ", "o");
            str = Regex.Replace(str, @"U|Ú|Ù|Ũ|Ụ|Ư|Ứ|Ừ|Ữ|Ự", "U");
            str = Regex.Replace(str, @"ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ", "u");
            str = Regex.Replace(str, @"Y|Ý|Ỳ|Ỹ|Ỵ", "Y");
            str = Regex.Replace(str, @"ỳ|ý|ỵ|ỷ|ỹ", "y");
            str = Regex.Replace(str, @"Đ", "D");
            str = Regex.Replace(str, @"đ", "d");
            str = Regex.Replace(str, @"\u0300|\u0301|\u0303|\u0309|\u0323", ""); // Huyền sắc hỏi ngã nặng 
            str = Regex.Replace(str, @"\u02C6|\u0306|\u031B", ""); // Â, Ê, Ă, Ơ, Ư
            return str;
        }
    }
}
