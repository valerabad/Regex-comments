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

foreach (var actionLog in actionLogs)
{
    var newDescription = ParseAndClearService.ProcessParseAndClear(actionLog.Description, actionLog);

    if (newDescription != actionLog.Description)
    {
        helper.UpdateDescriptionActionLogs(actionLog, newDescription);
    }   
}