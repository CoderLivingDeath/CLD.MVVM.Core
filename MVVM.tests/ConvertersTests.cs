using MVVM.ConvertersLibrary;
using System.Globalization;

namespace MVVM.tests
{
    [TestClass]
    public class ValueConvertersTests
    {
        private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

        [TestMethod]
        public void IntConverter_Convert_ValidString_ReturnsInt()
        {
            var converter = new IntConverter();
            var result = converter.Convert("123", typeof(int), null, _culture);
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void IntConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new IntConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(int), null, _culture));
        }

        [TestMethod]
        public void IntConverter_ConvertBack_ValidInt_ReturnsString()
        {
            var converter = new IntConverter();
            var result = converter.ConvertBack(123, typeof(string), null, _culture);
            Assert.AreEqual("123", result);
        }

        [TestMethod]
        public void IntConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new IntConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not an int", typeof(string), null, _culture));
        }

        [TestMethod]
        public void DoubleConverter_Convert_ValidString_ReturnsDouble()
        {
            var converter = new DoubleConverter();
            var result = converter.Convert("123.45", typeof(double), null, _culture);
            Assert.AreEqual(123.45, result);
        }

        [TestMethod]
        public void DoubleConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new DoubleConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(double), null, _culture));
        }

        [TestMethod]
        public void DoubleConverter_ConvertBack_ValidDouble_ReturnsString()
        {
            var converter = new DoubleConverter();
            var result = converter.ConvertBack(123.45, typeof(string), null, _culture);
            Assert.AreEqual("123.45", result);
        }

        [TestMethod]
        public void DoubleConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new DoubleConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a double", typeof(string), null, _culture));
        }

        [TestMethod]
        public void BoolConverter_Convert_ValidString_ReturnsBool()
        {
            var converter = new BoolConverter();
            var result = converter.Convert("true", typeof(bool), null, _culture);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void BoolConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new BoolConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(bool), null, _culture));
        }

        [TestMethod]
        public void BoolConverter_ConvertBack_ValidBool_ReturnsString()
        {
            var converter = new BoolConverter();
            var result = converter.ConvertBack(true, typeof(string), null, _culture);
            Assert.AreEqual("True", result);
        }

        [TestMethod]
        public void BoolConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new BoolConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a bool", typeof(string), null, _culture));
        }

        [TestMethod]
        public void DateTimeConverter_Convert_ValidString_ReturnsDateTime()
        {
            var converter = new DateTimeConverter();
            var result = converter.Convert("2023-12-31", typeof(DateTime), null, _culture);
            Assert.AreEqual(new DateTime(2023, 12, 31), result);
        }

        [TestMethod]
        public void DateTimeConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new DateTimeConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(DateTime), null, _culture));
        }

        [TestMethod]
        public void DateTimeConverter_ConvertBack_ValidDateTime_ReturnsString()
        {
            var converter = new DateTimeConverter();
            var date = new DateTime(2023, 12, 31);
            var result = converter.ConvertBack(date, typeof(string), "yyyy-MM-dd", _culture);
            Assert.IsTrue(result.ToString().Contains("2023-12-31"));
        }

        [TestMethod]
        public void DateTimeConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new DateTimeConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a datetime", typeof(string), null, _culture));
        }

        [TestMethod]
        public void DecimalConverter_Convert_ValidString_ReturnsDecimal()
        {
            var converter = new DecimalConverter();
            var result = converter.Convert("123.45", typeof(decimal), null, _culture);
            Assert.AreEqual(123.45m, result);
        }

        [TestMethod]
        public void DecimalConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new DecimalConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(decimal), null, _culture));
        }

        [TestMethod]
        public void DecimalConverter_ConvertBack_ValidDecimal_ReturnsString()
        {
            var converter = new DecimalConverter();
            var result = converter.ConvertBack(123.45m, typeof(string), null, _culture);
            Assert.AreEqual("123.45", result);
        }

        [TestMethod]
        public void DecimalConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new DecimalConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a decimal", typeof(string), null, _culture));
        }

        [TestMethod]
        public void FloatConverter_Convert_ValidString_ReturnsFloat()
        {
            var converter = new FloatConverter();
            var result = converter.Convert("123.45", typeof(float), null, _culture);
            Assert.AreEqual(123.45f, result);
        }

        [TestMethod]
        public void FloatConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new FloatConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(float), null, _culture));
        }

        [TestMethod]
        public void FloatConverter_ConvertBack_ValidFloat_ReturnsString()
        {
            var converter = new FloatConverter();
            var result = converter.ConvertBack(123.45f, typeof(string), null, _culture);
            Assert.AreEqual("123.45", result);
        }

        [TestMethod]
        public void FloatConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new FloatConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a float", typeof(string), null, _culture));
        }

        [TestMethod]
        public void LongConverter_Convert_ValidString_ReturnsLong()
        {
            var converter = new LongConverter();
            var result = converter.Convert("123456789", typeof(long), null, _culture);
            Assert.AreEqual(123456789L, result);
        }

        [TestMethod]
        public void LongConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new LongConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(long), null, _culture));
        }

        [TestMethod]
        public void LongConverter_ConvertBack_ValidLong_ReturnsString()
        {
            var converter = new LongConverter();
            var result = converter.ConvertBack(123456789L, typeof(string), null, _culture);
            Assert.AreEqual("123456789", result);
        }

        [TestMethod]
        public void LongConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new LongConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a long", typeof(string), null, _culture));
        }

        [TestMethod]
        public void ShortConverter_Convert_ValidString_ReturnsShort()
        {
            var converter = new ShortConverter();
            var result = converter.Convert("123", typeof(short), null, _culture);
            Assert.AreEqual((short)123, result);
        }

        [TestMethod]
        public void ShortConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new ShortConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(short), null, _culture));
        }

        [TestMethod]
        public void ShortConverter_ConvertBack_ValidShort_ReturnsString()
        {
            var converter = new ShortConverter();
            var result = converter.ConvertBack((short)123, typeof(string), null, _culture);
            Assert.AreEqual("123", result);
        }

        [TestMethod]
        public void ShortConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new ShortConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a short", typeof(string), null, _culture));
        }

        [TestMethod]
        public void ByteConverter_Convert_ValidString_ReturnsByte()
        {
            var converter = new ByteConverter();
            var result = converter.Convert("123", typeof(byte), null, _culture);
            Assert.AreEqual((byte)123, result);
        }

        [TestMethod]
        public void ByteConverter_Convert_InvalidString_ThrowsFormatException()
        {
            var converter = new ByteConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.Convert("invalid", typeof(byte), null, _culture));
        }

        [TestMethod]
        public void ByteConverter_ConvertBack_ValidByte_ReturnsString()
        {
            var converter = new ByteConverter();
            var result = converter.ConvertBack((byte)123, typeof(string), null, _culture);
            Assert.AreEqual("123", result);
        }

        [TestMethod]
        public void ByteConverter_ConvertBack_InvalidType_ThrowsFormatException()
        {
            var converter = new ByteConverter();
            Assert.ThrowsExactly<FormatException>(() => converter.ConvertBack("not a byte", typeof(string), null, _culture));
        }
    }
}
