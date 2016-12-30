/// <binding BeforeBuild='clean' AfterBuild='copy' />
module.exports = function (grunt) {

    var options = {
        root: 'DexCMS.Tickets'
    },
    dexCMSUtilities = require('./node_modules/dexcms-core/DexCMS.Core.Client/utilities');

    var applicationGrunt = dexCMSUtilities.gruntBuilder.application(grunt, options);
    var gruntOptions = applicationGrunt.builder();

    //Configuration setup
    grunt.initConfig(gruntOptions);
    //load npm tasks
    applicationGrunt.loadTasks();

    //register tasks
    applicationGrunt.registerTasks();
};
