﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace WebApplication1.Helpers
{

	internal class Encryption
	{


		// Encrypt a byte array into a byte array using a key and an IV 

		public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
		{
			// Create a MemoryStream to accept the encrypted bytes 

			MemoryStream ms = new MemoryStream();
			Rijndael alg = Rijndael.Create();

			alg.Key = Key;
			alg.IV = IV;

			CryptoStream cs = new CryptoStream(ms,
			   alg.CreateEncryptor(), CryptoStreamMode.Write);

			// Write the data and make it do the encryption 

			cs.Write(clearData, 0, clearData.Length);

			// Close the crypto stream (or do FlushFinalBlock). 

			cs.Close();

			// Now get the encrypted data from the MemoryStream.

			byte[] encryptedData = ms.ToArray();

			return encryptedData;
		}

		// Encrypt a string into a string using a password 

		//    Uses Encrypt(byte[], byte[], byte[]) 


		public static string Encrypt(string clearText, string Password)
		{
			// First we need to turn the input string into a byte array. 

			byte[] clearBytes =
			  System.Text.Encoding.Unicode.GetBytes(clearText);

			// Then, we need to turn the password into Key and IV 

			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			byte[] encryptedData = Encrypt(clearBytes,
					 pdb.GetBytes(32), pdb.GetBytes(16));

			return Convert.ToBase64String(encryptedData);

		}

		// Encrypt bytes into bytes using a password 

		//    Uses Encrypt(byte[], byte[], byte[]) 


		public static byte[] Encrypt(byte[] clearData, string Password)
		{

			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));

		}

		// Encrypt a file into another file using a password 

		public static void Encrypt(string fileIn,
					string fileOut, string Password)
		{

			// First we are going to open the file streams 

			FileStream fsIn = new FileStream(fileIn,
				FileMode.Open, FileAccess.Read);
			FileStream fsOut = new FileStream(fileOut,
				FileMode.OpenOrCreate, FileAccess.Write);

			// Then we are going to derive a Key and an IV from the

			// Password and create an algorithm 

			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			Rijndael alg = Rijndael.Create();
			alg.Key = pdb.GetBytes(32);
			alg.IV = pdb.GetBytes(16);

			CryptoStream cs = new CryptoStream(fsOut,
				alg.CreateEncryptor(), CryptoStreamMode.Write);


			int bufferLen = 4096;
			byte[] buffer = new byte[8];
			int bytesRead;

			do
			{
				// read a chunk of data from the input file 

				bytesRead = fsIn.Read(buffer, 0, bufferLen);

				// encrypt it 

				cs.Write(buffer, 0, bytesRead);
			} while (bytesRead != 0);

			// close everything 


			// this will also close the unrelying fsOut stream

			cs.Close();
			fsIn.Close();
		}

		// Decrypt a byte array into a byte array using a key and an IV 

		public static byte[] Decrypt(byte[] cipherData,
									byte[] Key, byte[] IV)
		{
			// Create a MemoryStream that is going to accept the

			// decrypted bytes 

			MemoryStream ms = new MemoryStream();

			Rijndael alg = Rijndael.Create();

			alg.Key = Key;
			alg.IV = IV;
			CryptoStream cs = new CryptoStream(ms,
				alg.CreateDecryptor(), CryptoStreamMode.Write);

			// Write the data and make it do the decryption 

			cs.Write(cipherData, 0, cipherData.Length);

			cs.Close();

			byte[] decryptedData = ms.ToArray();

			return decryptedData;
		}

		// Decrypt a string into a string using a password 

		//    Uses Decrypt(byte[], byte[], byte[]) 


		public static string Decrypt(string cipherText, string Password)
		{
			try
			{

				byte[] cipherBytes = Convert.FromBase64String(cipherText);


				PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
					new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});


				byte[] decryptedData = Decrypt(cipherBytes,
					pdb.GetBytes(32), pdb.GetBytes(16));



				return System.Text.Encoding.Unicode.GetString(decryptedData);
			}
			catch
			{
				return "wrongCode";
			}
		}

		// Decrypt bytes into bytes using a password 

		//    Uses Decrypt(byte[], byte[], byte[]) 


		public static byte[] Decrypt(byte[] cipherData, string Password)
		{


			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
		}

		// Decrypt a file into another file using a password 

		public static void Decrypt(string fileIn,
					string fileOut, string Password)
		{

			// First we are going to open the file streams 

			FileStream fsIn = new FileStream(fileIn,
						FileMode.Open, FileAccess.Read);
			FileStream fsOut = new FileStream(fileOut,
						FileMode.OpenOrCreate, FileAccess.Write);

			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
				new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
			Rijndael alg = Rijndael.Create();

			alg.Key = pdb.GetBytes(32);
			alg.IV = pdb.GetBytes(16);

			CryptoStream cs = new CryptoStream(fsOut,
				alg.CreateDecryptor(), CryptoStreamMode.Write);


			int bufferLen = 4096;
			byte[] buffer = new byte[8];
			int bytesRead;

			do
			{
				// read a chunk of data from the input file 

				bytesRead = fsIn.Read(buffer, 0, bufferLen);

				// Decrypt it 

				cs.Write(buffer, 0, bytesRead);

			} while (bytesRead != 0);

			// close everything 

			cs.Close(); // this will also close the unrelying fsOut stream 

			fsIn.Close();
		}
	}
}