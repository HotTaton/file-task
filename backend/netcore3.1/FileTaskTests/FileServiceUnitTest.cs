using FileTaskApiCore.DataContract;
using FileTaskApiCore.Services;
using FileTaskApiCore.Settings;
using FileTaskTests.FakeImplementations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FileTaskTests
{

    public class FileServiceUnitTest
    {

        private void MakeTreeFlat(FileViewModel element, List<FileViewModel> flatTree)
        {
            if (element != null)
            {
                foreach (var sibling in element.ChildNodes)
                {
                    MakeTreeFlat(sibling, flatTree);
                }

                flatTree.Add(element);
            }
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetFileTreeData), MemberType = typeof(TestDataGenerator))]
        public async Task GetFileTree(string filePath, FakeOptions<FileServiceSettings> settings, ICollection<string> files, FileViewModel expected)
        {
            using (var pathCreator = new PathCreator(files))
            {
                pathCreator.CreateFiles();
                var fileService = new FileService(new FakeLogger<FileService>(), settings);
                var response = await fileService.GetFileTree(pathCreator.GetFullPath(filePath));
                var flatTreeResult = new List<FileViewModel>();
                var flatTreeExpected = new List<FileViewModel>();

                MakeTreeFlat(response.Item, flatTreeResult);
                MakeTreeFlat(expected, flatTreeExpected);

                var comparer = new FileViewModelComparer();
                foreach (var expectedItem in flatTreeExpected)
                {
                    Assert.Contains(expectedItem, flatTreeResult, comparer);
                }
            }
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetFileData), MemberType = typeof(TestDataGenerator))]
        public async Task ReadFile(string filePath, FakeOptions<FileServiceSettings> settings, IEnumerable<IEnumerable<string>> expected)
        {
            var fileContent = expected.Select(x => string.Join(settings.Value.FileDelimeter, x)).ToArray();
            using (var pathCreator = new PathCreator(new[] { filePath }, fileContent))
            {
                pathCreator.CreateFiles();
                var fileService = new FileService(new FakeLogger<FileService>(), settings);
                var response = await fileService.ReadFileData(pathCreator.GetFullPath(filePath));
                var content = response.Item.Content;

                var castedExpected = expected.Select(item => item.ToArray()).ToArray();
                var castedContent = content.Select(item => item.ToArray()).ToArray();

                Assert.Equal(castedContent.Length, castedExpected.Length);

                for (int i = 0; i < castedExpected.Length; ++i)
                {
                    Assert.Equal(castedExpected[i].Length, castedContent[i].Length);

                    for (int j = 0; j < castedExpected[i].Length; ++j)
                    {
                        Assert.Equal(castedExpected[i][j], castedContent[i][j]);
                    }
                }
            }
        }
    }
}
