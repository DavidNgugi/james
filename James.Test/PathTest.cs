﻿using System.Collections.Generic;
using James.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace James.Test
{
    [TestClass]
    public class PathTest
    {

        private Path CreateBasicPath()
        {
            var fileExtensions = new List<FileExtension>
            {
                new FileExtension("txt", 20),
                new FileExtension("exe", 40),
                new FileExtension("pdf", 30),
                new FileExtension("md5", 20)
            };
            return new Path() { Location = @"C:\Users\Moser", Priority = 20, IsDefaultConfigurationEnabled = true, FileExtensions = fileExtensions};
        }

        [TestMethod]
        public void BasicCalcPriorityTest()
        {
            var path = CreateBasicPath();
            Assert.IsTrue(path.GetPathPriority(@"C:\Users\Moser\asdf.txt") == 40, "should be 40");
            Assert.IsTrue(path.GetPathPriority(@"C:\Users\Moser\asdf.exe") == 60, "should be 60");
            Assert.IsTrue(path.GetPathPriority(@"C:\Users\Moser\asdf.asdf") == -1, "should be -1");
        }

        [TestMethod]
        public void FallbackDefaultPriorityTest()
        {
            var path = CreateBasicPath();
            Config.Instance.DefaultFileExtensions.Add(new FileExtension("testEnding", 10));
            Assert.IsTrue(path.GetPathPriority(@"C:\Users\Moser\asdf.txt") == 40, "should be 40");
            Assert.IsTrue(path.GetPathPriority(@"C:\Users\Moser\asdf.exe") == 60, "should be 60");
            Assert.IsTrue(path.GetPathPriority(@"C:\Users\Moser\asdf.testEnding") == 30, "should be -1");
        }
    }
}
