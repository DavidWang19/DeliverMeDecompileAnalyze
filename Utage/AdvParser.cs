namespace Utage
{
    using System;
    using System.Runtime.InteropServices;

    public class AdvParser
    {
        public static bool IsEmptyCell(StringGridRow row, AdvColumnName name)
        {
            return row.IsEmptyCell(Localize(name));
        }

        public static string Localize(AdvColumnName name)
        {
            return name.QuickToString();
        }

        public static T ParseCell<T>(StringGridRow row, AdvColumnName name)
        {
            return row.ParseCell<T>(Localize(name));
        }

        public static T ParseCellOptional<T>(StringGridRow row, AdvColumnName name, T defaultVal)
        {
            return row.ParseCellOptional<T>(Localize(name), defaultVal);
        }

        public static bool TryParseCell<T>(StringGridRow row, AdvColumnName name, out T val)
        {
            return row.TryParseCell<T>(Localize(name), out val);
        }
    }
}

