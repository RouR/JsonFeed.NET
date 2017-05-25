﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonFeedNet
{

    public class JsonFeed
    {
        /// <summary>
        /// The URL of the version of the format the feed uses.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; } = @"https://jsonfeed.org/version/1"; //required

        /// <summary>
        /// The name of the feed.
        /// This will often correspond to the name of the website(blog, for instance).
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; } //required

        /// <summary>
        /// The URL of the resource that the feed describes.
        /// This resource may or may not actually be a “home” page, but it should be an HTML page.
        /// </summary>
        [JsonProperty("home_page_url")]
        public string HomePageUrl { get; set; } //optional

        /// <summary>
        /// The URL of the feed.
        /// Serves as the unique identifier for the feed.
        /// </summary>
        [JsonProperty("feed_url")]
        public string FeedUrl { get; set; } //optional

        /// <summary>
        /// More detail, beyond the title, on what the feed is about.
        /// A feed reader may display this text.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } //optional

        /// <summary>
        /// Description of the purpose of the feed.
        /// This is for the use of people looking at the raw JSON, and should be ignored by feed readers.
        /// </summary>
        [JsonProperty("user_comment")]
        public string UserComment { get; set; } //optional

        /// <summary>
        /// The URL of a feed that provides the next n items, where n is determined by the publisher.
        /// This allows for pagination, but with the expectation that reader software is not required to use it and probably won’t use it very often.
        /// Must not be the same as FeedUrl, and it must not be the same as a previous NextUrl (to avoid infinite loops).
        /// </summary>
        [JsonProperty("next_url")]
        public string NextUrl { get; set; } //optional

        /// <summary>
        /// The URL of an image for the feed suitable to be used in a timeline, much the way an avatar might be used.
        /// It should be square and relatively large — such as 512 x 512 — so that it can be scaled-down.
        /// It should use transparency where appropriate, since it may be rendered on a non-white background.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; } //optional

        /// <summary>
        /// The URL of an image for the feed suitable to be used in a source list.
        /// It should be square and relatively small, but not smaller than 64 x 64.
        /// It should use transparency where appropriate, since it may be rendered on a non-white background.
        /// </summary>
        [JsonProperty("favicon")]
        public string FavIcon { get; set; } //optional

        /// <summary>
        /// The feed author.
        /// </summary>
        [JsonProperty("author")]
        public Author Author { get; set; } //optional

        /// <summary>
        /// Indicates whether or not the feed is finished — that is, whether or not it will ever update again.
        /// If the value is true, then it’s expired. Any other value, or the absence of expired, means the feed may continue to update.
        /// </summary>
        [JsonProperty("expired")]
        public bool? Expired { get; set; } //optional

        /// <summary>
        /// Endpoints that can be used to subscribe to real-time notifications of changes to this feed.
        /// </summary>
        [JsonProperty("hubs")]
        public List<Hub> Hubs { get; set; } //optional

        /// <summary>
        /// The individual items in the feed.
        /// </summary>
        [JsonProperty("items")]
        public List<FeedItem> Items { get; set; } //required

        public static JsonFeed Parse(string jsonFeedString)
        {
            JsonFeed jsonFeed = JsonConvert.DeserializeObject<JsonFeed>(jsonFeedString);
            return jsonFeed;
        }

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        public static async Task<JsonFeed> ParseFromUriAsync(Uri jsonFeedUri, HttpMessageHandler httpMessageHandler = null)
        {
            var client = new HttpClient(httpMessageHandler ?? new HttpClientHandler());
            string jsonDocument = await client.GetStringAsync(jsonFeedUri);

            return Parse(jsonDocument);
        }

        public string Serialize()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            string jsonString = JsonConvert.SerializeObject(this, _serializerSettings);
            return jsonString;
        }
    }
}
