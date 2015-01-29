
namespace DB.Abstraction
{


    public static class MyExtensionMethods
    {

        //public static bool Contains(this List<string> ls, string strValueToSearch, System.StringComparer cmp)
        public static bool Contains(System.Collections.Generic.List<string> ls, string strValueToSearch, System.StringComparer cmp)
        {
            foreach (string strThisValue in ls)
            {
                if (cmp.Equals(strThisValue, strValueToSearch))
                    return true;
            } // Next strThisValue

            return false;
        } // End Extension Function List<string>.Contains


        //public static string[] ToNameArray(this System.Reflection.MemberInfo[] arrMi)
        public static string[] ToNameArray(System.Reflection.MemberInfo[] arrMi)
        {
            string[] arr = new string[arrMi.Length];

            for (int i = 0; i < arrMi.Length; ++i)
            {
                arr[i] = arrMi[i].Name;
            } // Next i

            return arr;
        } // End Extension Function System.Reflection.MemberInfo[].ToNameArray


    } // End Class MyExtensionMethods


} // End Namespace System.Collections.Generic
