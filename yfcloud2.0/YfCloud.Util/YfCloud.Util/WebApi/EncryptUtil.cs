using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YfCloud.Utilities.WebApi
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public static class EncryptUtil
    {

        private static byte[] ConvertBase64ToBytes(string base64)
        {
            char[] charBuffer = base64.ToCharArray();
            byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);

            return bytes;
        }


        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="contentstring">base64</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DeAESbyKey(string contentstring, string key)
        {
            byte[] content = ConvertBase64ToBytes(contentstring);

            byte[] enCodeFormat = Convert.FromBase64String(key);

            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = enCodeFormat;
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = content;
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Encoding.UTF8.GetString(results);
                }
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="contentstring">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string EnAESBykey(string contentstring,  string key)
        {
            byte[] content = Encoding.UTF8.GetBytes(contentstring);
            byte[] enCodeFormat = Convert.FromBase64String(key);

            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = enCodeFormat;
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = content;

                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Convert.ToBase64String(results);
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="contentstring">base64</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DeAESbyKey(string contentstring, System.Text.Encoding encoding, string key)
        {
            byte[] content = ConvertBase64ToBytes(contentstring);

            byte[] enCodeFormat = Convert.FromBase64String(key);

            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = enCodeFormat;
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = content;
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return encoding.GetString(results);
                }
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="contentstring">要加密的字符串</param>
        /// <param name="encoding">编码</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string EnAESBykey(string contentstring, System.Text.Encoding encoding, string key)
        {
            byte[] content = encoding.GetBytes(contentstring);
            byte[] enCodeFormat = Convert.FromBase64String(key);

            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = enCodeFormat;
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = content;

                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Convert.ToBase64String(results);
                }
            }
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string encryptString)
        {
            return MD5Encrypt(encryptString, Encoding.Default.BodyName);
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string encryptString, string encodeType)
        {
            if (string.IsNullOrEmpty(encryptString))
                throw new Exception("密文不得为空");

            using (var provider = new MD5CryptoServiceProvider())
            {
                var encrypt = provider.ComputeHash(Encoding.GetEncoding(encodeType).GetBytes(encryptString));

                var sb = new StringBuilder(32);

                for (int i = 0; i < encrypt.Length; i++)
                    sb.Append(encrypt[i].ToString("x").PadLeft(2, '0'));

                return sb.ToString();
            }
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md532(string str)
        {
            var md5Csp = new MD5CryptoServiceProvider();
            byte[] md5Source = Encoding.UTF8.GetBytes(str);
            byte[] md5Out = md5Csp.ComputeHash(md5Source);
            string pwd = "";
            for (int i = 0; i < md5Out.Length; i++)
            {
                pwd += md5Out[i].ToString("x2");
            }
            return pwd;
        }

        /// <summary>
        　/// Base64加密
        　/// </summary>
        　/// <param name="Message"></param>
        　/// <returns></returns>
        public static string Base64Code(string Message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        　/// Base64解密
        　/// </summary>
        　/// <param name="Message"></param>
        　/// <returns></returns>
        public static string Base64Decode(string Message)
        {
            byte[] bytes = Convert.FromBase64String(Message);
            return Encoding.UTF8.GetString(bytes);
        }

        #region RSA

        /// <summary>
        /// RSA验签
        /// </summary>
        /// <param name="content">验签数据</param>
        /// <param name="signedString">随数据签名</param>
        /// <returns>boos true or false</returns>
        public static bool RsaSHA256VerifySign(string content, string signedString, string publicKey)
        {
            RSAParameters para = ConvertFromPemPublicKey(publicKey);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(para);

            //X509Certificate2 publicCer = new X509Certificate2(publicKey);
            // var rsaCrypto = (RSACryptoServiceProvider)publicCer.PublicKey.Key;
            SHA256 s256 = new SHA256Managed();
            //对签名数据进行转码
            byte[] dataBytes = Encoding.GetEncoding("utf-8").GetBytes(content);
            //对签名进行解码
            byte[] signBytes = Convert.FromBase64String(signedString);

            return rsa.VerifyData(dataBytes, s256, signBytes);
        }

        /// <summary>
        /// RSA 加签
        /// </summary>
        /// <param name="dataStr">加签数据</param>
        /// <param name="privateKey">密钥</param>
        /// <returns>base64编码后的签名</returns>
        public static string RsaSHA256Sign(string dataStr, string privateKey)
        {
            RSACryptoServiceProvider rsa = ConvertFromPemPrivate_Key(privateKey);
            SHA256 s256 = new SHA256Managed();
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataStr);
            byte[] signBytes = rsa.SignData(dataBytes, s256);
            return Convert.ToBase64String(signBytes);
        }

        /// <summary>
        /// RSA OPENSSL加密
        /// </summary>
        /// <param name="content">需要加密的数据 string类型</param>
        /// <param name="publickKey">公钥</param>
        /// <returns>加密后的base64转码数据</returns>
        public static String RSAPEMEncrypt(String content, string publickKey)
        {
            RSAParameters para = ConvertFromPemPublicKey(publickKey);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(para);
            byte[] dataBytes = Encoding.UTF8.GetBytes(content);
            byte[] resultBytes = rsa.Encrypt(dataBytes, false);
            String result = Convert.ToBase64String(resultBytes);
            return result;
        }

        /// <summary>
        /// RSA openssl PEM格式秘钥解密
        /// </summary>
        /// <param name="content">需要解密的数据</param>
        /// <param name="privateKey">密钥</param>
        /// <returns>解密后的数据</returns>
        public static String RSAPEMDecrypter(String content, string privateKey)
        {
            RSAParameters para = ConvertFromPemPrivateKey(privateKey);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(para);
            byte[] data = Convert.FromBase64String(content);
            byte[] resultBytes = rsa.Decrypt(data, false);
            String reuslt = Encoding.UTF8.GetString(resultBytes);
            return reuslt;
        }

        /// <summary>
        /// 将pem格式私钥(1024 or 2048)转换为RSAParameters
        /// </summary>
        /// <param name="pemFileConent">pem私钥内容</param>
        /// <returns>转换得到的RSAParamenters</returns>
        private static RSAParameters ConvertFromPemPrivateKey(string pemFileConent)
        {
            if (string.IsNullOrEmpty(pemFileConent))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }
            pemFileConent = pemFileConent.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemFileConent);

            bool keySize1024 = (keyData.Length == 609 || keyData.Length == 610);
            bool keySize2048 = (keyData.Length == 1190 || keyData.Length == 1192);

            if (!(keySize1024 || keySize2048))
            {
                //   throw new ArgumentException("pem file content is incorrect, Only support the key size is 1024 or 2048");
            }

            int index = (keySize1024 ? 11 : 12);
            byte[] pemModulus = (keySize1024 ? new byte[128] : new byte[256]);
            Array.Copy(keyData, index, pemModulus, 0, pemModulus.Length);

            index += pemModulus.Length;
            index += 2;
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, index, pemPublicExponent, 0, 3);

            index += 3;
            index += 4;
            if ((int)keyData[index] == 0)
            {
                index++;
            }
            byte[] pemPrivateExponent = (keySize1024 ? new byte[128] : new byte[256]);
            Array.Copy(keyData, index, pemPrivateExponent, 0, pemPrivateExponent.Length);

            index += pemPrivateExponent.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemPrime1 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemPrime1, 0, pemPrime1.Length);

            index += pemPrime1.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemPrime2 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemPrime2, 0, pemPrime2.Length);

            index += pemPrime2.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemExponent1 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemExponent1, 0, pemExponent1.Length);

            index += pemExponent1.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemExponent2 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemExponent2, 0, pemExponent2.Length);

            index += pemExponent2.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemCoefficient = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemCoefficient, 0, pemCoefficient.Length);

            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            para.D = pemPrivateExponent;
            para.P = pemPrime1;
            para.Q = pemPrime2;
            para.DP = pemExponent1;
            para.DQ = pemExponent2;
            para.InverseQ = pemCoefficient;
            return para;
        }

        /// <summary>
        /// 将pem格式公钥(1024 or 2048)转换为RSAParameters
        /// </summary>
        /// <param name="pemFileConent">pem公钥内容</param>
        /// <returns>转换得到的RSAParamenters</returns>
        private static RSAParameters ConvertFromPemPublicKey(string pemFileConent)
        {
            if (string.IsNullOrEmpty(pemFileConent))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }
            pemFileConent = pemFileConent.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemFileConent);
            bool keySize1024 = (keyData.Length == 162);
            bool keySize2048 = (keyData.Length == 294);
            if (!(keySize1024 || keySize2048))
            {
                throw new ArgumentException("pem file content is incorrect, Only support the key size is 1024 or 2048");
            }
            byte[] pemModulus = (keySize1024 ? new byte[128] : new byte[256]);
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, (keySize1024 ? 29 : 33), pemModulus, 0, (keySize1024 ? 128 : 256));
            Array.Copy(keyData, (keySize1024 ? 159 : 291), pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        /// <summary>
        /// 将pem格式私钥(1024 or 2048)转换为RSAParameters
        /// </summary>
        /// <param name="pemFileConent">pem私钥内容</param>
        /// <returns>转换得到的RSAParamenters</returns>
        private static RSACryptoServiceProvider ConvertFromPemPrivate_Key(string pemFileConent)
        {
            if (string.IsNullOrEmpty(pemFileConent))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }
            pemFileConent = pemFileConent.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemFileConent);

            return DecodePrivateKeyInfo(keyData);
        }

        /// <summary>
        /// ------- Parses binary asn.1 PKCS #8 PrivateKeyInfo; returns RSACryptoServiceProvider ---
        /// </summary>
        /// <param name="pkcs8"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            // this byte[] includes the sequence byte and terminal encoded null
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)	//expect an Octet string
                    return null;

                bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }

        /// <summary>
        /// ------- Parses binary ans.1 RSA private key; returns RSACryptoServiceProvider  ---
        /// </summary>
        /// <param name="privkey"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)	//version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                Console.WriteLine("showing components ..");

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binr"></param>
        /// <returns></returns>
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        #endregion RSA
    }
}
