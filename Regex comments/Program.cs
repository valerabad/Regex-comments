using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Data.SqlClient;
using Regex_comments;

Helper helper = new Helper();
var actionLogs = await helper.GetActionLogs();

//var actionLogs = new List<ChainAndShoppingCenterLogItem>()
//    {
//        new ChainAndShoppingCenterLogItem()
//        {
//            Description = "Chain id: 19\r\n\r\nGroupName - old: , new:\r\nIndustry - old: Gave- og interiørbutikker, new: Gave- og interiørbutikker\r\nLegalName - old: KREMMERHUSET TING & SÅNT AS, new: KREMMERHUSET TING & SÅNT AS\r\nName - old: Kremmerhuset, new: Kremmerhuset\r\nPhone - old: 73884460, new: 73884460\r\nPostalAddress - old: POSTBOKS 4430 HOSPITALSLØKKAN, new: POSTBOKS 4430 HOSPITALSLØKKAN\r\nPostalCity - old: TRONDHEIM, new: TRONDHEIM\r\nPostalZip - old: 7418, new: 7418\r\nWeb - old: www.kremmerhuset.no, new: www.kremmerhuset.no\r\nStoreLocatorUrl - old: , new:\r\nDescription - old: Kremmerhuset Ting og Sånt er en litt annerledes gavebutikk med mange fristelser. Det bugner av ting som gjør deg glad, ting som inspirerer og kanskje overrasker. Vi ønsker å gjøre ditt hjem vakkert, og det uten at du må spise havregrøt resten av måneden.\r\n\r\nUtvalget endrer seg ofte, faktisk fra uke til uke. Våre herlige innkjøpere reiser flere ganger i året til Østen for å finne varer som gjør butikkene spennende. Det meste av Kremmerhusets varer kjøpes nettopp fra Østen, og det gjør at vi har konkurransedyktige priser og unike varer. Vi utvikler hver sesong nye produkter og farger, og lager en varepakke som gjør at du alltid finner mer enn en stil hos oss., new: Kremmerhuset Ting og Sånt er en litt annerledes gavebutikk med mange fristelser. Det bugner av ting som gjør deg glad, ting som inspirerer og kanskje overrasker. Vi ønsker å gjøre ditt hjem vakkert, og det uten at du må spise havregrøt resten av måneden.\r\n\r\nUtvalget endrer seg ofte, faktisk fra uke til uke. Våre herlige innkjøpere reiser flere ganger i året til Østen for å finne varer som gjør butikkene spennende. Det meste av Kremmerhusets varer kjøpes nettopp fra Østen, og det gjør at vi har konkurransedyktige priser og unike varer. Vi utvikler hver sesong nye produkter og farger, og lager en varepakke som gjør at du alltid finner mer enn en stil hos oss.\r\n\r\nUtvalget varierer fra butikk til butikk, nettbutikken og med sesong. Besøk oss på Instagram eller Facebook for inspirasjon, de siste nyhetene og gode tilbud :-)\r\n\r\nVelkommen til Kremmerhuset!\r\nFacebook - old: Ting og sånt, new: Interiørbutikk med det meste innen interiør, belysning og gaver\r\nParentRowId - old: , new:\r\nFacebook - old: , new: www.instagram.com/kremmerhuset\r\nYouTube - old: , new:\r\nInstagram - old: , new: www.facebook.com/kremmerhuset\r\nLinkedIn - old: , new:\r\nTwitter - old: , new:\r\nTikTok - old: , new:\r\nTrumf - old: , new:\r\nTilbudsavis - old: , new:\r\nKundeavis - old: , new:\r\nUkens tilbud - old: , new:"
//        }
//    };

int affectedCount = 1;
foreach (var actionLog in actionLogs)
{
    try
    {
        var newDescription = ParseAndClearService.ProcessParseAndClear(actionLog.Description, actionLog);

        if (newDescription != actionLog.Description)
        {
            helper.UpdateDescriptionActionLogs(actionLog, newDescription);
            Helper.WriteProcessLog("Updating in DB is finished", actionLog);
            Helper.WriteProcessLog(affectedCount.ToString(), null);
            affectedCount++;
        }
    }
    catch (Exception ex) 
    {
        Helper.WriteProcessLog("Fatal exception", actionLog);

    }
    

    
}

 Console.WriteLine(affectedCount);
Console.WriteLine(DateTime.Now);
Console.ReadKey();