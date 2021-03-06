using System;
using System.Collections.Generic;
using Sentry.Internal;
using Sentry.Protocol;
using Xunit;

// ReSharper disable once CheckNamespace
namespace Sentry.Tests.Protocol
{
    public class AppTests
    {
        [Fact]
        public void SerializeObject_AllPropertiesSetToNonDefault_SerializesValidObject()
        {
            var sut = new App
            {
                Version = "8b03fd7",
                Build = "1.23152",
                BuildType = "nightly",
                Hash = "93fd0e9a",
                Name = "Sentry.Test.App",
                StartTime = DateTimeOffset.MaxValue
            };

            var actual = JsonSerializer.SerializeObject(sut);

            Assert.Equal("{\"type\":\"app\","
                        + "\"app_start_time\":\"9999-12-31T23:59:59.9999999+00:00\","
                        + "\"device_app_hash\":\"93fd0e9a\","
                        + "\"build_type\":\"nightly\","
                        + "\"app_name\":\"Sentry.Test.App\","
                        + "\"app_version\":\"8b03fd7\","
                        + "\"app_build\":\"1.23152\"}",
                    actual);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void SerializeObject_TestCase_SerializesAsExpected((App app, string serialized) @case)
        {
            var actual = JsonSerializer.SerializeObject(@case.app);

            Assert.Equal(@case.serialized, actual);
        }

        public static IEnumerable<object[]> TestCases()
        {
            yield return new object[] { (new App(), "{\"type\":\"app\"}") };
            yield return new object[] { (new App { Name = "some name" }, "{\"type\":\"app\",\"app_name\":\"some name\"}") };
            yield return new object[] { (new App { Build = "some build" }, "{\"type\":\"app\",\"app_build\":\"some build\"}") };
            yield return new object[] { (new App { BuildType = "some build type" }, "{\"type\":\"app\",\"build_type\":\"some build type\"}") };
            yield return new object[] { (new App { Hash = "some hash" }, "{\"type\":\"app\",\"device_app_hash\":\"some hash\"}") };
            yield return new object[] { (new App { StartTime = DateTimeOffset.MaxValue }, "{\"type\":\"app\",\"app_start_time\":\"9999-12-31T23:59:59.9999999+00:00\"}") };
            yield return new object[] { (new App { Version = "some version" }, "{\"type\":\"app\",\"app_version\":\"some version\"}") };
            yield return new object[] { (new App { Identifier = "some identifier" }, "{\"type\":\"app\",\"app_identifier\":\"some identifier\"}") };
        }
    }
}
