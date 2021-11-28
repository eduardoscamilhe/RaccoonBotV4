using RaccoonBot.Domain.Command;

namespace RaccoonBot.Domain.Constants
{
    public static class Messages
    {
        #region Others
        public const string ListGameMostPlayed = "Lista dos jogos mais jogados : ";

        public const string SuggestMention = "{0} deu a sugestão: {1} ";

        #endregion

        #region Roles 
        public const string RemoveRoleMessage = "A role {0} foi removida de {1} pessoas";

        public const string NullRoleMessage = "A role {0} não existe nesse servidor";
        public const string AddRoleMessage = "A role {0} foi adicionada de {1} pessoas";
        public const string LuckyNumbers = "Seus números foram: {0}";
        public const string ErrorLuckyNumber = "Favor escrever os numeros corretamente. Exemplo **" + Commands.prefix + Commands.LuckyNumbers + " 3 10**. ou simplesmente **" + Commands.prefix + Commands.LuckyNumbers + "**";
        public const string RolesUpdated = "Roles Atualizada";
        public const string NoRolesToUpdated = "Não há roles para atualizar";
        #endregion

        #region Server Status
        public const string ServerStatusCreated = "Server Status Criado";

        public const string ServerStatusRemoved = "Server Status Removido";
        #endregion

        #region MHW
        public const string WeaponsRooms = "Ambiente MHW Criado";
        public const string WeaponsRoomsRemoved = "Ambiente MHW Removido";
        #endregion

        #region Secrets
        public const string WrongSecretChannel = "Favor ir na sala {0} para efetuar sua tentativa :smiling_imp: ";
        public const string CodeCongratz = "Parabens... {0}";
        public const string ChatVisibleCongratz = "Desbloqueou a visibilidade do canal {0}. Parabéns {1}";
        #endregion

        public const string VoiceLogJoin = "{0} ENTROU a sala '{1}'";
        public const string VoiceLogLeft = "{0} SAIU da sala '{1}'";

        public const string RemoveFriendUserEnvironment = "Amigos de {0} removidos das salas";
        public const string AddRoomUserEnvironment = "Salas de {0} criadas";

        public const string IPlaySuccess = "A role {0} foi adicionado em {1}";
        public const string IPlayFail = "Não foi encontrado a role";

        public const string Welcome = "Bem vindo ao {0} {1}, para saber os comandos do discord digitar comando r.help";

    }
}
