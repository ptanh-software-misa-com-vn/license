using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anh.License
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GenerateToken()
        {
            string token = TokenUtil.CreateToken();
            Assert.IsTrue(true);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestLicense()
        {
            string keyData = "MYybB3s0zsc36uVa+utgoPGjYHCiQh8LWoaABHbarU+AD2DXg9tdUOTWfjiOaB9IVqnReDJc0oMRYeMy4AjGVA==";
            SerialnumberHDD ser = new SerialnumberHDD();
            string serialHDD = ser.GetDriveSerialNumber();
            // get key from hdd serial with token sha1
            string key = ser.CreateKey(serialHDD);
            Assert.AreEqual(key, keyData);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetDriveSerialNumber()
        {
            SerialnumberHDD ser = new SerialnumberHDD();
            string serialHDD = ser.GetDriveSerialNumber();
            Assert.IsTrue(true);
        }
    }
}
