using System;
using System.Configuration;
using System.IO;
using System.Linq;
using ERMPower.Domain.Parser;
using ERMPower.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Resolution;

namespace ERMPower.Tests
{
    [TestClass]
    public class FileProcessingServiceTest
    {
        
        private IUnityContainer container;
        private string filePath;

        [TestInitialize]
        public void FileProcessortTestInitialize()
        {
            this.filePath = ConfigurationManager.AppSettings.Get("MeterFilePath");
            container = UnityConfig.GetConfiguredContainer();
        }

        [TestMethod]
        public void TestGetMedian()
        {
            var fileName = Path.Combine(filePath, "LP_210095893_20150901T011608049.csv");
            var fileProcessor = container.Resolve<IFileProcessingService>
            (new ParameterOverride("parser", container.Resolve<IParser>("LP")),
                new ParameterOverride("filePath", fileName));

            fileProcessor.Import();
            var currentMedian = fileProcessor.GetMedian();
            Assert.IsNotNull(currentMedian);
        }

        [TestMethod]
        public void TestGetDataOverMedian()
        {
            var fileName = Path.Combine(filePath, "LP_210095893_20150901T011608049.csv");
            var fileProcessor = container.Resolve<IFileProcessingService>
            (new ParameterOverride("parser", container.Resolve<IParser>("LP")),
                new ParameterOverride("filePath", fileName));

            fileProcessor.Import();
            var listOverMedian = fileProcessor.GetDataOverMedian();
            Assert.IsTrue(listOverMedian.Any());

        }

        [TestMethod]
        public void TestGetDataBelowMedian()
        {
            var fileName = Path.Combine(filePath, "LP_210095893_20150901T011608049.csv");
            var fileProcessor = container.Resolve<IFileProcessingService>
            (new ParameterOverride("parser", container.Resolve<IParser>("LP")),
                new ParameterOverride("filePath", fileName));

            fileProcessor.Import();
            var listOverMedian = fileProcessor.GetDataBelowMedian();
            Assert.IsTrue(listOverMedian.Any());

        }
    }
}
