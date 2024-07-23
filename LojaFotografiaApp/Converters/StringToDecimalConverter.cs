using System;
using Windows.UI.Xaml.Data;

namespace LojaFotografiaApp.Converters
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("G"); // Formata o decimal como string
            }
            return value?.ToString(); // Caso o valor não seja decimal, converte para string
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (decimal.TryParse(value?.ToString(), out decimal decimalValue))
            {
                return decimalValue;
            }
            return 0m; // Retorna 0 se a conversão falhar
        }
    }
}
