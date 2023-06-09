/* LANGUAGE 
================================================== */
typeof VMM != "undefined" && (VMM.Language =
    {
        lang: "en",
        api: { wikipedia: "en" },
        xrmc: {
            OpenFullRecord: "Open full record",
            Settings: "Settings",
            SelectDeselect: "Select/Deselct all",
            Update: "Update",
            RefreshTimeline: "Refresh Timeline",
            Status: "Status",
            NoPermissions: "You do not have the correct permissions to use Timeline. Please contact your system administrator"
        },
        date: {
            month: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
            month_abbr: ["Jan.", "Feb.", "March", "April", "May", "June", "July", "Aug.", "Sept.", "Oct.", "Nov.", "Dec."],
            day: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            day_abbr: ["Sun.", "Mon.", "Tues.", "Wed.", "Thurs.", "Fri.", "Sat."]
        },
        dateformats: {
            year: "yyyy",
            month_short: "mmm",
            month: "mmmm yyyy",
            full_short: "mmm d",
            full: "mmmm d',' yyyy",
            time_no_seconds_short: "h:MM TT",
            time_no_seconds_small_date: "h:MM TT'<br/><small>'mmmm d',' yyyy'</small>'",
            full_long: "mmm d',' yyyy 'at' h:MM TT",
            full_long_small_date: "h:MM TT'<br/><small>mmm d',' yyyy'</small>'"
        },
        messages: {
            loading_timeline: "Loading Timeline... ",
            return_to_title: "Return to Title",
            expand_timeline: "Expand Timeline",
            contract_timeline: "Contract Timeline",
            wikipedia: "From Wikipedia, the free encyclopedia",
            loading_content: "Loading Content",
            loading: "Loading"
        }
    });