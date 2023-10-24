using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Data.SqlClient;
using Regex_comments;

#region test text string
string description =
    "Chain id: 19\r\n\r\n" +
    "GroupName - old: , new: TEST\r\n" +
    "Industry - old: Gave- og interiørbutikker, new: Gave- og interiørbutikker\r\n" +
    "LegalName - old: KREMMERHUSET TING & SÅNT AS, new: KREMMERHUSET TING & SÅNT AS\r\n" +
    "Name - old: Kremmerhuset Ting & Sånt, new: Kremmerhuset Ting & Sånt\r\n" +
    "Phone - old: 73884460, new: 73884460\r\n" +
    "PostalAddress - old: POSTBOKS 4430 HOSPITALSLØKKAN, new: POSTBOKS 4430 HOSPITALSLØKKAN\r\n" +
    "PostalCity - old: TRONDHEIM, new: TRONDHEIM\r\n" +
    "PostalZip - old: 7418, new: 7418\r\n" +
    "Web - old: www.kremmerhuset.no, new: www.kremmerhuset.no\r\n" +
    "StoreLocatorUrl - old: , new:\r\n" +
    "KelkooQuery - old: , new:\r\n" +
    "KelkooMerchantId - old: , new:\r\n" +
    "KelkooOfferId - old: , new:\r\n" +
    "KelkooOtherParams - old: TEST2, new:\r\n" +
    "Description - old: " +
    "Kremmerhuset Ting og Sånt er en litt annerledes gavebutikk med mange fristelser. Det bugner av ting som gjør deg glad, ting som inspirerer og kanskje overrasker. Vi ønsker å gjøre ditt hjem vakkert, og det uten at du må spise havregrøt resten av måneden." +
    "\r\n" +
    "\r\n" +
    "Utvalget endrer seg ofte, faktisk fra uke til uke. Våre herlige innkjøpere reiser flere ganger i året til Østen for å finne varer som gjør butikkene spennende. Det meste av Kremmerhusets varer kjøpes nettopp fra Østen, og det gjør at vi har konkurransedyktige priser og unike varer. Vi utvikler hver sesong nye produkter og farger, og lager en varepakke som gjør at du alltid finner mer enn en stil hos oss." +
    ", " +
    "new: " +
    "Kremmerhuset Ting og Sånt er en litt annerledes gavebutikk med mange fristelser. Det bugner av ting som gjør deg glad, ting som inspirerer og kanskje overrasker. Vi ønsker å gjøre ditt hjem vakkert, og det uten at du må spise havregrøt resten av måneden." +
    "\r\n" +
    "\r\n" +
    "Utvalget endrer seg ofte, faktisk fra uke til uke. Våre herlige innkjøpere reiser flere ganger i året til Østen for å finne varer som gjør butikkene spennende. Det meste av Kremmerhusets varer kjøpes nettopp fra Østen, og det gjør at vi har konkurransedyktige priser og unike varer. Vi utvikler hver sesong nye produkter og farger, og lager en varepakke som gjør at du alltid finner mer enn en stil hos oss.\r\n" +

    "ShortDescription - old: Ting og sånt, new: Ting og sånt\r\n" +
    "ParentRowId - old: , new: TEST3";
#endregion

Helper helper = new Helper();
var actionLogs = await helper.GetActionLogs();

//var actionLogs = new List<ChainAndShoppingCenterLogItem>()
//    {
//        new ChainAndShoppingCenterLogItem()
//        {
//            Description = "Shopping center id: 760\r\n\r\nName: Libra Kjøpesenter\r\n\r\nPhone - old: 72524355, new: 72524355\r\nMobile - old: , new: \r\nFax - old: 72521789, new: 72521789\r\nEmail - old: post@librablomster.no, new: post@librablomster.no\r\nWeb - old: www.interflora.no, new: www.interflora.no\r\nComments - old: , new: \r\nAddress - old: Maren Juels Gate 1, new: Maren Juels Gate 1\r\nPostalNumber - old: 7130, new: 7130\r\nPostalCity - old: Brekstad, new: Brekstad\r\nDescriptionShort - old: Velkommen til vår flotte butikk på Libra Kjøpesenter i kystbyen Brekstad, new: Velkommen til vår flotte butikk på Libra Kjøpesenter i kystbyen Brekstad\r\nDescriptionLong - old: SI DET MED BLOMSTER\r\nVelkommen til vår flotte butikk på Libra Kjøpesenter i kystbyen Brekstad. Vi er en familiebedrift som har holdt på i over 45 år og vi gjør vårt ytterste for at våre kunder skal bli fornøyde. Vi er medlem hos EuroFlorist og kan formidle blomster over hele verden og i Norge. \r\n\r\nÅpningstider: \r\nMandag-Fredag : 9-18 \r\nLørdag : 9-15 \r\n\r\nButikken vår finner du her, og du kan kontakte oss på \r\nTlf 72 52 43 55\r\nLibra.blomster@online.no, eller bestill hos EuroFlorist.no \r\n\r\nLibra blomster har også en flott blogg der du kan se og følge med på hva vi lager. Ta en titt davel! \r\n, new: SI DET MED BLOMSTER\r\nVelkommen til vår flotte butikk på Libra Kjøpesenter i kystbyen Brekstad. Vi er en familiebedrift som har holdt på i over 45 år og vi gjør vårt ytterste for at våre kunder skal bli fornøyde. Vi er medlem hos EuroFlorist og kan formidle blomster over hele verden og i Norge. \r\n\r\nÅpningstider: \r\nMandag-Fredag : 9-18 \r\nLørdag : 9-15 \r\n\r\nButikken vår finner du her, og du kan kontakte oss på \r\nTlf 72 52 43 55\r\nLibra.blomster@online.no, eller bestill hos EuroFlorist.no \r\n\r\nLibra blomster har også en flott blogg der du kan se og følge med på hva vi lager. Ta en titt davel!\r\n\r\n\r\nFields affected in other sibling companies: Address, PostalNumber, PostalCity"
//        }
//    };

int affectedCount = 0;
foreach (var actionLog in actionLogs)
{
    var newDescription = ParseAndClearService.ProcessParseAndClear(actionLog.Description, actionLog);

    if (newDescription != actionLog.Description)
    {
        //helper.UpdateDescriptionActionLogs(actionLog, newDescription);
        Helper.WriteProcessLog("Updating is finished", actionLog);
        Helper.WriteProcessLog(affectedCount.ToString(), null);
        affectedCount++;
    }
}

{ } Console.WriteLine(affectedCount);
Console.ReadKey();