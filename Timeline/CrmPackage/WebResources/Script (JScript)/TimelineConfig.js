var ODataPath;
var startTime; //So that the duration time can be captured.

var accountName = "Timeline Configuration";
var serverUrl;
var configId;
var dbVersion = "";
var dbAcceptTermsDate = "";


function fnOpenTimelineConfig() {
    if (window.parent.Xrm.Utility) {
        window.parent.Xrm.Utility.openEntityForm("xrmc_timelineconfiguration", configId);
    } else {
        var varUrl = GetServerUrl() + "/main.aspx?etn=xrmc_timelineconfiguration&pagetype=entityrecord&id=%7B" + configId + "%7D";
        window.open(varUrl);
    }
}

function errorXrmc(error) {
    alert(error.message);
}


function fnLoad() {
   
    var solutionVersion = getSolutionVersion();
    configId = timelineConfigCount();
    document.getElementById("lblDate").innerHTML = dbAcceptTermsDate;
    document.getElementById("lblVersion").innerHTML = dbVersion;
    if ((configId != "") && (dbVersion == solutionVersion)) {
        document.getElementById("tblTerms").style.display = "none";
        document.getElementById("tblTimeline").style.display = "";
    }
    else {
        document.getElementById("tblTerms").style.display = "";
        document.getElementById("tblTimeline").style.display = "none";
    }
}

function fnAcceptTerms() {
    document.getElementById("acceptButton").disabled = true;
    serverUrl = GetServerUrl();
    var configId = timelineConfigCount();
    ODataPath = serverUrl + "/XRMServices/2011/OrganizationData.svc";
    startTime = new Date();
    if (configId != "")
        updateRecord(configId);
    else
        createRecord(accountName);
}

function createRecord(name) {
    var solutionVersion = getSolutionVersion();
    var currentDate = new Date();
    var xrmcTimelineconfiguration = new Object();
    xrmcTimelineconfiguration.xrmc_name = name;
    xrmcTimelineconfiguration.xrmc_termsandconditionsversion = solutionVersion;
    xrmcTimelineconfiguration.xrmc_termsandconditionsaccepted = currentDate;

    SDK.REST.createRecord(xrmcTimelineconfiguration,
                          'xrmc_timelineconfiguration',
                          TimelineReqCallBack,
                          errorXrmc); 
}


function updateRecord(id) {
    var changes = new Object();
    var currentDate = new Date();
    var solutionVersion = getSolutionVersion();
    changes.xrmc_termsandconditionsversion = solutionVersion;
    changes.xrmc_termsandconditionsaccepted = currentDate;

    SDK.REST.updateRecord(id,
                          changes,
                          'xrmc_timelineconfiguration',
                          TimelineReqCallBack,
                          errorXrmc
                         );
   
}

function TimelineReqCallBack(record) {
    fnLoad();

}

function GetServerUrl() {

    var context, crmServerUrl;
    if (typeof GetGlobalContext != "undefined") {
        context = GetGlobalContext();
    }
    else if (typeof Xrm != "undefined") {
        context = Xrm.Page.context;
    }
    else {
        if (typeof window.parent.Xrm != "undefined") {
            context = window.parent.Xrm.Page.context;
        } else {
            throw new Error("CRM context is not available.");
        }
    }

    if (context.client && context.client.getClient && context.client.getClient() == 'Outlook') {
        crmServerUrl = window.location.protocol + "//" + window.location.host;
    } else if (context.isOutlookClient && context.isOutlookClient() && !context.isOutlookOnline()) {
        crmServerUrl = window.location.protocol + "//" + window.location.host;
    } else {
        if (typeof context.getClientUrl != "undefined") {
            crmServerUrl = context.getClientUrl();
        } else {
            crmServerUrl = context.getServerUrl();
            crmServerUrl = crmServerUrl.replace(/^(http|https):\/\/([_a-zA-Z0-9\-\.]+)(:([0-9]{1,5}))?/, window.location.protocol + "//" + window.location.host);
            crmServerUrl = crmServerUrl.replace(/\/$/, ""); // remove trailing slash if any  
        }
    }

    return crmServerUrl;
}


function timelineConfigCount() {
    ODataPath = GetServerUrl() + "/XRMServices/2011/OrganizationData.svc";

    var retrieveRecordsReq = new XMLHttpRequest();
    var timelineConfigId = "";

    retrieveRecordsReq.open('GET', ODataPath + "/xrmc_timelineconfigurationSet", false);
    retrieveRecordsReq.setRequestHeader("Accept", "application/json");
    retrieveRecordsReq.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    retrieveRecordsReq.send();

    var timelineconfig = JSON.parse(retrieveRecordsReq.responseText).d.results;

    if (timelineconfig[0] == null) {
       
    }
    else {
        timelineConfigId = timelineconfig[0].xrmc_timelineconfigurationId;
        dbVersion = timelineconfig[0].xrmc_termsandconditionsversion;
        dbAcceptTermsDate = new Date(parseInt(timelineconfig[0].xrmc_termsandconditionsaccepted.substr(6)));
        
    }
    return timelineConfigId;

}

function getSolutionVersion() {
    ODataPath = GetServerUrl() + "/XRMServices/2011/OrganizationData.svc";

    var retrieveRequest = new XMLHttpRequest();
    var solutionVersion = "";

    retrieveRequest.open('GET', ODataPath + "/SolutionSet?$filter=UniqueName eq 'Timeline'", false);
    retrieveRequest.setRequestHeader("Accept", "application/json");
    retrieveRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    retrieveRequest.send();

    var timelineSolution = JSON.parse(retrieveRequest.responseText).d.results;

    if (timelineSolution[0] == null) {

    }
    else {
        solutionVersion = timelineSolution[0].Version;
    }
    return solutionVersion;
}
