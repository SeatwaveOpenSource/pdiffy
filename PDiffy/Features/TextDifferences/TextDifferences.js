function Filter() { this.init(); }

Filter.prototype.init = function () {
	this.bindToggleButtons();
}

Filter.prototype.bindToggleButtons = function () {
	var that = this;

	var toggles = document.getElementsByClassName('input-toggle');
	for (var i = 0; i < toggles.length; i++)
		toggles[i].onclick = function () { that.filter(); };

	var additionalToggles = document.getElementsByName('radioFilters');
	for (i = 0; i < additionalToggles.length; i++)
		additionalToggles[i].onclick = function () { that.filter(); };
}

/// none selected: show none 
/// site only: show all pages of each selected site
/// page only: show selected pages of all sites
/// site and page: show selected pages of selected sites
Filter.prototype.filter = function () {
	var that = this;
	var checkedSites = that.getInputTexts('site');
	var checkedPages = that.getInputTexts('page');

	var siteSpans = document.getElementsByClassName('site');

	for (var i = 0; i < siteSpans.length; i++) {
		var siteSpan = siteSpans[i];
		var pageSpan = siteSpan.nextElementSibling;
		var parentObjectBody = siteSpan.parentElement.parentElement.parentElement; //have to admit hate this, but then don't want to use jquery. revisit later.
		parentObjectBody.style.display = 'none';

		if (checkedSites.length == 0 && checkedPages.length == 0) {
			parentObjectBody.style.display = 'none';
		} else {
			var inSites = checkedSites.indexOf(siteSpan.innerText) >= 0;
			var inPages = checkedPages.indexOf(pageSpan.innerText) >= 0;

			if (inSites && inPages ||
				inSites && checkedPages.length == 0 ||
				inPages && checkedSites.length == 0) {
				parentObjectBody.style.display = 'block';
			}
		}
	}

	that.applyAdditionalFilters();
}

Filter.prototype.applyAdditionalFilters = function () {
	var selectedRadio = document.querySelectorAll('input[name=radioFilters]:checked')[0].id;
	if (selectedRadio == "OnlyDifferences") {
		var shownComparisons = document.querySelectorAll('div.page-object[style="display: block;"] span.comparison.invalid');
		for (var j = 0; j < shownComparisons.length; j++)
			shownComparisons[j].parentElement.parentElement.parentElement.style.display = 'none';

	} else if (selectedRadio == "ShowEverything") { }
}

Filter.prototype.getInputTexts = function (toggle) {
	var checkedInputs = document.querySelectorAll('input.input-toggle[toggles=' + toggle + ']:checked');
	var filters = '';
	for (var i = 0; i < checkedInputs.length; i++)
		filters += ' ' + checkedInputs[i].nextElementSibling.innerText;

	//console.log(toggle + " filters: " + filters);
	return filters;
}

var theFilter = new Filter();
theFilter.filter();