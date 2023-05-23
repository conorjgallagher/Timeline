/* LANGUAGE 
================================================== */
typeof VMM != "undefined" && (VMM.Language =
    {
        lang: "fr",
        api: { wikipedia: "fr" },
        xrmc: {
            OpenFullRecord: "Ouvrir l'enregistrement",
            Settings: "Configurations",
            SelectDeselect: "Sélectionner/Désélectionner tout",
            Update: "Rafraichir",
            RefreshTimeline: "Rafraichir la Timeline",
            Status: "Statut",
            NoPermissions: "Vous n'avez pas la permission d'utiliser la Timeline. Contactez votre administrateur"
        },
        date: {
            month: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"],
            month_abbr: ["Jan.", "Fév.", "Mar.", "Avril", "Mai", "Juin", "Juil.", "Août", "Sept.", "Oct.", "Nov.", "Déc."],
            day: ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"],
            day_abbr: ["Dim.", "Lun.", "Mar.", "Mer.", "Jeu.", "Ven.", "Sam."]
        },
        dateformats: {
            year: "yyyy",
            month_short: "mmm",
            month: "mmmm yyyy",
            full_short: "mmm d",
            full: "mmmm d',' yyyy",
            time_no_seconds_short: "HH:MM",
            time_no_seconds_small_date: "HH:MM'<br/><small>'mmmm d',' yyyy'</small>'",
            full_long: "mmm d',' yyyy 'at' HH:MM",
            full_long_small_date: "HH:MM'<br/><small>mmm d',' yyyy'</small>'"
        },
        messages: {
            loading_timeline: "Chargement de la Timeline... ",
            return_to_title: "Retour au titre",
            expand_timeline: "Etendre Timeline",
            contract_timeline: "Refermer Timeline",
            wikipedia: "De Wikipedia, l'encyclopédie libre",
            loading_content: "Chargement du contenu",
            loading: "Chargement"
        }
    });
