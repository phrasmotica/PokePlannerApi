using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokePlannerApi.Data.DataStore.Services;

namespace PokePlannerApi.Tests
{
    /// <summary>
    /// Tests for the <see cref="TypeData"/> singleton.
    /// </summary>
    public class TypeTests
    {
        /// <summary>
        /// The service provider.
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Setup hook.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            serviceProvider = TestHelper.BuildServiceProvider();
        }

        /// <summary>
        /// Verifies that type loading works correctly.
        /// </summary>
        [Test]
        [Category("Integration")]
        public async Task TypesLoadingTest()
        {
            // load type data
            var typesService = serviceProvider.GetService<TypeService>();
            var entries = await typesService.UpsertAll();

            // verify it's all loaded
            Assert.AreEqual(20, entries.ToArray().Length);
        }
    }
}
