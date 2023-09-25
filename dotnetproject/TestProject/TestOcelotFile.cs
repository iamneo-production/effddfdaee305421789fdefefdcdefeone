using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Ocelot.Tests
{
    [TestFixture]
    public class OcelotJsonTests
    {
        private JObject ocelotJson;

        [SetUp]
        public void Setup()
        {
            // Load the Ocelot.json file for testing (replace with your actual file path)
            string jsonFilePath = @"/home/coder/project/workspace/dotnetproject/dotnetapigateway/Ocelot.json";
            string jsonText = System.IO.File.ReadAllText(jsonFilePath);
            ocelotJson = JObject.Parse(jsonText);
        }

       [Test]
        public void VerifyCallRoute()
        {
            var CallRoute = ocelotJson["Routes"][0];

            Assert.That(CallRoute, Is.Not.Null);
            Assert.That(CallRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Call"));
            Assert.That(CallRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(CallRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(CallRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8080));
        }

        [Test]
        public void VerifyCallIDRoute()
        {
            var ComplaintRoute = ocelotJson["Routes"][1];

            Assert.That(ComplaintRoute, Is.Not.Null);
            Assert.That(ComplaintRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Call/{id}"));
            Assert.That(ComplaintRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(ComplaintRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(ComplaintRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8080));
        }
[Test]
        public void VerifyComplaintRoute()
        {
            var ComplaintRoute = ocelotJson["Routes"][2];

            Assert.That(ComplaintRoute, Is.Not.Null);
            Assert.That(ComplaintRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Complaint"));
            Assert.That(ComplaintRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(ComplaintRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(ComplaintRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8081));
        }

        [Test]
        public void VerifyComplaintIDRoute()
        {
            var ComplaintRoute = ocelotJson["Routes"][3];

            Assert.That(ComplaintRoute, Is.Not.Null);
            Assert.That(ComplaintRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Complaint/{id}"));
            Assert.That(ComplaintRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(ComplaintRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(ComplaintRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8081));
        }
        [Test]
        public void VerifyCallRouteUpstreamPath()
        {
            var CallRoute = ocelotJson["Routes"][0];

            Assert.That(CallRoute, Is.Not.Null);
            Assert.That(CallRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Call"));
        }
        [Test]
        public void VerifyCallIDRouteUpstreamPath()
        {
            var CallRoute = ocelotJson["Routes"][1];

            Assert.That(CallRoute, Is.Not.Null);
            Assert.That(CallRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Call/{id}"));
        }
        [Test]
        public void VerifyComplaintRouteUpstreamPath()
        {
            var CallRoute = ocelotJson["Routes"][2];

            Assert.That(CallRoute, Is.Not.Null);
            Assert.That(CallRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Complaint"));
        }
        [Test]
        public void VerifyComplaintNamesRouteUpstreamPath()
        {
            var CallRoute = ocelotJson["Routes"][3];

            Assert.That(CallRoute, Is.Not.Null);
            Assert.That(CallRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Complaint/{id}"));
        }

        [Test]
        public void VerifyCallRouteHttpMethods()
        {
            var CallRoute = ocelotJson["Routes"][0];

            Assert.That(CallRoute, Is.Not.Null);
            Assert.That(CallRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "POST", "GET" }));
        }
        [Test]
        public void VerifyCallIDRouteHttpMethods()
        {
            var CallRoute = ocelotJson["Routes"][1];

            Assert.That(CallRoute, Is.Not.Null);
            Assert.That(CallRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "DELETE", "GET" }));
        }
        [Test]
        public void VerifyComplaintRouteHttpMethods()
        {
            var ComplaintRoute = ocelotJson["Routes"][2];

            Assert.That(ComplaintRoute, Is.Not.Null);
            Assert.That(ComplaintRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "POST", "GET" }));
        }

        [Test]
        public void VerifyComplaintDeleteIDRouteHttpMethods()
        {
            var ComplaintRoute = ocelotJson["Routes"][3];

            Assert.That(ComplaintRoute, Is.Not.Null);
            Assert.That(ComplaintRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "DELETE" }));
        }
    }
}
