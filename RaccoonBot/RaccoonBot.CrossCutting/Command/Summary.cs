namespace RaccoonBot.Domain.Command
{
    public static class Summary
    {
        #region Other
        public const string Suggest = "Para sugerir melhorias para bot. Exemplo:** " + Commands.prefix + Commands.Suggest + " sugestao**";
        #endregion

        #region Manage
        public const string Ping = "Ping no bot.";

        public const string Clean = "Deleta as ultimas x mensagens. Exemplo:** " + Commands.prefix + Commands.Clean + " 5**";



        public const string InviteBot = "Cria o invite para colocar o bot no servidor.";

        public const string InviteDiscord = "Cria o invite do servidor.";
        public const string LinkRoleRoom = "Linka a sala com a role/roles desejadas para vizualicação/escrita/leitura e exclui o acesso do everyone . Exemplo: **" + Commands.prefix + Commands.LinkRoleRoom + " #general @Gaming @Music**";
        public const string Copy = "Copia a quantidade de mensagens desejadas de um chat para outro chat";
        public const string LuckyNumbers = "Envia numeros random. Exemplo **" + Commands.prefix + Commands.LuckyNumbers + " 3 10**. ou simplesmente **" + Commands.prefix + Commands.LuckyNumbers + "**";


        public const string InviteOriginDiscord = "Envia o invite do servidor oficial do bot.";

        public const string PlayingCount = "Busca quantas pessoas estão jogando o jogo escrito apos o comando.";
        public const string RemoveCategory = "Remove a categoria e as salas dela.";

        public const string Help = "Faz o sumário de comandos do bot.";
        public const string HelpAdmin = "Faz o sumário de comandos de admin do bot.";
        public const string HelpMhw = "Faz o sumário de comandos de mhw do bot.";
        public const string HelpUserChannel = "Faz o sumário de comandos dos chats privados do bot.";

        public const string MuteChannel = "Muta/Desmuta todos os membros da sala";
        public const string MuteMaster = "Ganha o cargo para mutar todos numa sala, só pode 1 por sala, se mencionar alguem transfere a responsabilidade. Exemplo **" + Commands.prefix + Commands.MuteMaster + " ou usar " + Commands.prefix + Commands.MuteMaster + " @Deus **";


        #endregion

        #region Roles

        public const string ReorderRoleGaming = "Reordena os cargos atraves de quantas pessoas tiverem";
        public const string ReorderBotRoles = "";

        public const string SetRoleAll = "Coloca a role desejada em todos do discord. Exemplo: ** " + Commands.prefix + Commands.SetRoleAll + " role**";
        public const string DelRoleAll = "Remove a role desejada em todos do discord. Exemplo:** " + Commands.prefix + Commands.DelRoleAll + " role**";

        public const string SetRole = "Coloca a role x na pessoa y. Exemplo:** " + Commands.prefix + Commands.SetRole + " @role @pessoa**";
        public const string RemoveRole = "Remove a role x na pessoa y. Exemplo:** " + Commands.prefix + Commands.RemoveRole + " @role @pessoa**";

        public const string NSFW = "Habilita/Desabilita a sala de NSFW";
        public const string BlackHumor = "Habilita/Desabilita a sala de Humor Negro";
        public const string UpdateGameRoles = "Atualiza a role de jogos para o servidor";
        public const string SetMentionable = "Coloca/Retira  menção de todas as roles";
        public const string PurgeRoles = "Deleta roles pouco atribuidos";
        public const string DeleteRepeatableRoles = "Deleta as roles repetidas";

        public const string SetHoistable = "Coloca/Retira destaque de todas as roles";

        public const string CountRoles = "Conta quantas pessoas possuem as roles.";

        public const string ApplyRoleGaming = "Você pode usar esse commando se voce quiser habilitar no seu discord a criação de roles por jogos.";

        #endregion

        #region MHW
        public const string MHWCreate = "Cria salas das armas do MHW. Exemplo:** " + Commands.prefix + Commands.MHWCreate + " Builds**";
        public const string MHWLinkRoleRoom = "Linka as armas para as suas respectivas salas. Exemplo:** " + Commands.prefix + Commands.MHWLinkRoleRoom + "**";
        public const string MHWCreateRole = "Cria somentes os perfis de armas do monster hunter. Exemplo:** " + Commands.prefix + Commands.MHWCreateRole + "**";
        public const string MHWRemove = "Remove salas das armas do MHW. Exemplo:** " + Commands.prefix + Commands.MHWRemove + " 3**";
        public const string GetWeapons = "Escreva abreviado as armas para ter acesso às salas de cada arma. Exemplo: ** " + Commands.prefix + Commands.GetWeapons + " lbg hbg ig l cb ** ... Ultilize o comando :" + Commands.prefix + Commands.AllWeapons;
        public const string AllWeapons = "Faz a listagem das armas do MHW.  Exemplo:** " + Commands.prefix + Commands.AllWeapons + " short** ou ** " + Commands.prefix + Commands.AllWeapons + " long**";
        public const string Events = "Faz a listagem dos eventos ativos no MHW";
        public const string Decoration = "Faz a busca de decorations que contenham o nome escrito . Exemplo: **" + Commands.prefix + Commands.Decoration + " critical 4 12**";
        public const string Material = "Faz a busca de drops que contenham o nome escrito . Exemplo: **" + Commands.prefix + Commands.Material + " cortex**";

        public const string Weakness = "Faz a busca da fraqueza e resistencia do monstro . Exemplo: **" + Commands.prefix + Commands.Weakness + " rathian**";
        #endregion

        #region Secret
        public const string Cheat = "Não use ;D";

        #endregion

        #region UserEnvironment
        public const string CreateUserChannel = "Cria a categoria e as suas salas privadas (0 - Text Channels, 1 Voice Channels). Exemplo: **" + Commands.prefix + Commands.CreateUserChannel + " 1 Nome da Sala 1, Nome Da Sala 2,...";
        public const string UserAddFriend = "Adiciona os amigos nas suas salas. Exemplo: **" + Commands.prefix + Commands.UserAddFriend + " @amigo1,@amigo2,@...";
        public const string UserAddRoom = "Cria as suas salas privadas (0 - Text Channels, 1 Voice Channels). Exemplo: **" + Commands.prefix + Commands.UserAddRoom + " 1 Nome da Sala 1, Nome Da Sala 2,...";
        public const string UserRemoveFriend = "Remove os amigos da suas salas. Exemplo: **" + Commands.prefix + Commands.UserAddFriend + " @amigo1,@amigo2,@...";
        public const string UserRemoveRoom = "removeChannel";
        #endregion
    }
}
