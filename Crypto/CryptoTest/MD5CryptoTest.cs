using Crypto;
using NUnit.Framework;

namespace CryptoTest
{
    public class MD5CryptoTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Encrypt_ShouldSuccess()
        {
            var key = "mykey";
            var message = "123";
            var cipher = MD5Crypto.Encrypt(message, key);
            

            Assert.IsNotEmpty(cipher);
            Assert.AreEqual("q0N89UqEAdE=", cipher);
            
        }

        [Test]
        public void Decrypt_ShouldSuccess()
        {
            var key = "mykey";
            var cipher = "q0N89UqEAdE=";
            var message = MD5Crypto.Decrypt(cipher, key);
            
            Assert.IsNotEmpty(message);
            Assert.AreEqual("123", message);

        }
    }
}