using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using RaccoonBot.Domain.Command;
using RaccoonBot.Domain.Constants;
using RaccoonBot.Domain.Enum;
using RaccoonBot.Domain.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace RaccoonBot.Modules
{
    public class UserEnvironment : ModuleBase<SocketCommandContext>
    {
        private const string _rooms = "Rooms";
        private const string _friends = "Friends";

        [Command(Commands.CreateUserChannel)]
        [Summary(Summary.CreateUserChannel)]
        public async Task Create(int voice, [Remainder] string nameRooms)
        {
            var guild = Context.Guild;
            RestCategoryChannel c = null;
            RestRole r = null;
            RestRole owner = null;
            var userName = Context.Message.Author.Username;
            var idOwner = Context.Message.Author.Id.ToString();
            var nameCategory = $"{userName} {_rooms}";
            var nameRole = $"{userName} {_friends}";
            var listRooms = nameRooms.Split(",");
            var everyone = guild.Roles.FirstOrDefault(x => x.IsEveryone);
            var typeChannel = (VoiceEnum)voice;

            try
            {
                var ownerFound = guild.Roles.FirstOrDefault(x => x.Name.ToUpper() == idOwner);
                if (ownerFound == null)
                {
                    owner = await guild.CreateRoleAsync(idOwner, null, new Color(0, 0, 0), true, null);
                }
                else await (Context.Message.Author as SocketGuildUser).AddRoleAsync(ownerFound);

                var roleFound = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToUpper() == nameRole.ToUpper());
                if (roleFound == null)
                {
                    r = await guild.CreateRoleAsync(nameRole, null, new Color(), true, null);
                    await (Context.Message.Author as SocketGuildUser).AddRoleAsync(r);
                }
                else await (Context.Message.Author as SocketGuildUser).AddRoleAsync(roleFound);

                var cFound = guild.CategoryChannels.FirstOrDefault(x => x.Name.ToUpper() == nameCategory.ToUpper());
                if (cFound == null)
                {
                    c = await guild.CreateCategoryChannelAsync(nameCategory.ToUpper());

                    await PermissionOwnerRoleFoundOrNot(c, owner, ownerFound);
                    await PermissionRoleFriendsFoundOrNot(c, r, roleFound);
                }

                if (cFound != null && cFound.Channels.Count() > 0)
                    await Context.Channel.SendMessageAsync($"{ Context.Message.Author.Mention} ja possui categoria adicionada favor utilidar o comando {Commands.prefix + Commands.Ping}<voiceType> <roomNames>. Qualquer dúvida favor utilizar o comando {Commands.prefix + Commands.Help} ");
                else
                {
                    foreach (var room in listRooms)
                    {
                        RestTextChannel tc = null;
                        RestVoiceChannel vc = null;

                        switch (typeChannel)
                        {
                            case VoiceEnum.Text:
                                if (guild.TextChannels.FirstOrDefault(x => x.Name == room.ToLower().Replace(' ', '-')) == null)
                                    tc = await guild.CreateTextChannelAsync(room.ToLower().Replace(' ', '-'), x => { x.Topic = room; x.CategoryId = cFound != null ? cFound.Id : c.Id; });

                                await tc.AddPermissionOverwriteAsync(everyone, new OverwritePermissions(viewChannel: PermValue.Deny));

                                if (roleFound != null)
                                    await tc.AddPermissionOverwriteAsync(roleFound, new OverwritePermissions(viewChannel: PermValue.Allow));
                                else
                                    await tc.AddPermissionOverwriteAsync(r, new OverwritePermissions(viewChannel: PermValue.Allow));

                                break;
                            case VoiceEnum.Voice:
                                if (guild.VoiceChannels.FirstOrDefault(x => x.Name == room) == null)
                                    vc = await guild.CreateVoiceChannelAsync(room, x => { x.CategoryId = cFound != null ? cFound.Id : c.Id; });

                                await vc.AddPermissionOverwriteAsync(everyone, new OverwritePermissions(viewChannel: PermValue.Deny));

                                if (roleFound != null)
                                    await vc.AddPermissionOverwriteAsync(roleFound, new OverwritePermissions(viewChannel: PermValue.Allow));
                                else
                                    await vc.AddPermissionOverwriteAsync(r, new OverwritePermissions(viewChannel: PermValue.Allow));
                                break;
                        }
                    }

                    await Context.Channel.SendMessageAsync($"Salas de { Context.Message.Author.Mention} criadas");
                }
            }
            catch
            {
                var cFound = guild.CategoryChannels.FirstOrDefault(x => x.Name.ToUpper() == nameCategory.ToUpper());
                var roleFound = guild.Roles.FirstOrDefault(x => x.Name.ToUpper() == nameRole.ToUpper());
                var ownerRole = guild.Roles.FirstOrDefault(x => x.Name.ToUpper() == Context.Message.Author.Id.ToString());

                if (ownerRole != null)
                    await ownerRole.DeleteAsync();

                if (cFound != null)
                    await cFound.DeleteAsync();
                if (roleFound != null)
                    await roleFound.DeleteAsync();
            }

            await Task.CompletedTask;
        }



        [Command(Commands.UserAddFriend)]
        [Summary(Summary.UserAddFriend)]
        public async Task AddFriend([Remainder] string friends)
        {
            try
            {
                var guild = Context.Guild;
                var listFriends = friends.Split(' ');

                var userName = Context.Message.Author.Username;
                var nameRole = $"{userName} {_friends}";
                var roleFriend = guild.Roles.FirstOrDefault(x => x.Name == nameRole);

                foreach (var friend in listFriends)
                {
                    var friendId = DiscordHelper.GetIdFromMention(friend);
                    var user = guild.Users.FirstOrDefault(x => x.Id == friendId);
                    await user.AddRoleAsync(roleFriend);
                }
                await Context.Channel.SendMessageAsync($"Amigos de { Context.Message.Author.Mention} adicionados às salas");

            }
            catch
            {

            }
            await Task.CompletedTask;
        }

        [Command(Commands.UserRemoveFriend)]
        [Summary(Summary.UserRemoveFriend)]
        public async Task RemoveFriend([Remainder] string friends)
        {
            await Task.Run(() =>
            {
                try
                {
                    var guild = Context.Guild;
                    var listFriends = friends.Split(' ');

                    var userName = Context.Message.Author.Username;
                    var nameRole = $"{userName} {_friends}";
                    var roleFriend = guild.Roles.FirstOrDefault(x => x.Name == nameRole);

                    foreach (var friend in listFriends)
                    {
                        var friendId = DiscordHelper.GetIdFromMention(friend);
                        var user = guild.Users.FirstOrDefault(x => x.Id == friendId);
                        user.RemoveRoleAsync(roleFriend);
                    }
                    Context.Channel.SendMessageAsync(string.Format(Messages.RemoveFriendUserEnvironment, Context.Message.Author.Mention));
                }
                catch
                {

                }

            });
        }
        [Command(Commands.UserAddRoom)]
        [Summary(Summary.UserAddRoom)]
        public async Task AddRoom(int voice, [Remainder] string nameRooms)
        {

            try
            {
                var guild = Context.Guild;
                RestCategoryChannel c = null;
                var userName = Context.Message.Author.Username;
                var nameCategory = $"{userName} {_rooms}";
                var nameRole = $"{userName} {_friends}";
                var listRooms = nameRooms.Split(",");
                var everyone = guild.Roles.FirstOrDefault(x => x.IsEveryone);
                var typeChannel = (VoiceEnum)voice;

                var roleFound = guild.Roles.FirstOrDefault(x => x.Name.ToUpper() == nameRole.ToUpper());
                var cFound = guild.CategoryChannels.FirstOrDefault(x => x.Name.ToUpper() == nameCategory.ToUpper());


                foreach (var room in listRooms)
                {
                    RestTextChannel tc = null;
                    RestVoiceChannel vc = null;

                    switch (typeChannel)
                    {
                        case VoiceEnum.Text:
                            if (guild.TextChannels.FirstOrDefault(x => x.Name == room.ToLower().Replace(' ', '-')) == null)
                                tc = await guild.CreateTextChannelAsync(room.ToLower().Replace(' ', '-'), x => { x.Topic = room; x.CategoryId = cFound != null ? cFound.Id : c.Id; });

                            await tc.AddPermissionOverwriteAsync(everyone, new OverwritePermissions(viewChannel: PermValue.Deny));
                            await tc.AddPermissionOverwriteAsync(roleFound, new OverwritePermissions(viewChannel: PermValue.Allow));

                            break;
                        case VoiceEnum.Voice:
                            if (guild.VoiceChannels.FirstOrDefault(x => x.Name == room) == null)
                                vc = await guild.CreateVoiceChannelAsync(room, x => { x.CategoryId = cFound != null ? cFound.Id : c.Id; });

                            await vc.AddPermissionOverwriteAsync(everyone, new OverwritePermissions(viewChannel: PermValue.Deny));
                            await vc.AddPermissionOverwriteAsync(roleFound, new OverwritePermissions(viewChannel: PermValue.Allow));

                            break;
                    }
                }
                await Context.Channel.SendMessageAsync(string.Format(Messages.AddRoomUserEnvironment, Context.Message.Author.Mention));
            }
            catch
            {


            }


        }

        private static async Task PermissionOwnerRoleFoundOrNot(RestCategoryChannel c, RestRole owner, SocketRole ownerFound)
        {
            if (ownerFound != null)
                await c.AddPermissionOverwriteAsync(ownerFound, new OverwritePermissions(viewChannel: PermValue.Allow));
            else
                await c.AddPermissionOverwriteAsync(owner, new OverwritePermissions(viewChannel: PermValue.Allow));
        }

        private static async Task PermissionRoleFriendsFoundOrNot(RestCategoryChannel c, RestRole r, SocketRole roleFound)
        {
            if (roleFound != null)
                await c.AddPermissionOverwriteAsync(roleFound, new OverwritePermissions(viewChannel: PermValue.Allow));
            else
                await c.AddPermissionOverwriteAsync(r, new OverwritePermissions(viewChannel: PermValue.Allow));
        }


    }
}
