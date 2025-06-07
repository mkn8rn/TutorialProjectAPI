using Microsoft.AspNetCore.Mvc;

namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialController : ControllerBase
    {
        public readonly Random _random = new();

        [HttpGet("bool")]
        public object GetBoolData() => new
        {
            randomValue = _random.Next(2) == 1,
            aboutType = "bool: true or false value"
        };

        [HttpGet("byte")]
        public object GetByteData() => new
        {
            randomValue = (byte)_random.Next(0, 256),
            aboutType = "byte: 8-bit unsigned integer"
        };

        [HttpGet("sbyte")]
        public object GetSByteData() => new
        {
            randomValue = (sbyte)_random.Next(sbyte.MinValue, sbyte.MaxValue),
            aboutType = "sbyte: 8-bit signed integer"
        };

        [HttpGet("short")]
        public object GetShortData() => new
        {
            randomValue = (short)_random.Next(short.MinValue, short.MaxValue),
            aboutType = "short: 16-bit signed integer"
        };

        [HttpGet("ushort")]
        public object GetUShortData() => new
        {
            randomValue = (ushort)_random.Next(0, ushort.MaxValue),
            aboutType = "ushort: 16-bit unsigned integer"
        };

        [HttpGet("int")]
        public object GetIntData() => new
        {
            randomValue = _random.Next(),
            aboutType = "int: 32-bit signed integer"
        };

        [HttpGet("uint")]
        public object GetUIntData() => new
        {
            randomValue = (uint)_random.Next(0, int.MaxValue),
            aboutType = "uint: 32-bit unsigned integer"
        };

        [HttpGet("long")]
        public object GetLongData() => new
        {
            randomValue = (long)_random.Next() << 32 | (long)_random.Next(),
            aboutType = "long: 64-bit signed integer"
        };

        [HttpGet("ulong")]
        public object GetULongData() => new
        {
            randomValue = (ulong)_random.Next() << 32 | (ulong)_random.Next(),
            aboutType = "ulong: 64-bit unsigned integer"
        };

        [HttpGet("float")]
        public object GetFloatData() => new
        {
            randomValue = (float)_random.NextDouble(),
            aboutType = "float: 32-bit floating-point number"
        };

        [HttpGet("double")]
        public object GetDoubleData() => new
        {
            randomValue = _random.NextDouble(),
            aboutType = "double: 64-bit floating-point number"
        };

        [HttpGet("decimal")]
        public object GetDecimalData() => new
        {
            randomValue = (decimal)_random.NextDouble(),
            aboutType = "decimal: high-precision floating-point"
        };

        [HttpGet("char")]
        public object GetCharData() => new
        {
            randomValue = (char)_random.Next(65, 91),
            aboutType = "char: single 16-bit Unicode character"
        };

        [HttpGet("string")]
        public object GetStringData() => new
        {
            randomValue = "value" + _random.Next(100),
            aboutType = "string: FUCk NIGGERS"
        };

        [HttpGet("object")]
        public object GetObjectData() => new
        {
            randomValue = new { example = "object" },
            aboutType = "object: base type of all types"
        };
    }
}
