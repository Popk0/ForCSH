using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AService
{
    class Operations
    {
		internal static string targetDirectoryPath;
        internal static string sourceDirectoryPath;
        internal static bool IsForEncrypt;
		internal static bool IsForArchive;
        internal static void OnFileUpdated(object sender, FileSystemEventArgs e) => FileUpdated(e.Name);
        private static void FileUpdated(string name)
		{
            try
			{
				Service1.logger.watcher.EnableRaisingEvents = false;
				if (IsForArchive)				
					Zip(sourceDirectoryPath, sourceDirectoryPath, name);
				
				Service1.logger.watcher.EnableRaisingEvents = true;

				if (IsForEncrypt)				
					Encrypt(sourceDirectoryPath, name + ".gz");
				
				Move(sourceDirectoryPath, targetDirectoryPath, name + ".gz");

                if (IsForEncrypt)
                    Decrypt(targetDirectoryPath, name + ".gz");

                if (IsForArchive)
                    Unzip(targetDirectoryPath, targetDirectoryPath, name + ".gz");

				Delete(targetDirectoryPath, name + ".gz");
			}
			catch (Exception ex)
			{
				using (StreamWriter writer = new StreamWriter(Path.Combine(targetDirectoryPath, "textlog.txt"), true))
				{
					writer.WriteLine("\n================\n" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss ") + ex.Message + "\n================\n");
					writer.Flush();
				}
			}
		}
		static void Unzip(string sourceDirectoryPath, string targetDirectoryPath, string name)
		{
			var buf = name.Substring(0, name.Length - 3);

			using (var targetStream = File.Create(Path.Combine(targetDirectoryPath, buf)))
			using (var sourceStream = File.OpenRead(Path.Combine(sourceDirectoryPath, name)))
			using (var decomressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
			{
				decomressionStream.CopyTo(targetStream);
			}
		}
		private static void Move(string SourcePath, string TargetPath, string name)
        {
			if (File.Exists(Path.Combine(TargetPath, name)))
			{
				Delete(TargetPath, name);
			}

			File.Move(Path.Combine(SourcePath, name), Path.Combine(TargetPath, name));
		}
		static private void Delete(string filePath, string name)
		{
			var buf = Path.Combine(filePath, name);

			File.Delete(buf);
		}
		private static void Zip(string SourcePath, string TargetPath, string name)
		{
			var buf = new StringBuilder(name);
			buf.Append(".gz");

			using (var targetStream = File.Create(Path.Combine(TargetPath, buf.ToString())))
			using (var sourceStream = new FileStream(Path.Combine(SourcePath, name), FileMode.OpenOrCreate, FileAccess.Read))
			using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
			{
				sourceStream.CopyTo(compressionStream);
			}
		}
        private static void Encrypt(string filePath, string name) => File.Encrypt(Path.Combine(filePath, name));
        private static void Decrypt(string filePath, string name) => File.Decrypt(Path.Combine(filePath, name));
    }
}