using FileTaskApiCore.DataContract;
using FileTaskApiCore.Settings;
using FileTaskTests.FakeImplementations;
using System.Collections.Generic;

namespace FileTaskTests
{
    public class TestDataGenerator
    {      

        public static IEnumerable<object[]> GetFileTreeData()
        {
            var setting = new FakeOptions<FileServiceSettings>(new FileServiceSettings
            {
                FileDelimeter = "\t",
                InitialPath = string.Empty,
                MaxLoadDepth = int.MaxValue
            });

            return new List<object[]>
            {

                new object[]
                {
                    $"testdir",
                    setting,
                    new List<string>
                    {
                        $"testdir",
                        $"testdir\\testdir1_level1",
                        $"testdir\\testdir1_level1\\testdir1_level2",
                        $"testdir\\testdir1_level1\\testfile1_level2.txt",
                        $"testdir\\testfile1_level1.txt"
                    },
                    new FileViewModel
                    {
                        Name = $"testdir",
                        IsDirectory = true,
                        IsExpandable = true
                    }
                    .AddChild(new FileViewModel
                    {
                        Name = $"testdir\\testdir1_level1",
                        IsDirectory = true,
                        IsExpandable = true
                    }
                        .AddChild(new FileViewModel { Name = $"testdir\\testdir1_level1\\testdir1_level2", IsDirectory = true })
                        .AddChild(new FileViewModel { Name = $"testdir\\testdir1_level1\\testfile1_level2.txt"}))
                    .AddChild(new FileViewModel
                    {
                        Name = $"testdir\\testfile1_level1.txt"
                    })
                },
                new object[]
                {
                    $"testdir2\\testdir1_level1",
                    setting,
                    new List<string>
                    {
                        $"testdir2",
                        $"testdir2\\testdir1_level1",
                        $"testdir2\\testdir1_level1\\testdir1_level2",
                        $"testdir2\\testdir1_level1\\testfile1_level2.txt",
                        $"testdir2\\testfile1_level1.txt"
                    },
                    new FileViewModel
                    {
                        Name = $"testdir2\\testdir1_level1",
                        IsDirectory = true,
                        IsExpandable = true
                    }
                    .AddChild(new FileViewModel { Name = $"testdir2\\testdir1_level1\\testdir1_level2", IsDirectory = true })
                    .AddChild(new FileViewModel { Name = $"testdir2\\testdir1_level1\\testfile1_level2.txt"})
                },
            };
        }

        public static IEnumerable<object[]> GetFileData()
        {
            var setting = new FakeOptions<FileServiceSettings>(new FileServiceSettings
            {
                FileDelimeter = "\t",
                InitialPath = string.Empty,
                MaxLoadDepth = int.MaxValue
            });

            return new List<object[]>
            {
                new object[]
                {
                    $"test.txt",
                    setting,
                    new string [][]
                    {
                        new string[] { "column1", "column2", "column3" },
                        new string[] { "item1", "item2", "item3" }
                    }
                }
            };
        }
    }
}
