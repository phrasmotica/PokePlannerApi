using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PokeApiNet;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.DataStore.Services;

namespace PokePlannerWeb.Tests.DataStore.Services
{
    /// <summary>
    /// Tests for the encounters service.
    /// </summary>
    public class EncountersServiceTests
    {
        /// <summary>
        /// Verifies that encounter details from multiple versions are grouped correctly.
        /// </summary>
        [Test]
        public async Task Grouping_MultipleVersions()
        {
            // arrange
            var service = SetupForGetEncounterDetails();

            var version1 = PokeApiHelpers.NamedResourceNavigation<Version>("version1", "url1");
            var maxChance1 = 1;
            var encounters1 = PokeApiHelpers.Encounters(2);
            var ved1 = new VersionEncounterDetail
            {
                Version = version1,
                MaxChance = maxChance1,
                EncounterDetails = encounters1.ToList()
            };

            var version2 = PokeApiHelpers.NamedResourceNavigation<Version>("version2", "url2");
            var maxChance2 = 2;
            var encounters2 = PokeApiHelpers.Encounters(2);
            var ved2 = new VersionEncounterDetail
            {
                Version = version2,
                MaxChance = maxChance2,
                EncounterDetails = encounters2.ToList()
            };

            var maxChance3 = 3;
            var encounters3 = PokeApiHelpers.Encounters(2);
            var ved3 = new VersionEncounterDetail
            {
                Version = version2,
                MaxChance = maxChance3,
                EncounterDetails = encounters3.ToList()
            };

            var versionEncounterDetails = new[] { ved1, ved2, ved3 };

            // act
            var entries = await service.GetEncounterDetails(versionEncounterDetails);
            var entryList = entries.ToList();

            // assert
            Assert.That(entryList.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Verifies that encounter details from one version with multiple methods are grouped correctly.
        /// </summary>
        [Test]
        public async Task Grouping_OneVersion_MultipleMethods()
        {
            // arrange
            var service = SetupForGetEncounterDetails();

            var version = PokeApiHelpers.NamedResourceNavigation<Version>();
            var conditionValues = PokeApiHelpers.ConditionValues(0).ToList();

            var maxChance1 = 1;
            var method1 = PokeApiHelpers.NamedResourceNavigation<EncounterMethod>("method1", "url1");
            var encounters1 = PokeApiHelpers.Encounters(2, conditionValues, method1);
            var ved1 = new VersionEncounterDetail
            {
                Version = version,
                MaxChance = maxChance1,
                EncounterDetails = encounters1.ToList()
            };

            var maxChance2 = 2;
            var method2 = PokeApiHelpers.NamedResourceNavigation<EncounterMethod>("method2", "url2");
            var encounters2 = PokeApiHelpers.Encounters(3, conditionValues, method2);
            var ved2 = new VersionEncounterDetail
            {
                Version = version,
                MaxChance = maxChance2,
                EncounterDetails = encounters2.ToList()
            };

            var versionEncounterDetails = new[] { ved1, ved2 };

            // act
            var entries = await service.GetEncounterDetails(versionEncounterDetails);
            var entryList = entries.ToList();

            // assert
            var methodData = entryList[0].Data;
            Assert.That(methodData.Length, Is.EqualTo(2));
        }

        /// <summary>
        /// Verifies that encounter details from multiple versions with multiple methods are grouped correctly.
        /// </summary>
        [Test]
        public async Task Grouping_MultipleVersions_MultipleMethods()
        {
            // arrange
            var service = SetupForGetEncounterDetails();

            var conditionValues = PokeApiHelpers.ConditionValues(0).ToList();

            var version1 = PokeApiHelpers.NamedResourceNavigation<Version>("version1", "url1");
            var maxChance1 = 1;
            var method1 = PokeApiHelpers.NamedResourceNavigation<EncounterMethod>("method1", "url1");
            var encounters1 = PokeApiHelpers.Encounters(2, conditionValues, method1);
            var ved1 = new VersionEncounterDetail
            {
                Version = version1,
                MaxChance = maxChance1,
                EncounterDetails = encounters1.ToList()
            };

            var maxChance2 = 2;
            var method2 = PokeApiHelpers.NamedResourceNavigation<EncounterMethod>("method2", "url2");
            var encounters2 = PokeApiHelpers.Encounters(3, conditionValues, method2);
            var ved2 = new VersionEncounterDetail
            {
                Version = version1,
                MaxChance = maxChance2,
                EncounterDetails = encounters2.ToList()
            };

            var version2 = PokeApiHelpers.NamedResourceNavigation<Version>("version2", "url2");
            var ved3 = new VersionEncounterDetail
            {
                Version = version2,
                MaxChance = maxChance1,
                EncounterDetails = encounters1.ToList()
            };

            var ved4 = new VersionEncounterDetail
            {
                Version = version2,
                MaxChance = maxChance2,
                EncounterDetails = encounters2.ToList()
            };

            var versionEncounterDetails = new[] { ved1, ved2, ved3, ved4 };

            // act
            var entries = await service.GetEncounterDetails(versionEncounterDetails);
            var entryList = entries.ToList();

            // assert
            Assert.That(entryList.Count, Is.EqualTo(2));

            foreach (var entry in entryList)
            {
                Assert.That(entry.Data.Length, Is.EqualTo(2));
            }
        }

        /// <summary>
        /// Verifies that encounter details from one version with the same method and different
        /// condition value sets are grouped correctly.
        /// </summary>
        [Test]
        public async Task Grouping_OneVersion_MultipleEncounters_SameMethod_DifferentConditions()
        {
            // arrange
            var service = SetupForGetEncounterDetails();

            var method = PokeApiHelpers.NamedResourceNavigation<EncounterMethod>("method1", "url1");

            var conditionValues1 = PokeApiHelpers.ConditionValues(1).ToList();
            var encounter1 = PokeApiHelpers.Encounter(conditionValues: conditionValues1, method: method);
            
            var conditionValues2 = PokeApiHelpers.ConditionValues(2).ToList();
            var encounter2 = PokeApiHelpers.Encounter(conditionValues: conditionValues2, method: method);

            var ved1 = new VersionEncounterDetail
            {
                Version = PokeApiHelpers.NamedResourceNavigation<Version>(),
                MaxChance = 1,
                EncounterDetails = new[] { encounter1, encounter2 }.ToList()
            };

            var versionEncounterDetails = new[] { ved1 };

            // act
            var entries = await service.GetEncounterDetails(versionEncounterDetails);
            var entryList = entries.ToList();

            // assert

            // should be only one version
            Assert.That(entryList.Count, Is.EqualTo(1));
            var entry = entryList[0];

            // should only be one method
            Assert.That(entry.Data.Length, Is.EqualTo(1));
            var methodDetail = entry.Data[0];

            // should be two condition values details
            Assert.That(methodDetail.ConditionValuesDetails.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Returns an EncountersService instance for testing
        /// <see cref="EncountersService.GetEncounterDetails(IEnumerable{VersionEncounterDetail})"/>.
        /// </summary>
        private static EncountersService SetupForGetEncounterDetails()
        {
            var encounterConditionValueService = new Mock<EncounterConditionValueService>(null, null, null, null);
            encounterConditionValueService
                .Setup(
                    s => s.UpsertMany(
                        It.IsAny<IEnumerable<NamedApiResource<EncounterConditionValue>>>()
                    )
                )
                .Returns<IEnumerable<NamedApiResource<EncounterConditionValue>>>(
                    navs => Task.FromResult(
                        navs.Select(nav =>
                            new EncounterConditionValueEntry
                            {
                                Name = nav.Name
                            }
                        )
                    )
                );

            var encounterMethodService = new Mock<EncounterMethodService>(null, null, null, null);
            encounterMethodService
                .Setup(
                    s => s.Upsert(
                        It.IsAny<NamedApiResource<EncounterMethod>>()
                    )
                )
                .Returns<NamedApiResource<EncounterMethod>>(
                    m => Task.FromResult(
                        new EncounterMethodEntry
                        {
                            Name = m.Name
                        }
                    )
                );

            var versionService = new Mock<VersionService>(null, null, null, null);
            versionService
                .Setup(
                    s => s.Upsert(
                        It.IsAny<NamedApiResource<Version>>()
                    )
                )
                .Returns<NamedApiResource<Version>>(
                    m => Task.FromResult(
                        new VersionEntry
                        {
                            Name = m.Name
                        }
                    )
                );

            return new EncountersService(
                null,
                null,
                null,
                encounterConditionValueService.Object,
                encounterMethodService.Object,
                null,
                null,
                versionService.Object,
                null,
                null
            );
        }
    }
}
