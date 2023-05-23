﻿/**
 * @author xRM Consultancy
 */

//If the SDK namespace object is not defined, create it.
if (typeof (xRMC) == "undefined")
{ xRMC = {}; }
// Create Namespace container for functions in this library;
xRMC.timeline = {};
xRMC.timeline.settings = {
    filtered: false,
    tv: true,
    daysparam: 90,
    days: 45,
    recordcount: 0,
    entities: {}
};


var WebResource = {};
WebResource.DataParams = null;

WebResource.GetDataParams = function () { //Get any query string parameters and load them into the vals array  
    if (WebResource.DataParams == null) {
        WebResource.DataParams = '';
        var vals;
        if (location.search !== "") {
            vals = location.search.substr(1).split("&");
            for (var i in vals) {
                vals[i] = vals[i].replace(/\+/g, " ").split("=");
            }

            //look for the parameter named 'data'  
            var datavals;
            for (var j in vals) {
                if (vals[j][0].toLowerCase() == "data") {
                    datavals = decodeURIComponent(vals[j][1]).split("|");
                    for (var k in datavals) {
                        datavals[k] = datavals[k].replace(/\+/g, " ").split("=");
                    }
                    break;
                }
            }
            if (datavals) {
                WebResource.DataParams = datavals;
            }
        }
    }
    return WebResource.DataParams;
};
WebResource.CreateDataParams = function() {
    var lang = undefined;
    var timelinePosition = undefined;
    var dashboard = undefined;
    var hideintroslide = undefined;
    var data = WebResource.GetDataParams();
    for (var i in data) {
        if (data[i][0] && data[i][0].toString().toLowerCase() == 'timelineposition') {
            timelinePosition = data[i][1];
        }
        if (data[i][0] && data[i][0].toString().toLowerCase() == 'dashboard') {
            dashboard = data[i][1];
        }
        if (data[i][0] && data[i][0].toString().toLowerCase() == 'hideintroslide') {
            hideintroslide = data[i][1];
        }
        if (data[i][0] && data[i][0].toString().toLowerCase() == 'lang') {
            lang = data[i][1].trim();
        }
    }
    
    var includeEntities = undefined;
    for (var e in xRMC.timeline.settings.entities) {
        if (xRMC.timeline.settings.entities[e].include) {
            if (includeEntities == undefined) {
                includeEntities = '';
            } else {
                includeEntities += ';';
            }
            includeEntities += e + ',' + xRMC.timeline.settings.entities[e].DayLimit;
        }
    }
    var newDataParam = 'includeentities=' + includeEntities;
    if (timelinePosition) {
        newDataParam += '|timelineposition=' + timelinePosition;
    }
    if (dashboard) {
        newDataParam += '|dashboard=' + dashboard;
    }
    if (hideintroslide) {
        newDataParam += '|hideintroslide=' + hideintroslide;
    }
    if (lang) {
        newDataParam += '|lang=' + lang;
    }
    if (xRMC.timeline.settings.daysparam) {
        newDataParam += '|days=' + xRMC.timeline.settings.daysparam;
    }
    if (xRMC.timeline.settings.startdate) {
        newDataParam += '|startdate=' + xRMC.timeline.settings.startdate.toISOString();
    }
    if (xRMC.timeline.settings.enddate) {
        newDataParam += '|enddate=' + xRMC.timeline.settings.enddate.toISOString();
    }
    if (xRMC.timeline.settings.pageto) {
        newDataParam += '|pageto=' + xRMC.timeline.settings.pageto;
    }
    return 'data=' + encodeURIComponent(newDataParam);
};

if (typeof String.prototype.endsWith !== 'function') {
    String.prototype.endsWith = function(suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}

// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

xRMC.timeline.parseEntityData = function(entityData) {
    xRMC.timeline.settings.entities = {};
    xRMC.timeline.settings.filtered = true;
    var entityParams = entityData.split(';');
    for (i in entityParams) {
        var entityParam = entityParams[i].split(",");
        if (entityParam[0] != undefined && entityParam[0] != null) {
            xRMC.timeline.settings.entities[entityParam[0]] = { logicalname: entityParam[0], include: true, DayLimit: 0 };
            if (entityParam[1]) {
                xRMC.timeline.settings.entities[entityParam[0]].DayLimit = parseInt(entityParam[1]);
            }
        }
    }
};

xRMC.timeline.getEntitySettings = function(entity) {
    if (!xRMC.timeline.settings.entities[entity]) {
        if (!xRMC.timeline.settings.filtered) {
            xRMC.timeline.settings.entities[entity] = { logicalname: entity, include: true };
        } else if (xRMC.timeline.settings.entities[entity] == undefined) {
            xRMC.timeline.settings.entities[entity] = { logicalname: entity, include: false };
        }
    }
    return xRMC.timeline.settings.entities[entity];
};

var EntityTypeMap =
{
    appointment: { code: 4201, description: "Appointment" },
    campaignresponse: { code: 4401, description: "Campaign Response" },
    email: { code: 4202, description: "Email" },
    fax: { code: 4204, description: "Fax" },
    incident: { code: 3, description: "Case" },
    letter: { code: 4207, description: "Letter" },
    opportunity: { code: 3, description: "Opportunity" },
    phonecall: { code: 4210, description: "Phone Call" },
    salesorder: { code: 3, description: "Order" },
    serviceappointment: { code: 4214, description: "Service Appointment" },
    task: { code: 4212, description: "Task" }
};

xRMC.timeline.buildTimeline = function (entityName, id) {
    xRMC.timeline.settings.entityName = entityName;
    xRMC.timeline.settings.entityId = entityName;

    var timelineData;
    if (Settings.hideintroslide) {
        timelineData = {
            timeline: {
                "headline": "",
                "type": "default",
                "text": "",
                "date": []
            }
        };
    } else {
        timelineData = {
            timeline: {
                "headline": "Microsoft Dynamics CRM Timeline",
                "type": "default",
                "text": "By xRM Consultancy, www.xrmconsultancy.com",
                "date": []
            }
        };
    }

    var blankEntityData = {};
    blankEntityData.startDate = new Date();
    blankEntityData.endDate = new Date();
    blankEntityData.headline = "There is no timeline data to display.";
    blankEntityData.text = "";
    blankEntityData.asset = {};
    blankEntityData.asset.credit = "";
    if (Settings.lang == 'cz') {
        blankEntityData.headline = "Neexistují data pro zobrazení na časové ose.";
    } else if (Settings.lang == 'fr') {
        blankEntityData.headline = "Aucune donnée à afficher";
    }

    var notAuthorised = 'You do not have the correct permissions to use Timeline. Please contact your system administrator';
    if (Settings.lang == 'cz') {
        notAuthorised = 'Nemáte správná oprávnění k použití časové osy. Obraťte se na správce systému.';
    } else if (Settings.lang == 'fr') {
        notAuthorised = "Vous n'avez pas la permission d'utliser la Timeline. Contactez votre adminsitrateur";
    }

    if (!XrmServiceToolkit.Soap.IsCurrentUserRole("Timeline Reader") &&
        !XrmServiceToolkit.Soap.IsCurrentUserRole("Timeline Writer")) {
        blankEntityData.headline = notAuthorised;

        timelineData.timeline.date.push(blankEntityData);
        xRMC.timeline.data = timelineData;
        return;
    }

    xRMC.timeline.settings.tv = getTV();
    
    // Pull back all timeline entity configurations
    getEntityConfigurations();

    var entities;
    var allEntities;

    for (s in xRMC.timeline.settings.entities) {
        var entitySettings = xRMC.timeline.settings.entities[s];
        if (entitySettings.LinkQueries != null && typeof entitySettings.LinkQueries != "undefined") {
            if (allEntities) {
                allEntities = allEntities.concat(fetchTimelineEntitiesByLinkQueries(entitySettings));
            } else {
                allEntities = fetchTimelineEntitiesByLinkQueries(entitySettings);
            }
        }
    }
    entities = cleanupEntities(allEntities);
    
    $('#currentDateRange').text(formatDate(xRMC.timeline.settings.startdate) + ' - ' + formatDate(xRMC.timeline.settings.enddate));

    if (entities && entities.length > 0) {
        buildTimelineEntities(timelineData.timeline, entities);
    } else {
        timelineData.timeline.date.push(blankEntityData);
    }

    xRMC.timeline.data = timelineData;

    function formatDate(date) {
        if (!date) return "";
        var monthNames = [
          "Jan", "Feb", "Mar",
          "Apr", "May", "Jun", "Jul",
          "Aug", "Sep", "Oct",
          "Nov", "Dec"
        ];

        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();

        return day + ' ' + monthNames[monthIndex] + ' ' + year;
    }

    function fetchTimelineEntitiesByLinkQueries(es) {
        var entitiesFound = [];
        if (es.include && es.LinkQueries) {
            for (var q in es.LinkQueries) {
                if (es.LinkQueries[q]) {
                    var fetchXml = es.LinkQueries[q].format(id);
                    if (es.MaxRecords && es.MaxRecords > 0) {
                        fetchXml = fetchXml.toLowerCase().replace('<fetch', '<fetch count=\'' + es.MaxRecords.toString() + '\' page=\'1\'');
                    }

                    if (es.DateField) {
                        var dayLimit = xRMC.timeline.settings.days;
                        var startDate = xRMC.timeline.settings.startdate;
                        if (!startDate) {
                            startDate = new Date();
                            startDate.setDate(startDate.getDate() - dayLimit);
                            xRMC.timeline.settings.startdate = startDate;
                        }
                        var startDateIso = startDate.toISOString();
                        var endDate = xRMC.timeline.settings.enddate;
                        if (!endDate) {
                            endDate = new Date();
                            endDate.setDate(endDate.getDate() + dayLimit);
                            xRMC.timeline.settings.enddate = endDate;
                        }
                        var endDateIso = endDate.toISOString();
                        if (es.DateField) {
                            if (es.DateField == "actualend") {
                                fetchXml = fetchXml.toLowerCase().replace('</entity>',
                                    '<filter type="or">' +
                                    '<filter type="and">' +
                                    '<condition attribute="actualend" operator="on-or-after" value="' + startDateIso + '"/>' +
                                    '<condition attribute="actualend" operator="on-or-before" value="' + endDateIso + '"/>' +
                                    '</filter>' +
                                    '<filter type="and">' +
                                    '<condition attribute="scheduledend" operator="on-or-after" value="' + startDateIso + '"/>' +
                                    '<condition attribute="scheduledend" operator="on-or-before" value="' + endDateIso + '"/>' +
                                    '</filter>' +
                                    '</filter></entity>');
                            } else if (es.DateField == "actualclosedate") {
                                fetchXml = fetchXml.toLowerCase().replace('</entity>',
                                    '<filter type="or">' +
                                    '<filter type="and">' +
                                    '<condition attribute="actualclosedate" operator="on-or-after" value="' + startDateIso + '"/>' +
                                    '<condition attribute="actualclosedate" operator="on-or-before" value="' + endDateIso + '"/>' +
                                    '</filter>' +
                                    '<filter type="and">' +
                                    '<condition attribute="estimatedclosedate" operator="on-or-after" value="' + startDateIso + '"/>' +
                                    '<condition attribute="estimatedclosedate" operator="on-or-before" value="' + endDateIso + '"/>' +
                                    '</filter>' +
                                    '</filter></entity>');
                            } else {
                                fetchXml = fetchXml.toLowerCase().replace('</entity>',
                                    '<filter type="and">' +
                                    '<condition attribute="' + es.DateField + '" operator="on-or-after" value="' + startDateIso + '"/>' +
                                    '<condition attribute="' + es.DateField + '" operator="on-or-before" value="' + endDateIso + '"/>' +
                                    '</filter></entity>');
                            }
                        }
                    }
                    var result = XrmServiceToolkit.Soap.Fetch(fetchXml);
                    for (var r in result) {
                        entitiesFound.push(result[r]);
                    }
                }
            }
            return entitiesFound;
        }
        return [];
    }

    function getTV() {
        // LICENCEFREE - UNCOMMENT FOR LICENCE FREE VERSION!
        //return false;
        var fetchXml =
            "<fetch  mapping='logical' aggregate='true' >" +
                "<entity name='xrmc_timelineconfiguration'>" +
                    "<attribute name='xrmc_timelineconfigurationid' aggregate='count' alias='count' />" +
                    "<filter type='and'>" +
                        "<condition attribute='xrmc_licensekeystatus' operator='eq' value='922680000' />" +
                        "<condition attribute='xrmc_licensekeyexpiry' operator='on-or-after' value='" + new Date().yyyymmdd() + "' />" +
                    "</filter>" +
                "</entity>" +
            "</fetch>";
        var validConfig = XrmServiceToolkit.Soap.Fetch(fetchXml);
        if (validConfig[0].attributes['count'].formattedValue != 1) {
                return true;
        }
        return false;
    }

    function getEntityConfigurations() {
        var fetchQuery =
            '<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">' +
                '<entity name="xrmc_timelineentity">' +
                  '<attribute name="xrmc_timelineentityid" />' +
                  '<attribute name="xrmc_name" />' +
                  '<attribute name="xrmc_maxrecords" />' +
                  '<attribute name="xrmc_daylimit" />' +
                  '<attribute name="xrmc_html" />' +
                  '<attribute name="xrmc_query" />' +
                  '<attribute name="xrmc_entitylogicalname" />' +
                  '<attribute name="xrmc_datefield" />' +
                  '<attribute name="xrmc_enddatefield" />' +
                  '<attribute name="xrmc_headlinefield" />' +
                  '<attribute name="xrmc_smallicon" />' +
                  '<attribute name="xrmc_hidestatus" />' +
                  '<order attribute="xrmc_name" descending="false" />' +
                  '<filter type="and">' +
                    '<condition attribute="statecode" operator="eq" value="0" />' +
                  '</filter>' +
                    '<link-entity name="xrmc_timelineentitylink" from="xrmc_timelineentity" to="xrmc_timelineentityid" alias="aa">' +
                      '<filter type="and">' +
                        '<condition attribute="statecode" operator="eq" value="0" />' +
                        '<condition attribute="xrmc_logicalentityname" value="' + entityName + '" operator="eq"/>' +
                      '</filter>' +
                    '</link-entity>' +
                  '</entity>' +
                '</fetch>';

        var results = XrmServiceToolkit.Soap.Fetch(fetchQuery);

        if (results) {
            for (var i = 0; i < results.length; i++) {
                EntityTypeMap[results[i].attributes["xrmc_entitylogicalname"].value] = {};
                EntityTypeMap[results[i].attributes["xrmc_entitylogicalname"].value].code = -1;
                EntityTypeMap[results[i].attributes["xrmc_entitylogicalname"].value].description = results[i].attributes["xrmc_name"].value;
                var settings = xRMC.timeline.getEntitySettings(results[i].attributes["xrmc_entitylogicalname"].value);
                settings.HTML = (results[i].attributes["xrmc_html"] ? results[i].attributes["xrmc_html"].value : '');
                settings.Query = (results[i].attributes["xrmc_query"] ? results[i].attributes["xrmc_query"].value : '');
                settings.DateField = (results[i].attributes["xrmc_datefield"] ? results[i].attributes["xrmc_datefield"].value : '');
                settings.EndDateField = (results[i].attributes["xrmc_enddatefield"] ? results[i].attributes["xrmc_enddatefield"].value : '');
                settings.Thumbnail = (results[i].attributes["xrmc_smallicon"] ? results[i].attributes["xrmc_smallicon"].value : '');
                settings.HeadlineField = (results[i].attributes["xrmc_headlinefield"] ? results[i].attributes["xrmc_headlinefield"].value : '');
                settings.MaxRecords = (results[i].attributes["xrmc_maxrecords"] ? results[i].attributes["xrmc_maxrecords"].value : 0);
                settings.DayLimit = (results[i].attributes["xrmc_daylimit"] ? results[i].attributes["xrmc_daylimit"].value : 0);
                settings.HideStatus = (results[i].attributes["xrmc_hidestatus"] ? results[i].attributes["xrmc_hidestatus"].value : false);
                settings.LinkQueries = getEntityLinkQueries(results[i].id);
            }
        }
    };

    function getEntityLinkQueries(timelineEntityId) {
        var fetchQuery =
            '<fetch distinct="false" mapping="logical" output-format="xml-platform" version="1.0">' +
                '<entity name="xrmc_timelineentitylink">' +
                    '<attribute name="xrmc_timelineentitylinkid"/>' +
                    '<attribute name="xrmc_query"/>' +
                    '<filter type="and">' +
                        '<condition attribute="xrmc_logicalentityname" value="' + entityName + '" operator="eq"/>' +
                        '<condition attribute="xrmc_timelineentity" value="' + timelineEntityId + '" operator="eq"/>' +
                        '<condition attribute="statecode" value="0" operator="eq"/>' +
                    '</filter>' +
                    '<link-entity name="xrmc_timelineentity" from="xrmc_timelineentityid" to="xrmc_timelineentity" alias="aa">' +
                      '<filter type="and">' +
                        '<condition attribute="statecode" operator="eq" value="0" />' +
                      '</filter>' +
                    '</link-entity>' +
                '</entity>' +
             '</fetch>';

        var results = XrmServiceToolkit.Soap.Fetch(fetchQuery);

        var queries = [];
        if (results) {
            for (var e in results) {
                if (results[e].attributes["xrmc_query"]) {
                    queries.push(results[e].attributes["xrmc_query"].value);
                }
            }
            return queries;
        }
        return [];
    }

    function cleanupEntities(allEntities) {
        var entities = [];

        if (allEntities) {
            for (var p in allEntities) {
                var okToAdd = true;
                for (var e in entities) {
                    if (!entities[e]) {
                        okToAdd = false;
                    } else if (entities[e].id) {
                        if (entities[e].id == allEntities[p].id) {
                            okToAdd = false;
                        }
                    }
                }
                if (okToAdd) {
                    entities.push(allEntities[p]);
                }
            }
        }
        return entities;
    }

};

function buildTimelineEntities(timeline, entities) {
    var te = 2;
    for (var e = 0; e < entities.length; e++) {
        var entity = entities[e];
		var entityData = {};
        var entityName = entity.logicalName;
        var entitySettings;
        
        // Activiies are very specific. Deal with them now...
        if (entity.logicalName == "activitypointer") {
            entityData.startDate = getActivityStartDate(entity, entity.attributes["activitytypecode"].value);
            entityData.endDate = getActivityEndDate(entity, entity.attributes["activitytypecode"].value);
            entityName = entity.attributes["activitytypecode"].value;
        }

        entitySettings = xRMC.timeline.getEntitySettings(entityName);
        
        // if we have a headline deal with it up front
        if (entitySettings.HeadlineField) {
            var fields = entitySettings.HeadlineField.replace(' ', '').split(',');
            for (i = 0; i < fields.length; i++) {
                if (entity.attributes[fields[i]]) {
                    if (!entityData.headline) {
                        entityData.headline = '';
                    }
                    var fieldValue;
                    if (entity.attributes[fields[i]].formattedValue) {
                        fieldValue = entity.attributes[fields[i]].formattedValue;
                    } else {
                        fieldValue = entity.attributes[fields[i]].value;
                    }
                    entityData.headline += (fieldValue ? fieldValue + " " : "");
                }
            }
        }

		if (entity.logicalName == "opportunity") {
		    entitySettings = xRMC.timeline.getEntitySettings(entity.logicalName);
		    entityData.startDate = (
		            entity.attributes["actualclosedate"] ||
		            entity.attributes["estimatedclosedate"] ||
		            entity.attributes["createdon"] ||
		            { value: new Date() }).value;
		    entityData.endDate = entityData.startDate;
		    if (!entityData.headline) {
		        entityData.headline = (entity.attributes["name"] ? entity.attributes["name"].value : "(No Topic)");
		    }
		}
		else if (entity.logicalName == "incident") {
		    entitySettings = xRMC.timeline.getEntitySettings(entity.logicalName);
		    if (entitySettings.DateField) {
		        entityData.startDate = entity.attributes[entitySettings.DateField].value;
		    } else {
		        entityData.startDate = entity.attributes["createdon"].value;
		    }
		    if (entitySettings.EndDateField && entitySettings.EndDateField in entity.attributes && entity.attributes[entitySettings.EndDateField]) {
		        entityData.endDate = entity.attributes[entitySettings.EndDateField].value;
		    } else {
		        entityData.endDate = entity.attributes["modifiedon"].value;
		    }
		    if (!entityData.headline) {
		        entityData.headline = (entity.attributes["title"] ? entity.attributes["title"].value : "(No Title)");
		    }
		}
		else if (entity.logicalName == "salesorder") {
		    entitySettings = xRMC.timeline.getEntitySettings(entity.logicalName);
		    entityData.startDate = (entity.attributes["datefulfilled"] ? entity.attributes["datefulfilled"].value : entity.attributes["createdon"].value);
		    entityData.endDate = (entity.attributes["datefulfilled"] ? entity.attributes["datefulfilled"].value : entity.attributes["createdon"].value);
		    entityData.headline = (entity.attributes["name"] ? entity.attributes["name"].value : "(No Name)");
		} else if (entity.logicalName == "activitypointer") {
		    if (!entityData.headline) {
		        entityData.headline = (entity.attributes["subject"] ? entity.attributes["subject"].value : "(No subject)");
		    }
		} else {
		    // Activities have already been handled... So apart from that just grab the dates from the entity config...
		    entitySettings = xRMC.timeline.getEntitySettings(entity.logicalName);
		    entityData.startDate = entity.attributes[entitySettings.DateField].value;
		    if (entitySettings.EndDateField && entitySettings.EndDateField in entity.attributes && entity.attributes[entitySettings.EndDateField]) {
		        entityData.endDate = entity.attributes[entitySettings.EndDateField].value;
		    } else {
		        entityData.endDate = entity.attributes[entitySettings.DateField].value;
		    }
		    if (!entityData.headline) {
		        entityData.headline = (entity.attributes[entitySettings.HeadlineField] ? entity.attributes[entitySettings.HeadlineField].value : "(No Title)");
		    }
		}
        
		if (entitySettings.HideStatus == false) {
		    var statusLabel = 'Status';
            if (Settings.lang == 'cz') {
                statusLabel = 'Stav';
            } else if (Settings.lang == 'fr') {
                statusLabel = 'Statut';
            }
		    var statusText = (entity.logicalName == "activitypointer" || typeof entity.attributes["statuscode"] == "undefined"
                ? (typeof entity.attributes["statecode"] == "undefined" ? "undefined" : entity.attributes["statecode"].formattedValue)
                : entity.attributes["statuscode"].formattedValue);
		    if (typeof statusText != "undefined" && statusText != "undefined") {
		        entityData.text = statusLabel + ': ' + statusText;
		    } else {
		        entityData.text = statusLabel + ': n/a';
		    }
		} else {
		    entityData.text = ' ';
		}
        entityData.entityName = entityName;
        entityData.asset = {};
        entityData.asset.entityName = entityName;
        entityData.asset.id = entity.id;
		entityData.asset.credit = "";
		entityData.asset.media = "<iframe frameborder=0 class=map src='TimelineEntity.html?id=" + entity.id + "&type=" + entityName + "'></iframe>";
		    entityData.asset.thumbnail = entitySettings.Thumbnail;
		    entityData.asset.caption = "";
		    timeline.date.push(entityData);
        if (xRMC.timeline.settings.tv) {
            var be = {};
            te = te - 1;
            if (te == 0) {
                var bd1 = new Date(entityData.startDate);
                var bd2 = new Date(entityData.startDate);
                bd2.setHours(bd1.getHours() - 1);
                be.startDate = bd2;
                be.headline = "TIMELINE EVALUATION VERSION";
                be.text =
                    '<img alt="" src="img/logo.png" width="310" height="50" style="border: 0">' +
                    '<br/>' +
                    'The Timeline solution is running in evaluation mode and will show these messages randomly in your timeline.<br/>' +
                    '<br/>' +
                    'You have either not purchased a license or your license has expired. Please contact sales@xrmconsultancy.com or purchase directly from our website www.xrmconsultancy.com';
                be.asset = {};
                be.asset.credit = "";
                timeline.date.push(be);
                te = 2;
            }
        }
    }
    return timeline;
}

function getParameterByName(name) {
  name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
  var regexS = "[\\?&]" + name + "=([^&#]*)";
  var regex = new RegExp(regexS);
  var results = regex.exec(window.location.search);
  if(results == null)
    return "";
  else
    return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function getStatus(entity) {
    return (entity.attributes["statecode"] ? entity.attributes["statecode"].formattedValue : '');
}

function getEntity(id, type) {
    var entity = null;
    var entitySettings = xRMC.timeline.getEntitySettings(type);
    if (entitySettings.Query) {
        var fetchQuery = entitySettings.Query.format(id);

        var result = XrmServiceToolkit.Soap.Fetch(fetchQuery);
        if (result.length > 0)
            entity = result[0];
    }

    return entity;

}

function getActivityStartDate(activity, activitytype) {
    // For appointements always return the scheduled start
    if (activitytype == "appointment" || activitytype == "serviceappointment") {
        return activity.attributes["scheduledstart"].value;
    }
    if (activitytype != "task" && activitytype != "phonecall" &&  activitytype != "letter") {
        // If we are a custom activity, and they are using the start dates, try return the actual start, else the scheeduled start
        if (activity.attributes["actualstart"]) {
            return activity.attributes["actualstart"].value;
        }

        // try scheduled start
        if (activity.attributes["scheduledstart"]) {
            return activity.attributes["scheduledstart"].value;
        }
    }
    return getActivityEndDate(activity, activitytype);
}

function getActivityEndDate(activity, activitytype) {
    // For appointments always return the scheduled end
    if (activitytype == "appointment" || activitytype == "serviceappointment") {
        return activity.attributes["scheduledend"].value;
    }

    // If it's a task, phone call or letter try scheduled end first
    if (activitytype == "task" || activitytype == "phonecall" || activitytype == "letter") {
        if (activity.attributes["scheduledend"]) {
            return activity.attributes["scheduledend"].value;
        }
    }

    // If we still haven't got a date pass the actualend if it exists
    if (activity.attributes["actualend"]) {
        return activity.attributes["actualend"].value;
    }

    // If we still haven't got a date, custom entities tend to use scheduled end for due dates
    if (activity.attributes["scheduledend"]) {
        return activity.attributes["scheduledend"].value;
    }

    // If we get this far and the activity is open, if we have an actualstart date return it
    if (activity.attributes["statecode"].value == 0 && activity.attributes["actualstart"]) {
        return activity.attributes["actualstart"].value;
    }

    // If we still haven't got a date, try scheduled start
    if (activity.attributes["scheduledstart"]) {
        return activity.attributes["scheduledstart"].value;
    }

    // If all of the above fails return the create date as a fallback
    return activity.attributes["createdon"].value;
}

function getOpportunityEndDate(entity) {
    if (entity.attributes["statecode"].value == 0) {
        return entity.attributes["estimatedclosedate"].value;
    } else {
        return entity.attributes["actualclosedate"].value;
    }
}

function getOpportunityValue(entity) {
    if (entity.attributes["statecode"].value == 0) {
        return (entity.attributes["estimatedvalue"] ? entity.attributes["estimatedvalue"].formattedValue : '');
    } else {
        return (entity.attributes["actualvalue"] ? entity.attributes["actualvalue"].formattedValue : '');
    }
}

function getLongDateString(d) {
    var cz = false;
    var m_names = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
    var lVMM = undefined;
    if (typeof VMM != "undefined") {
        lVMM = VMM;
    } else {
        if (typeof parent.VMM != "undefined") {
            lVMM = parent.VMM;
        }
    }
    if (typeof lVMM != "undefined") {
        m_names = lVMM.Language.date.month;
        cz = (lVMM.Language.lang == 'cz');
    }
	var curr_date = d.getDate() + "";
    var curr_month = m_names[d.getMonth()]; //Months are zero based
    var curr_year = d.getFullYear();
	var a_p = "";
	var curr_hour = d.getHours();
	if (!cz) {
	    if (curr_hour < 12) {
	        a_p = "am";
	    } else {
	        a_p = "pm";
	    }
	    if (curr_hour == 0) {
	        curr_hour = 12;
	    }
	    if (curr_hour > 12) {
	        curr_hour = curr_hour - 12;
	    }
	}
	var curr_min = d.getMinutes();
	curr_min = curr_min + "";

	if (curr_min.length == 1) {
   		curr_min = "0" + curr_min;
	}
	var day_sep = " ";
	if (cz) {
	    day_sep = ". ";
	}
    return curr_date + day_sep + curr_month + " " + curr_year + " " + curr_hour + ":" + curr_min + a_p;
}

function getEntityUrl(id, entityName) {
    return 'javascript:openRecord(\'' + id + '\',\'' + entityName + '\');';
}

function createEntityAnchor(entityReference) {
    if (entityReference) {
        var id = entityReference.id;
        var entityName = entityReference.logicalName;
        var label = entityReference.name;
        var entitySettings = xRMC.timeline.getEntitySettings(entityName);
        if (entitySettings.Thumbnail) {
            return '<a style="padding-left: 20px; background: url(\'' + entitySettings.Thumbnail + '\') no-repeat center left;" href="' + getEntityUrl(id, entityName) + '">' + label + '</a>';
        } else {
            return '<a class="' + entityName + '" href="' + getEntityUrl(id, entityName) + '">' + label + '</a>';
        }
    }
    return '';
}

function createAnchor(url) {
    if (url) {
        return '<a href="' + url + '" target="blank">' + url + '</a>';
    }
    return '';
}

function convertEscapesToHtml(s) {
    if (s) {
        // if text already is in html just throw it back to the user
        if (s.indexOf("<p>") >= 0 || s.indexOf("<br>") >= 0) return s;
        return s.replace(/\n/g, '<br>');
    }
    return '';
}

function openRecord(id, entityName) {
    //Open the record
    var navigation;
    if (typeof window.parent.Xrm != "undefined") {
        navigation = window.parent.Xrm.Navigation;
    } else {
        if (typeof Xrm != "undefined") {
            navigation = Xrm.Navigation;
        }
        else { throw new Error("Context is not available."); }
    }
    var options = {
        entityId: id,
        entityName: entityName,
        openInNewWindow: false,
    };
    navigation.openForm(options);
}

function getIconUrl(etc) {
    var url;
    if (etc >= 10000) {
        // return a custom entity icon
        url = "/_Common/icon.aspx?objectTypeCode=" + etc + "&iconType=GridIcon&inProduction=1&cache=1";
    } else {
        // return a system entity icon
        url = "/_imgs/ico_16_" + etc + ".gif";
    }
    return url;
}

Date.prototype.yyyymmdd = function () {
    var yyyy = this.getFullYear().toString();
    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
    var dd = this.getDate().toString();
    return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]); // padding
};