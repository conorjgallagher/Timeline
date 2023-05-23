/* LANGUAGE 
================================================== */
typeof VMM != "undefined" && (VMM.Language =
    {
        lang: "cz",
        api: { wikipedia: "cs" },
        xrmc: {
            OpenFullRecord: "Otevřít záznam",
            Settings: "Nastavení",
            SelectDeselect: "Vybrat/Zrušit vše",
            Update: "Nastavit",
            RefreshTimeline: "Obnovit časovou osu",
            Status: "Stav",
            NoPermissions: "Nemáte správná oprávnění k použití časové osy. Obraťte se na správce systému."
        },
        date: {
            month: ["ledna", "února", "března", "dubna", "května", "června", "července", "srpna", "září", "října", "listopadu", "prosince"],
            month_abbr: ["Led", "Úno", "Bře", "Dub", "Kvě", "Čen", "Čec", "Srp", "Zář", "Říj", "Lis", "Pro"],
            day: ["neděle", "pondělí", "úterý", "středa", "čtvrtek", "pátek", "sobota"],
            day_abbr: ["Ne", "Po", "Út", "St", "Čt", "Pá", "So"]
        },
        dateformats: {
            year: "yyyy",
            month_short: "mmm",
            month: "mmmm yyyy",
            full_short: "d. mmm ",
            full: "d. mmmm'.' yyyy",
            time_no_seconds_short: "H:MM",
            time_no_seconds_small_date: "H:MM'<br/><small>'d. mmmm'.' yyyy'</small>'",
            full_long: "d. mmm'.' yyyy H:MM",
            full_long_small_date: "H:MM'<br/><small>d. mmm'.' yyyy'</small>'"
        },
        messages: {
            loading_timeline: "Načítám časovou osu... ",
            return_to_title: "Zpět na začátek",
            expand_timeline: "Rozbalit časovou osu",
            contract_timeline: "Sbalit časovou osu",
            wikipedia: "Zdroj: otevřená encyklopedie Wikipedia",
            loading_content: "Nahrávám obsah",
            loading: "Nahrávám"
        }
    });