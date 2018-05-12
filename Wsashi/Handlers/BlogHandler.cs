using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsashi.Features.Blogs;
using Discord;
using Newtonsoft.Json;

namespace Wsashi.Handlers
{
    class BlogHandler
    {
        private static readonly string blogFile = "blogs.json";
        public static Embed SubscribeToBlog(ulong userId, string blogname)
        {
            var blogs = Configuration.DataStorage.RestoreObject<List<BlogItem>>(blogFile);

            var blog = blogs.FirstOrDefault(k => k.Name == blogname);

            if (blog != null)
            {
                if (!blog.Subscribers.Contains(userId))
                {
                    blog.Subscribers.Add(userId);

                    Configuration.DataStorage.StoreObject(blogs, blogFile, Formatting.Indented);

                    return EmbedHandler.CreateEmbed("Blog", "You now follow this blog", EmbedHandler.EmbedMessageType.Success);
                }
                else
                {
                    return EmbedHandler.CreateEmbed("Blog :x:", "You already follow this blog", EmbedHandler.EmbedMessageType.Info);
                }
            }
            else
            {
                return EmbedHandler.CreateEmbed("Blog :x:", $"There is no Blog with the name {blogname}", EmbedHandler.EmbedMessageType.Error);
            }
        }

        public static Embed UnSubscribeToBlog(ulong userId, string blogname)
        {
            var blogs = Configuration.DataStorage.RestoreObject<List<BlogItem>>(blogFile);

            var blog = blogs.FirstOrDefault(k => k.Name == blogname);

            if (blog != null)
            {
                if (blog.Subscribers.Contains(userId))
                {
                    blog.Subscribers.Remove(userId);

                    Configuration.DataStorage.StoreObject(blogs, blogFile, Formatting.Indented);

                    return EmbedHandler.CreateEmbed("Blog", "You stopped following this blog", EmbedHandler.EmbedMessageType.Success);
                }
                else
                {
                    return EmbedHandler.CreateEmbed("Blog :x:", "You don't follow this blog", EmbedHandler.EmbedMessageType.Info);
                }
            }
            else
            {
                return EmbedHandler.CreateEmbed("Blog :x:", $"There is no Blog with the name {blogname}", EmbedHandler.EmbedMessageType.Error);
            }
        }
    }
}
