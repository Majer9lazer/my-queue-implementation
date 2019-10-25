using System;
using System.IO;
using System.Linq;
using System.Text;
using MyQueue_Implementation.Modeling.Interfaces;

namespace MyQueue_Implementation.Core.Generators
{
    public class StrNumberGenerator : IStrNumberGenerator
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// Phone number format <example>###-###-##-##</example>
        /// </summary>
        private readonly string _format;

        /// <summary>
        /// format separator <example>-</example>
        /// </summary>
        private readonly char _separator;

        private readonly int _numbersCount;

        private readonly StringBuilder _builder;

        /// <summary>
        /// ctor of number generator based on format
        /// </summary>
        /// <param name="format">phone format like "###-###-##-##"</param>
        /// <param name="separator">separator like "-"</param>
        /// <param name="numbersCount"></param>
        public StrNumberGenerator(string format, char separator = '-', int numbersCount = 5)
        {
            if (format.GroupBy(g => g).Count() > 2)
            {
                throw new FormatException($"Invalid {nameof(format)} = {format}. Right should be \"###-###-##-##\"");
            }

            _format = format;
            _separator = separator;
            _numbersCount = numbersCount;
            _builder = new StringBuilder(_format.Length);
        }

        public string GenerateByCustomFormat(string format, char separator = '#')
        {
            foreach (var t in format)
            {
                if (t == separator)
                {
                    _builder.Append(Rnd.Next(0, 10));
                    continue;
                }

                _builder.Append(t);
            }

            var result = _builder.ToString();

            _builder.Clear();
            return result;
        }

        public string[] GenerateArray()
        {
            var result = new string[_numbersCount];

            for (int i = 0; i < _numbersCount; i++)
            {
                result[i] = this.GenerateSingle();
            }

            return result;
        }

        public string GenerateSingle()
        {
            var phoneChars = _format.Split(_separator);

            foreach (var c in phoneChars)
            {
                // ## = 10/100
                // ### = 100/1000
                // #### = 1000/10000

                var minRnd = (int)Math.Pow(10, c.Length - 1);
                var maxRnd = (int)Math.Pow(10, c.Length);

                _builder.Append(Rnd.Next(minRnd, maxRnd));
                _builder.Append(_separator);
            }

            _builder.Remove(_format.Length, 1);

            var res = _builder.ToString();
            _builder.Clear();

            return res;
        }
    }
}