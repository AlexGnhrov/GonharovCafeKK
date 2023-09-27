using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace TC_Application.AppFolder.GlobalClassFolder
{
    public static class ValidationDataClass
    {
        public static void OnlyNumsTB(this TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        public static void OnlyEnglish(this TextCompositionEventArgs e)
        {
            e.Handled = !new Regex($"^[a-zA-Z0-9_';:-@!#$%^&*()+№]*$").IsMatch(e.Text);
        }

        public static void Login(this TextCompositionEventArgs e)
        {
            e.Handled = !new Regex(@"^[a-zA-Z0-9_!?]*$").IsMatch(e.Text);
        }

        public static void FloatNums(this TextBox textBox)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', 'б' };

            bool CommaIsUsing = false;
            int i = 1;

            byte afterCommaleng = 0;

            foreach (char c in textBox.Text)
            {

                if (Array.IndexOf(validChars, c) != -1)
                {

                    if (c == ',' || c == '.' || c == 'б')
                    {
                        if (i != 1 && !CommaIsUsing)
                        {

                            result += ",";
                            CommaIsUsing = true;
                        }
                    }
                    else if (CommaIsUsing && afterCommaleng < 2)
                    {
                        result += c;
                        ++afterCommaleng;
                    }
                    else if (result.Length < textBox.MaxLength-3 && afterCommaleng < 2)
                    {
                        result += c;
                    }
                }


                ++i;
            }

            textBox.Text = result;



            if (!Keyboard.IsKeyDown(Key.Back))
            {

                if (textBox.CaretIndex == textBox.Text.Length ||
                    textBox.CaretIndex == 0)
                {
                    textBox.CaretIndex = textBox.Text.Length;
                }


            }
        }


    }
}
