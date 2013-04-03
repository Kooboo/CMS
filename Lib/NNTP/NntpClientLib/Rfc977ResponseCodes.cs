using System;

namespace NntpClientLib
{
    internal sealed class Rfc977ResponseCodes
    {
        public const int HelpTextFollows = 100;
        public const int DebuggingOutput = 199;

        public const int ServerReadyPostingAllowed = 200;
        public const int ServerReadyNoPostingAllowed = 201;
        public const int SlaveStatusNoted = 202;
        public const int ClosingConnection = 205;
        public const int NewsgroupSelected = 211;
        public const int NewsgroupsFollow = 215;
        public const int ArticleRetrieved = 220;
        public const int ArticleRetrievedHeadFollows = 221;
        public const int ArticleRetrievedBodyFollows = 222;
        public const int ArticleRetrievedTextSeparate = 223;
        public const int NewArticlesFollow = 230;
        public const int NewNewsgroupsFollow = 231;
        public const int ArticleTransferredOk = 235;
        public const int ArticlePostedOk = 240;

        public const int SendArticle = 335;
        public const int SendArticleToPost = 340;

        public const int ServiceDiscontinued = 400;
        public const int NoSuchNewsgroup = 411;
        public const int NoNewsgroupSelected = 412;
        public const int NoCurrentArticleSelected = 420;
        public const int NoNextArticleInThisGroup = 421;
        public const int NoPreviousArticleInThisGroup = 422;
        public const int NoSuchArticleNumber = 423;
        public const int NoSuchArticleFound = 430;

        public const int ArticleNotWanted = 435;
        public const int ArticleTransferFAiled = 436;
        public const int ArticleRejected = 437;
        public const int PostingNotAllowed = 440;
        public const int PostingFailed = 441;

        public const int CommandNotRecognized = 500;
        public const int CommandSyntaxError = 501;
        public const int CommandUnavailable = 502;
        public const int ProgramFault = 503;
    }
}

