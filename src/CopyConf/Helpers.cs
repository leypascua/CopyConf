using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CopyConf
{
    public static class Helpers
    {
        const string DistFileExtension = ".dist";

        public static void SyncFolders(DirectoryInfo sourcePath, DirectoryInfo destinationPath, string[] includedFileExtensions)
        {
            SyncFiles(sourcePath, destinationPath, includedFileExtensions);
            
            foreach (DirectoryInfo sourceFolder in sourcePath.EnumerateDirectories())
            {
                var destFolder = new DirectoryInfo(Path.Combine(destinationPath.FullName, sourceFolder.Name));
                if (!destFolder.Exists)
                {
                    destFolder.Create();
                }

                SyncFolders(sourceFolder, destFolder, includedFileExtensions);                
            }
        }

        public static void SyncFiles(DirectoryInfo sourcePath, DirectoryInfo destinationPath, string[] includedFileExtensions)
        {
            foreach (string extensionFilename in includedFileExtensions)
            {
                SyncFileExtension(sourcePath, destinationPath, "*" + extensionFilename);
                SyncFileExtension(sourcePath, destinationPath, "*" + extensionFilename + DistFileExtension);
            }

            // delete all .dist files on destination.
            foreach (FileInfo distFile in destinationPath.EnumerateFiles("*" + DistFileExtension))
            {
                distFile.Delete();
            }
        }

        public static void SyncFileExtension(DirectoryInfo sourcePath, DirectoryInfo destinationPath, string extensionFilename)
        {
            foreach (FileInfo sourceFile in sourcePath.EnumerateFiles(extensionFilename))
            {
                var destFile = new FileInfo(Path.Combine(destinationPath.FullName, sourceFile.Name));

                // if file doesn't exist on destination, copy.
                if (!destFile.Exists)
                {
                    sourceFile.CopyTo(destFile.FullName);
                }

                // if a non-dist file on source is newer than destination, copy.
                if (!sourceFile.Extension.EndsWith(DistFileExtension))
                {
                    if (sourceFile.LastWriteTimeUtc > destFile.LastWriteTimeUtc)
                    {
                        sourceFile.CopyTo(destFile.FullName, overwrite: true);
                    }
                }

                // if a .dist file, rename if the expected destination filename doesn't exist on dest path
                if (sourceFile.Extension.EndsWith(DistFileExtension))
                {
                    string destFileName = Regex.Replace(sourceFile.Name, DistFileExtension, string.Empty);
                    var expectedDestFile = new FileInfo(Path.Combine(destinationPath.FullName, destFileName));

                    if (!expectedDestFile.Exists)
                    {
                        sourceFile.CopyTo(expectedDestFile.FullName);
                    }
                }
            }
        }
    }
}
