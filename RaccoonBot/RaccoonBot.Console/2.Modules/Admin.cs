﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Constants;
using RaccoonBot.Domain.Utils;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RaccoonBot.Modules
{
    public class Admin : ModuleBase<SocketCommandContext>
    {
        private Settings _settings;

        [Command(Commands.Copy), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.Copy)]
        public async Task CopyMesssageChat(SocketGuildChannel copyChannel, SocketGuildChannel pasteChannel, string quantityMsg, string mentionUsers = "false")
        {
            try
            {
                var msgs = await (copyChannel as ISocketMessageChannel).GetMessagesAsync(Int32.Parse(quantityMsg) + 1).FlattenAsync();
                var mentionUser = bool.Parse(mentionUsers);
                foreach (var msg in msgs.OrderBy(x => x.CreatedAt.DateTime))
                {
                    try
                    {
                        var paste = (pasteChannel as ISocketMessageChannel);
                        var mention = mentionUser ? $"{msg.Author.Mention} disse :  \n" : string.Empty;
                        await paste.SendMessageAsync(mention + msg.Content);

                        foreach (var att in msg.Attachments)
                        {
                            var webClient = new WebClient();
                            byte[] imageBytes = webClient.DownloadData(att.Url);
                            Stream stream = new MemoryStream(imageBytes);
                            await paste.SendFileAsync(stream, att.Filename);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Command(Commands.RemoveRolePermissionToCategory), RequireUserPermission(GuildPermission.Administrator)]
        [Summary("")]
        public async Task RemoveRolePermissionToCategory(string category, [Remainder] string roles)
        {
            var rolesMention = roles.Split(' ');
            var categoryChannel = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Name.ToUpper() == category.ToUpper());
            var everyone = Context.Guild.Roles.FirstOrDefault(x => x.IsEveryone);

            foreach (var roleMention in rolesMention)
            {
                var role = Context.Guild.GetRole(DiscordHelper.UnMentionRole(roleMention));

                await categoryChannel.AddPermissionOverwriteAsync(role, new OverwritePermissions(
                               connect: PermValue.Deny,
                               viewChannel: PermValue.Deny,
                               manageChannel: PermValue.Deny,
                               manageMessages: PermValue.Deny,
                               muteMembers: PermValue.Deny,
                               moveMembers: PermValue.Deny,
                               manageRoles: PermValue.Deny,
                               deafenMembers: PermValue.Deny
                            ));
            }
            await Context.Channel.SendMessageAsync("Feito");
        }

        [Command(Commands.SetRoleAll), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.SetRoleAll)]
        public async Task SetRoleToAll(string nameRole)
        {
            var role = Context.Guild.GetRole(DiscordHelper.UnMentionRole(nameRole));
            var usersWithoutRole = Context.Guild.Users.Where(x => x.Roles.Any(y => !(y == role)) && !x.IsBot).ToList();
            await Context.Channel.SendMessageAsync(usersWithoutRole.Count().ToString());

            if (usersWithoutRole.Count() > 0)
            {
                foreach (var user in usersWithoutRole)
                {
                    try
                    {
                        await (user as IGuildUser).AddRoleAsync(role);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                await Context.Channel.SendMessageAsync(string.Format(Messages.AddRoleMessage, role.Name));
            }
        }

        [Command(Commands.DelRoleAll), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.DelRoleAll)]
        public async Task RemoveRoleToAll(string nameRole, string num = "5")
        {
            var role = Context.Guild.Roles.Where(x => x.Name.Contains(nameRole)).FirstOrDefault();
            var usersWithRole = Context.Guild.Users.Where(x => x.Roles.Any(y => y == role) && !x.IsBot).ToList();

            if (usersWithRole.Count() > 0)
            {
                foreach (var user in usersWithRole)
                {
                    try
                    {
                        await (user as IGuildUser).RemoveRoleAsync(role);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                await Context.Channel.SendMessageAsync(string.Format(Messages.RemoveRoleMessage, role.Name));
            }
        }

        [Command(Commands.AddRolePermissionModeratorToCategory), RequireUserPermission(GuildPermission.Administrator)]
        [Summary("")]
        public async Task AddRolePermissionModeratorToCategory(string category, [Remainder] string roles)
        {
            var rolesMention = roles.Split(' ');
            var categoryChannel = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Name.Contains(category.ToUpper()));
            var everyone = Context.Guild.Roles.FirstOrDefault(x => x.IsEveryone);

            foreach (var roleMention in rolesMention)
            {
                var role = Context.Guild.GetRole(DiscordHelper.UnMentionRole(roleMention));

                await categoryChannel.AddPermissionOverwriteAsync(everyone, new OverwritePermissions
                     (connect: PermValue.Deny,
                      viewChannel: PermValue.Deny,
                      readMessageHistory: PermValue.Deny,
                      sendMessages: PermValue.Deny,
                      speak: PermValue.Deny
                      ));

                await categoryChannel.AddPermissionOverwriteAsync(role, new OverwritePermissions(
                           connect: PermValue.Allow,
                           viewChannel: PermValue.Allow,
                           manageChannel: PermValue.Allow,
                           manageMessages: PermValue.Allow,
                           muteMembers: PermValue.Allow,
                           moveMembers: PermValue.Allow,
                           manageRoles: PermValue.Allow,
                           deafenMembers: PermValue.Allow
                        ));
            }
            await Context.Channel.SendMessageAsync("Feito");
        }

        [Command(Commands.AddRolePermissionToCategory), RequireUserPermission(GuildPermission.Administrator)]
        [Summary("")]
        public async Task AddRolePermissionToCategory(string category, [Remainder] string roles)
        {
            var rolesMention = roles.Split(' ');
            var categoryChannel = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Name.Contains(category.ToUpper()));

            foreach (var roleMention in rolesMention)
            {
                var role = Context.Guild.GetRole(DiscordHelper.UnMentionRole(roleMention));

                await categoryChannel.AddPermissionOverwriteAsync(role, new OverwritePermissions
                           (connect: PermValue.Allow,
                              viewChannel: PermValue.Allow,
                              readMessageHistory: PermValue.Allow,
                              sendMessages: PermValue.Allow,
                              speak: PermValue.Allow
                        ));
            }
            await Context.Channel.SendMessageAsync("Feito");
        }

        [Command(Commands.RemoveRolePermissionAllChats), RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Remove todas as permissões de uma role de todos os canais")]
        public async Task RemoveRolePermissionAllChats([Remainder] string roles)
        {
            var rolesMention = roles.Split(' ');
            var allChannels = Context.Guild.Channels;
            foreach (var roleMention in rolesMention)
            {
                var role = Context.Guild.GetRole(DiscordHelper.UnMentionRole(roleMention));
                foreach (var channel in allChannels)
                {
                    await channel.AddPermissionOverwriteAsync(role, new OverwritePermissions(
                            viewChannel: PermValue.Inherit,
                            connect: PermValue.Inherit,
                            manageChannel: PermValue.Deny,
                            manageMessages: PermValue.Deny,
                            muteMembers: PermValue.Deny,
                            moveMembers: PermValue.Deny,
                            manageRoles: PermValue.Deny,
                            deafenMembers: PermValue.Deny
                         ));
                }
            }
            await Context.Channel.SendMessageAsync("Feito");
        }

        //[Command(Commands.RemoveCategory), RequireUserPermission(GuildPermission.Administrator)]
        //[Summary(Summary.RemoveCategory)]
        //public async Task RemoveCategory([Remainder] string categories)
        //{
        //    try
        //    {
        //        var guild = Context.Guild;
        //        var listCategories = categories.Split(',');

        //        foreach (var category in listCategories)
        //        {
        //            var cat = guild.CategoryChannels.FirstOrDefault(x => x.Name.ToUpper() == category.ToUpper());

        //            foreach (var room in cat.Channels)
        //                await room.DeleteAsync();

        //            await cat.DeleteAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //[Command(Commands.PurgeRoles), RequireUserPermission(GuildPermission.Administrator)]
        //[Summary(Summary.PurgeRoles)]
        //public async Task PurgeRoles()
        //{
        //    await Task.Run(() =>
        //    {
        //        var guild = Context.Guild;
        //        var rolesByCountUsers = guild.Roles.Where(x =>
        //            x.Name.ToLower() != CustomRoles.RaccoonsRole.ToLower() &&
        //            x.Name.ToLower() != CustomRoles.BotRole.ToLower() &&
        //            x.Name.ToLower() != CustomRoles.LeaderRole.ToLower() &&
        //            !x.IsEveryone &&
        //            !_settings.NotPurgeRoles.ToUpper().Contains(x.Name.ToUpper()) &&
        //            x.Members.Count() < _settings.PurgeCount);
        //        var count = rolesByCountUsers.Count();
        //        var catchMinus = 0;
        //        foreach (var role in rolesByCountUsers)
        //        {
        //            try
        //            {
        //                role.DeleteAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                catchMinus++;
        //            }
        //        }
        //        count -= catchMinus;
        //        Context.Channel.SendMessageAsync($"{count} Roles limpadas com {_settings.PurgeCount} ou menos pessoas com a role");
        //    });
        //}

        [Command(Commands.SetRole), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.SetRole)]
        public async Task Set(SocketRole role, SocketUser user)
        {
            await (user as IGuildUser).AddRoleAsync(role);
        }

        [Command(Commands.RemoveRole), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.RemoveRole)]
        public async Task Remove(SocketRole role, SocketUser user)
        {
            await (user as IGuildUser).RemoveRoleAsync(role);
        }

        [Command(Commands.SetMentionable), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.SetMentionable)]
        public async Task SetAllMentionable()
        {
            await Task.Run(() =>
            {
                var roles = Context.Guild.Roles.Where(x => !x.IsMentionable && !x.IsEveryone).ToList();
                foreach (var role in roles)
                    role.ModifyAsync(x => { x.Mentionable = true; });

                ShowMessageRoleUpdated(roles.Count);
            });
        }

        [Command(Commands.DeleteRepeatableRoles), RequireUserPermission(GuildPermission.Administrator)]
        [Summary(Summary.DeleteRepeatableRoles)]
        public async Task DeleteRepeatableRoles()
        {
            var roles = Context.Guild.Roles.Where(x =>
                x.Name.ToLower() != CustomRoles.RaccoonsRole.ToLower() &&
                x.Name.ToLower() != CustomRoles.BotRole.ToLower() &&
                x.Name.ToLower() != CustomRoles.DarkHumorRole.ToLower() &&
                x.Name.ToLower() != CustomRoles.NsfwRole.ToLower() &&
                x.Name.ToLower() != CustomRoles.LeaderRole.ToLower() &&
                !x.IsEveryone).ToList();

            foreach (var role in roles.Select(x => x.Name).Distinct())
            {
                var usersCount = Context.Guild.Users.Where(x => x.Roles.Where(y => y.Name.Contains(role)).Count() > 0).Count();
                var subRoles = roles.Where(x => x.Name == role).ToArray();
                if (subRoles.Count() > 1)
                {
                    for (int i = 0; i < subRoles.Count() - 1; i++)
                    {
                        try
                        {
                            subRoles[i].DeleteAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            ShowMessageRoleUpdated(roles.Count);
        }

        [Command(Commands.SetHoistable), RequireOwner]
        [Summary(Summary.SetHoistable)]
        public async Task SetAllHoistable(string hoist, [Remainder] string roleChat)
        {
            var guild = Context.Guild;
            var roles = guild.Roles.Where(x =>
            x.IsHoisted != bool.Parse(hoist) &&
            !x.IsEveryone &&
            x.Name != Context.Client.CurrentUser.Username &&
            x.Name.ToLower() != CustomRoles.BotRole.ToLower() && (string.IsNullOrWhiteSpace(roleChat) || x.Name.Contains(roleChat))).ToList();

            foreach (var role in roles)
                await role.ModifyAsync(x => { x.Hoist = bool.Parse(hoist); });

            ShowMessageRoleUpdated(roles.Count);
        }

        private void ShowMessageRoleUpdated(int count)
        {
            var msg = count > 0 ? Messages.RolesUpdated : Messages.NoRolesToUpdated;
            Context.Channel.SendMessageAsync(msg);
        }
    }
}