
namespace DB.Abstraction
{


    public static class IEnumerableExtensions
    {


        // System.IEnumerableExtensions.Contains(list, "")
        public static bool Contains(System.Collections.IEnumerable enStringList, string strKey)
        {
            foreach (string strItem in enStringList)
            {
                if (System.StringComparer.OrdinalIgnoreCase.Equals(strItem, strKey))
                    return true;
            } // Next strItem

            return false;
        } // End Function Contains


        // System.IEnumerableExtensions.ConcatItems
        public static string ConcatItems(System.Collections.IEnumerable enStringList, string strSeparator)
        {
            string strReturnValue = "";

            System.Collections.Generic.List<string> lsItemList = new System.Collections.Generic.List<string>();

            foreach (string strItem in enStringList)
            {
                if(!string.IsNullOrEmpty(strItem))
                    lsItemList.Add(strItem);
            } // Next strItem

            strReturnValue = string.Join(strSeparator, lsItemList.ToArray());
            lsItemList.Clear();
            lsItemList = null;

            return strReturnValue;
        } // End ConcatItems


        /*
        public static string Concat(this System.Collections.IEnumerable stringList)
        {
            System.Text.StringBuilder textBuilder = new System.Text.StringBuilder();
            string separator = String.Empty;
            foreach (string item in stringList)
            {
                textBuilder.Append(separator);
                textBuilder.Append(item);
                separator = ", ";
            } // Next item

            return textBuilder.ToString();
        } // End Function Concat
        */


    } // End Class IEnumerableExtensions


} // End Namespace System
