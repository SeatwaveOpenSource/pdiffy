var path = require('path');
var paths = require('./paths');
var fileSync = require('fs');
var excludedFeatures = require('./excluded.features');

var apps = fileSync.readdirSync(paths.SRC.FEATURES);



var entries = module.exports = [];

apps.forEach(function (app) {
	if (excludedFeatures.indexOf(app) !== -1) return;

	entries.push(path.join(paths.SRC.FEATURES, app, '*.scss'));
});